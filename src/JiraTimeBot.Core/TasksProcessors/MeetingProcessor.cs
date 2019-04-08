using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using JiraTimeBot.Core.Configuration;
using JiraTimeBot.Core.JiraIntegration;
using JiraTimeBot.Core.TaskTime.Objects;

namespace JiraTimeBot.Core.TasksProcessors
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

        public void Process(DateTime setForDate, DateTime realDate, List<TaskTimeItem> taskTimes, Settings settings, bool dummyMode, CancellationToken cancellationToken = default(CancellationToken))
        {
            _log.Trace($"На реальную дату {realDate:dd.MM.yyyy} распределение по задачам:");

            foreach (var taskTime in taskTimes.OrderByDescending(f => f.Time))
            {
                var taskName = _jiraApi.GetTaskName(taskTime.Branch, settings);
                _log.Trace($" - [{taskTime.Branch}, коммитов {taskTime.Commits}]: {taskName} - {taskTime.Time}");
            }
        }
    }
}
