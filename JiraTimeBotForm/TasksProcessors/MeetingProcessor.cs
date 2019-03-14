using System;
using System.Collections.Generic;
using System.Linq;
using JiraTimeBotForm.Configuration;
using JiraTimeBotForm.JiraIntegration;
using JiraTimeBotForm.TaskProcessors;
using JiraTimeBotForm.TaskTime;

namespace JiraTimeBotForm.TasksProcessors
{
    class MeetingProcessor : ITasksProcessor
    {
        private readonly ILog _log;

        public MeetingProcessor(ILog log)
        {
            _log = log;
        }

        public void Process(DateTime date, List<TaskTimeItem> taskTimes, Settings settings)
        {
            var jira = new JiraApi(settings, _log);

            _log.Trace($"На реальную дату {date:dd.MM.yyyy} распределение по задачам:");

            foreach (var taskTime in taskTimes.OrderByDescending(f => f.Time))
            {
                var taskName = jira.GetTaskName(taskTime.Branch);
                _log.Trace($" - [{taskTime.Branch}, коммитов {taskTime.Commits}]: {taskName} - {taskTime.Time}");
            }
        }
    }
}
