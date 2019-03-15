using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JiraTimeBotForm.Configuration;
using JiraTimeBotForm.Mercurial;
using JiraTimeBotForm.Mercurial.Objects;
using JiraTimeBotForm.TasksProcessors;
using JiraTimeBotForm.TaskTime;
using JiraTimeBotForm.TaskTime.Objects;

namespace JiraTimeBotForm
{
    class Job
    {
        private readonly IMercurialLog _mercurialLog;
        private readonly ITaskTimeDiscoverer _taskTimeDiscoverer;
        private readonly ILog _log;

        public Job(IMercurialLog mercurialLog, ITaskTimeDiscoverer taskTimeDiscoverer, ILog log)
        {
            _mercurialLog = mercurialLog;
            _taskTimeDiscoverer = taskTimeDiscoverer;
            _log = log;
        }

        public Task DoTheJob(Settings settings, ITasksProcessor tasksProcessor, CancellationToken cancellationToken)
        {
            _log.Info("Начинаем работу");

            return Task.Run(() => DoTheJobImpl(settings, tasksProcessor, cancellationToken), cancellationToken);
        }

        private void DoTheJobImpl(Settings settings, ITasksProcessor tasksProcessor, CancellationToken cancellationToken)
        {
            int daysDiff = 0;
            if (tasksProcessor is MeetingProcessor)
            {
                daysDiff = -1;
            }

            while (true)
            {
                DateTime date = DateTime.Now.Date.AddDays(daysDiff);

                List<MercurialCommitItem> commits = _mercurialLog.GetMercurialLog(settings, date, cancellationToken);
                List<TaskTimeItem> taskTimes = _taskTimeDiscoverer.GetTaskTimes(settings, commits, cancellationToken);

                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                if (!taskTimes.Any())
                {
                    _log.Warn($"{date:dd.MM.yyyy} вы не сделали ничего полезного =) Использую предыдущий день.");
                    daysDiff--;
                    if (daysDiff < -7)
                    {
                        _log.Error("Не нашли ни одного коммита за предыдущие 7 дней. Возможно вы в отпуске? Выхожу.");
                        return;
                    }

                    continue;
                }

                tasksProcessor.Process(date, taskTimes, settings);

                _log.Info("Готово.");
                return;
            }
        }

    }
}
