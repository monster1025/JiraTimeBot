using System;
using System.Collections.Generic;
using System.Threading;
using JiraTimeBot.Configuration;
using JiraTimeBot.JiraIntegration;
using JiraTimeBot.TaskTime.Objects;

namespace JiraTimeBot.TasksProcessors
{
    public class WorkLogTasksProcessor : ITasksProcessor
    {
        private readonly ILog _log;
        private readonly JiraApi _jiraApi;

        public WorkLogTasksProcessor(ILog log, JiraApi jiraApi)
        {
            _log = log;
            _jiraApi = jiraApi;
        }

        public void Process(DateTime setForDate, DateTime realDate, List<TaskTimeItem> taskTimes, Settings settings, bool dummyMode, CancellationToken cancellationToken = default(CancellationToken))
        {
            _log.Trace($"На реальную дату {realDate:dd.MM.yyyy} распределение по задачам:");
            foreach (var taskTime in taskTimes)
            {
                _log.Trace($"- {taskTime.Branch} (коммитов {taskTime.Commits}): {taskTime.Time}");
            }

            _jiraApi.SetTodayWorklog(taskTimes, settings, date: setForDate, dummy: dummyMode, addCommentsToWorklog: settings.AddCommentsToWorklog, cancellationToken: cancellationToken);
        }
    }
}
