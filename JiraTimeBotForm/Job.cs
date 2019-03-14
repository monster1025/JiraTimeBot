using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JiraTimeBotForm.Configuration;
using JiraTimeBotForm.TaskProcessors;
using JiraTimeBotForm.TaskTime;

namespace JiraTimeBotForm
{
    class Job
    {
        private readonly ILog _log;

        public Job(ILog log)
        {
            _log = log;
        }

        public Task DoTheJob(Settings settings, ITasksProcessor tasksProcessor, CancellationToken cancellationToken)
        {
            _log.Info("Начинаем работу");

            return Task.Run(() => DoTheJobImpl(settings, tasksProcessor, cancellationToken), cancellationToken);
        }

        private void DoTheJobImpl(Settings settings, ITasksProcessor tasksProcessor, CancellationToken cancellationToken)
        {
            var taskDiscoverer = new TaskTimeDiscoverer(_log);

            int daysDiff = 0;

            while (true)
            {
                DateTime date = DateTime.Now.Date.AddDays(daysDiff);

                List<TaskTimeItem> taskTimes = taskDiscoverer.GetTaskTimes(settings, date, cancellationToken);

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
