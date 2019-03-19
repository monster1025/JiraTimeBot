using System;
using System.Collections.Generic;
using System.Linq;
using JiraTimeBot.Configuration;
using JiraTimeBot.JiraIntegration;
using JiraTimeBot.TaskTime.Objects;

namespace JiraTimeBot.TasksProcessors
{
    public class MeetingProcessor : ITasksProcessor
    {
        private readonly ILog _log;
        private readonly JiraApi _jiraApi;

        public MeetingProcessor(ILog log, JiraApi jiraApi)
        {
            _log = log;
            _jiraApi = jiraApi;
        }

        public void Process(DateTime date, List<TaskTimeItem> taskTimes, Settings settings, bool dummyMode)
        {
            _log.Trace($"На реальную дату {date:dd.MM.yyyy} распределение по задачам:");

            foreach (var taskTime in taskTimes.OrderByDescending(f => f.Time))
            {
                var taskName = _jiraApi.GetTaskName(taskTime.Branch, settings);
                _log.Trace($" - [{taskTime.Branch}, коммитов {taskTime.Commits}]: {taskName} - {taskTime.Time}");
            }
        }
    }
}
