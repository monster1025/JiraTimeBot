using System;
using System.Collections.Generic;
using JiraTimeBotForm.Configuration;
using JiraTimeBotForm.JiraIntegration;
using JiraTimeBotForm.TaskProcessors;
using JiraTimeBotForm.TaskTime;

namespace JiraTimeBotForm.TasksProcessors
{
    class WorkLogTasksProcessor : ITasksProcessor
    {
        private readonly ILog _log;

        public WorkLogTasksProcessor(ILog log)
        {
            _log = log;
        }

        public void Process(DateTime date, List<TaskTimeItem> taskTimes, Settings settings)
        {
            _log.Trace($"На реальную дату {date:dd.MM.yyyy} распределение по задачам:");
            foreach (var taskTime in taskTimes)
            {
                _log.Trace($"- {taskTime.Branch} (коммитов {taskTime.Commits}): {taskTime.Time}");
            }

            var jira = new JiraApi(settings, _log);
            jira.SetTodayWorklog(taskTimes, dummy: settings.DummyMode, addCommentsToWorklog: settings.AddCommentsToWorklog);
        }
    }
}
