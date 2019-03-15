using System;
using System.Collections.Generic;
using JiraTimeBotForm.Configuration;
using JiraTimeBotForm.JiraIntegration;
using JiraTimeBotForm.TaskTime;
using JiraTimeBotForm.TaskTime.Objects;

namespace JiraTimeBotForm.TasksProcessors
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

        public void Process(DateTime date, List<TaskTimeItem> taskTimes, Settings settings, bool dummyMode)
        {
            _log.Trace($"На реальную дату {date:dd.MM.yyyy} распределение по задачам:");
            foreach (var taskTime in taskTimes)
            {
                _log.Trace($"- {taskTime.Branch} (коммитов {taskTime.Commits}): {taskTime.Time}");
            }

            _jiraApi.SetTodayWorklog(taskTimes, settings, dummy: dummyMode, addCommentsToWorklog: settings.AddCommentsToWorklog);
        }
    }
}
