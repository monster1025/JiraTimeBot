using System;
using System.Collections.Generic;
using System.Threading;
using Atlassian.Jira;
using JiraTimeBot.Configuration;
using JiraTimeBot.TaskTime.Objects;

namespace JiraTimeBot.JiraIntegration
{
    public interface IJiraApi
    {
        List<Issue> GetIssuesByJQL(string jql, Settings settings, DateTime? date = null, CancellationToken cancellationToken = default);
        string GetTaskName(string branch, Settings settings);
        List<Issue> GetWorkloggedIssuesByDate(Settings settings, DateTime? date = null, CancellationToken cancellationToken = default);
        void SetTodayWorklog(List<TaskTimeItem> taskTimeItems, Settings settings, DateTime? date = null, bool dummy = false, bool addCommentsToWorklog = false, CancellationToken cancellationToken = default);
        List<Worklog> GetManuallyWorklogged(List<TaskTimeItem> taskTimeItems, Settings settings, DateTime? date, CancellationToken cancellationToken = default);
    }
}