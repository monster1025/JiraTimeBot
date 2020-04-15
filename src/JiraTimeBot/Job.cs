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
        private readonly IAllRepositoryProviders _mercurialProviders;
        private readonly ITaskTimeCalculator _taskTimeDiscoverer;
        private readonly IJiraApi _jiraApi;
        private readonly ILog _log;

        public Job(IAllRepositoryProviders mercurialProviders, ITaskTimeCalculator taskTimeDiscoverer, IJiraApi jiraApi, ILog log)
        {
            _mercurialProviders = mercurialProviders;
            _taskTimeDiscoverer = taskTimeDiscoverer;
            _jiraApi = jiraApi;
            _log = log;
        }

        public Task DoTheJob(Settings settings, ITasksProcessor tasksProcessor, bool dummyMode, CancellationToken cancellationToken)
        {
            _log.Info("Начинаем работу");

            return Task.Run(() => DoTheJobImpl(settings, tasksProcessor, dummyMode, cancellationToken), cancellationToken);
        }

        private void DoTheJobImpl(Settings settings, ITasksProcessor tasksProcessor, bool dummyMode, CancellationToken cancellationToken)
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

        private bool SetTaskTimesForDateImpl(DateTime setForDate, DateTime realDate, Settings settings, ITasksProcessor tasksProcessor, bool dummyMode, CancellationToken cancellationToken)
        {
            IRepositoryLog repository = _mercurialProviders.MercurialLog;
            if (settings.WorkType == WorkType.JiraLogs)
            {
                _log.Info("Использую Jira как источник информации.");
                repository = _mercurialProviders.JiraCommitEmulator;
            }
            else if (settings.WorkType == WorkType.GitLogs)
            {
                _log.Info("Использую Git как источник информации.");
                repository = _mercurialProviders.GitLog;
            }

            List<TaskTimeItem> taskTimes;
            if (!(tasksProcessor is MeetingProcessor))
            {
                List<TaskTimeItem> commits = repository.GetRepositoryLog(settings, realDate, cancellationToken);
                ModifyWorktimeByManualLogs(setForDate, settings, cancellationToken, commits);
                taskTimes = _taskTimeDiscoverer.CalculateTaskTime(commits, settings, cancellationToken);
            }
            else
            {
                var provider = _mercurialProviders.JiraHistory;
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
