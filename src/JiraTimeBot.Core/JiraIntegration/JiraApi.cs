using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Atlassian.Jira;
using JiraTimeBot.Core.Configuration;
using JiraTimeBot.Core.JiraIntegration.Comments;
using JiraTimeBot.Core.TaskTime.Objects;

namespace JiraTimeBot.Core.JiraIntegration
{
    public class JiraApi
    {
        private readonly ILog _log;
        private readonly IJiraDescriptionSource _descriptionSource;

        public JiraApi(ILog log, IJiraDescriptionSource descriptionSource)
        {
            _log = log;
            _descriptionSource = descriptionSource;
        }

        public string GetTaskName(string branch, Settings settings)
        {
            var jira = Jira.CreateRestClient(settings.JiraUrl, settings.JiraUserName, settings.JiraPassword);
            try
            {
                var issue = jira.Issues.Queryable.FirstOrDefault(f => f.Key == branch);
                return issue?.Summary;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<Issue> GetIssuesByJQL(string jql, Settings settings, DateTime? date = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var jira = Jira.CreateRestClient(settings.JiraUrl, settings.JiraUserName, settings.JiraPassword);
            date = date.GetValueOrDefault(DateTime.Now.Date);

            var userName = settings.JiraUserName;

            if (string.IsNullOrEmpty(jql))
            {
                jql = $"status changed by '%USER%' during (\"%DATE%\",\"%DATE%\")";
            }

            jql = jql.Replace("%USER%", userName);
            jql = jql.Replace("%DATE%", $"{date.Value:yyyy-MM-dd}");

            try
            {
                List<Issue> affectedIssues =
                    jira.Issues.GetIssuesFromJqlAsync(jql, 100, 0, cancellationToken).Result.ToList();
                return affectedIssues;
            }
            catch (Exception ex)
            {
                _log.Error($"Ошибка получения информации из JIRA: {ex.InnerException?.Message}");
                return new List<Issue>();
            }
        }

        public List<Issue> GetWorkloggedIssuesByDate(Settings settings, DateTime? date = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            date = date.GetValueOrDefault(DateTime.Now.Date);
            var jql = "issueFunction in workLogged(\"on %DATE% by '%USER%'\")";
            return GetIssuesByJQL(jql, settings, date, cancellationToken);
        }

        private void RemoveWorklogsAddedByUser(List<TaskTimeItem> taskTimeItems, Settings settings, DateTime? date = null,  bool dummy = false, CancellationToken cancellationToken = default(CancellationToken))
        {
            var alreadyLoggedToday = GetWorkloggedIssuesByDate(settings, date);
            foreach (var issue in alreadyLoggedToday)
            {
                if (!taskTimeItems.Any(f => f.Branch.Equals(issue.Key.Value, StringComparison.InvariantCulture)))
                {
                    var workLogs = issue.GetWorklogsAsync(cancellationToken).Result;
                    var userWorklogs = workLogs.Where(w =>
                        w.CreateDate.GetValueOrDefault().Date == date && w.Author.Equals(settings.JiraUserName,
                            StringComparison.InvariantCultureIgnoreCase)).ToList();
                    foreach (var userWorklog in userWorklogs)
                    {
                        _log.Trace($"Удаляю добавленный руками WorkLog для задачи {issue.Key.Value}: {userWorklog.TimeSpent}, {userWorklog.Comment}.");

                        if (!dummy)
                        {
                            issue.DeleteWorklogAsync(userWorklog, token: cancellationToken);
                        }
                    }
                }
            }
        }

        public void SetTodayWorklog(List<TaskTimeItem> taskTimeItems, Settings settings, DateTime? date = null, bool dummy = false, bool addCommentsToWorklog = false, CancellationToken cancellationToken = default(CancellationToken))
        {
            date = date.GetValueOrDefault(DateTime.Now.Date).Date;
            var jira = Jira.CreateRestClient(settings.JiraUrl, settings.JiraUserName, settings.JiraPassword);

            //Удаляем добавленные вручную пользователем данные.
            RemoveWorklogsAddedByUser(taskTimeItems, settings, date, dummy, cancellationToken);

            foreach (TaskTimeItem taskTimeItem in taskTimeItems)
            {
                Issue issue = null;
                try
                {
                    issue = jira.Issues.Queryable.FirstOrDefault(f => f.Key == taskTimeItem.Branch);
                }
                catch (Exception)
                {

                }
                if (issue == null)
                {
                    _log.Error($"[!] Не могу найти ветку {taskTimeItem.Branch} в JIRA, пропускаю!");
                    continue;
                }

                var hasTodayWorklog = false;
                var workLogs = issue.GetWorklogsAsync(cancellationToken).Result;
                var userWorklogs = workLogs.Where(w =>
                    w.StartDate.GetValueOrDefault().Date == date && w.Author.Equals(settings.JiraUserName,
                        StringComparison.InvariantCultureIgnoreCase)).ToList();

                foreach (var workLog in userWorklogs)
                {
                    var timeSpent = TimeSpan.FromSeconds(workLog.TimeSpentInSeconds);
                    var timeDiff = Math.Abs((timeSpent - taskTimeItem.Time).TotalMinutes);
                    if (timeDiff > 1 || userWorklogs.Count > 1)
                    {
                        if (timeDiff > 1)
                        {
                            _log.Trace($"Время отличается на {timeDiff} минут, удаляю worklog: {taskTimeItem.Branch} {workLog.Author} {workLog.CreateDate}: {workLog.TimeSpent}");
                        }
                        else
                        {
                            _log.Trace($"Найдено более одного ворклога на задачу, удаляю worklog: {taskTimeItem.Branch} {workLog.Author} {workLog.CreateDate}: {workLog.TimeSpent}");
                        }
                        if (!dummy)
                        {
                            issue.DeleteWorklogAsync(workLog, token: cancellationToken);
                        }
                        hasTodayWorklog = false;
                    }
                    else
                    {
                        _log.Trace($"По задаче {taskTimeItem.Branch} уже есть аналогичный worklog. Пропускаю.");
                        hasTodayWorklog = true;
                    }
                }

                if (!hasTodayWorklog)
                {
                    var timeSpentJira = $"{taskTimeItem.Time.TotalMinutes}m";

                    var comment = _descriptionSource.GetDescription(taskTimeItem, addCommentsToWorklog, settings);

                    Worklog workLogToAdd = new Worklog(timeSpentJira, date.Value, comment);
                    if (!dummy)
                    {
                        workLogToAdd = issue.AddWorklogAsync(workLogToAdd, token: cancellationToken).Result;
                    }
                    _log.Trace($"Добавили Worklog для {taskTimeItem.Branch}: {workLogToAdd.Author} {workLogToAdd.CreateDate}: {workLogToAdd.TimeSpent}");
                }
            }
        }
    }
}
