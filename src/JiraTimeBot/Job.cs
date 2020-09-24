using JiraTimeBot.Configuration;
using JiraTimeBot.RepositoryProviders.Interfaces;
using JiraTimeBot.TasksProcessors;
using JiraTimeBot.TaskTime;
using JiraTimeBot.TaskTime.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JiraTimeBot.JiraIntegration;

namespace JiraTimeBot
{
    class Job
    {
        private readonly IAllRepositoryProviders _repositoryProviders;
        private readonly ITaskTimeCalculator _taskTimeDiscoverer;
        private readonly IJiraApi _jiraApi;
        private readonly ILog _log;

        public Job(IAllRepositoryProviders repositoryProviders, 
                   ITaskTimeCalculator taskTimeDiscoverer, 
                   IJiraApi jiraApi, 
                   ILog log)
        {
            _repositoryProviders = repositoryProviders;
            _taskTimeDiscoverer = taskTimeDiscoverer;
            _jiraApi = jiraApi;
            _log = log;
        }

        public Task DoTheJob(Settings settings, ITasksProcessor tasksProcessor, bool dummyMode, CancellationToken cancellationToken)
        {
            _log.Info("Начинаем работу");

            return Task.Run(() => DoTheJobImpl(settings, tasksProcessor, dummyMode, cancellationToken), cancellationToken);
        }

        private void DoTheJobImpl(Settings settings, 
                                  ITasksProcessor tasksProcessor, 
                                  bool dummyMode, 
                                  CancellationToken cancellationToken)
        {
            int daysDiff = 0;
            if (tasksProcessor is MeetingProcessor)
            {
                daysDiff = -1;
            }

            var setForDate = DateTime.Now.Date;
            while (true)
            {
                DateTime realDate = DateTime.Now.Date.AddDays(daysDiff);

                if (SetTaskTimesForDateImpl(setForDate, realDate, settings, tasksProcessor, dummyMode, cancellationToken))
                {
                    return;
                }
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                _log.Warn($"{realDate:dd.MM.yyyy} вы не сделали ничего полезного =) Использую предыдущий день.");
                daysDiff--;
                if (daysDiff < -7)
                {
                    _log.Error("Не нашли ни одного коммита за предыдущие 7 дней. Возможно вы в отпуске? Выхожу.");
                    return;
                }
            }
        }

        public Task SetTaskTimesForDate(DateTime setForDate, DateTime realDate, Settings settings, ITasksProcessor tasksProcessor, bool dummyMode, CancellationToken cancellationToken)
        {
            _log.Info("Начинаем работу");

            return Task.Run(() => SetTaskTimesForDateImpl(setForDate, realDate, settings, tasksProcessor, dummyMode, cancellationToken), cancellationToken);
        }

        private bool SetTaskTimesForDateImpl(DateTime setForDate,
                                             DateTime realDate,
                                             Settings settings,
                                             ITasksProcessor tasksProcessor,
                                             bool dummyMode,
                                             CancellationToken cancellationToken)
        {
            List<TaskTimeItem> tasks = new List<TaskTimeItem>();
            if (tasksProcessor is MeetingProcessor)
            {
                var provider = _repositoryProviders.JiraHistory;
                tasks = provider.GetDayWorklogs(realDate, settings, cancellationToken);
                return SetTaskTimesForDate(setForDate, realDate, settings, tasksProcessor, dummyMode, tasks, cancellationToken);
            }

            switch (settings.WorkType)
            {
                case WorkType.Mercurial:
                {
                    var repo = _repositoryProviders.JiraCommitEmulator;
                    tasks = GetTasksFromRepositories(realDate, settings, new[] {repo});
                    break;
                }
                case WorkType.JiraLogs:
                {
                    var repo = _repositoryProviders.JiraCommitEmulator;
                    tasks = GetTasksFromRepositories(realDate, settings, new[] {repo});
                    break;
                }
                case WorkType.GitLogs:
                {
                    var repo = _repositoryProviders.GitLog;
                    tasks = GetTasksFromRepositories(realDate, settings, new[] {repo});
                    break;
                }
                case WorkType.CVSMixed:
                {
                    var repos = new[] {_repositoryProviders.GitLog as IRepositoryLog, _repositoryProviders.MercurialLog};
                    tasks = GetTasksFromRepositories(realDate, settings, repos);
                    break;
                }
                default:
                {
                    return false;
                }
            }

            var result = SetTaskTimesForDate(setForDate, realDate, settings, tasksProcessor, dummyMode, tasks, cancellationToken);
            return result;
        }

        private List<TaskTimeItem> GetTasksFromRepositories(DateTime realDate, 
                                                            Settings settings,  
                                                            IEnumerable<IRepositoryLog> repositories)
        {
            var result = new List<TaskTimeItem>();
            foreach (var repositoryLog in repositories)
            {
                foreach (var repoPath in settings.GetRepositories())
                {
                    var items = repositoryLog.GetRepositoryLog(settings, repoPath, realDate);
                    result.AddRange(items);
                }
            }
            return result;
        }


        private bool SetTaskTimesForDate(DateTime setForDate, 
                                         DateTime realDate, 
                                         Settings settings, 
                                         ITasksProcessor tasksProcessor, 
                                         bool dummyMode, 
                                         List<TaskTimeItem> taskTimes,
                                         CancellationToken cancellationToken)
        {
            if (!(tasksProcessor is MeetingProcessor))
            {
                List<TaskTimeItem> commits = taskTimes;
                ModifyWorktimeByManualLogs(setForDate, settings, cancellationToken, commits);
                taskTimes = _taskTimeDiscoverer.CalculateTaskTime(commits, settings, cancellationToken);
            }
            else
            {
                var provider = _repositoryProviders.JiraHistory;
                taskTimes = provider.GetDayWorklogs(realDate, settings, cancellationToken);
            }

            if (cancellationToken.IsCancellationRequested)
            {
                return false;
            }

            if (!taskTimes.Any())
            {
                return false;
            }

            tasksProcessor.Process(setForDate, realDate, taskTimes, settings, dummyMode, cancellationToken);

            _log.Info("Готово.");
            return true;
        }

        private void ModifyWorktimeByManualLogs(DateTime setForDate, Settings settings, CancellationToken cancellationToken,
            List<TaskTimeItem> commits)
        {
            if (settings.RemoveManuallyAddedWorklogs)
            {
                return;
            }

            //если не удаляем добавленное вручную, то посчитаем сколько там надобавляли и отнимем от рабочего дня
            var manuallyWorklogged = _jiraApi.GetManuallyWorklogged(commits, settings, setForDate, cancellationToken);
            if (manuallyWorklogged.Any() && manuallyWorklogged.Sum(f => f.TimeSpentInSeconds) < settings.MinuterPerWorkDay)
            {
                settings.MinuterPerWorkDay = (int) (settings.MinuterPerWorkDay - manuallyWorklogged.Sum(f => f.TimeSpentInSeconds));
            }
        }
    }
}
