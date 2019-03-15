using System;
using System.Collections.Generic;
using System.Linq;
using Atlassian.Jira;
using JiraTimeBotForm.Configuration;
using JiraTimeBotForm.TaskTime;

namespace JiraTimeBotForm.JiraIntegration
{
    public class JiraApi
    {
        private readonly ILog _log;

        private readonly List<string> _dummyComments = new List<string>
        {
            "Написание кода", "написание кода", "программирование", "реализация задачи", "кодинг", "код + тесты", 
            "кодинг", "написание кода и тестов"
        };

        public JiraApi(ILog log = null)
        {
            _log = log;
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

        public void SetTodayWorklog(List<TaskTimeItem> taskTimeItems, Settings settings, DateTime? date = null, bool dummy = false, bool addCommentsToWorklog = false)
        {
            var jira = Jira.CreateRestClient(settings.JiraUrl, settings.JiraUserName, settings.JiraPassword);

            if (date == null)
            {
                date = DateTime.Now.Date;
            }
            date = date.Value.Date;

            foreach (var taskTimeItem in taskTimeItems)
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
                var workLogs = issue.GetWorklogsAsync().Result;
                foreach (var workLog in workLogs)
                {
                    var timeSpent = TimeSpan.FromSeconds(workLog.TimeSpentInSeconds);
                    if (workLog.CreateDate.GetValueOrDefault().Date == date && workLog.Author == settings.JiraUserName)
                    {
                        var timeDiff = Math.Abs((timeSpent - taskTimeItem.Time).TotalMinutes);
                        if (timeDiff > 1)
                        {
                            _log.Trace($"Время отличается на {timeDiff} минут, удаляю worklog: {taskTimeItem.Branch} {workLog.Author} {workLog.CreateDate}: {workLog.TimeSpent}");
                            if (!dummy)
                            {
                                issue.DeleteWorklogAsync(workLog);
                            }
                            hasTodayWorklog = false;
                        }
                        else
                        {
                            _log.Trace($"По задаче {taskTimeItem.Branch} уже есть аналогичный worklog. Пропускаю.");
                            hasTodayWorklog = true;
                        }
                    }
                }

                if (!hasTodayWorklog)
                {
                    var timeSpentJira = $"{taskTimeItem.Time.TotalMinutes}m";
                    
                    var comment = _dummyComments.OrderBy(f=>Guid.NewGuid()).FirstOrDefault();
                    if (addCommentsToWorklog)
                    {
                        comment = taskTimeItem.Description;
                    }

                    Worklog workLogToAdd = new Worklog(timeSpentJira, date.Value, comment);
                    if (!dummy)
                    {
                        workLogToAdd = issue.AddWorklogAsync(workLogToAdd).Result;
                    }
                    _log.Trace($"Добавили Worklog для {taskTimeItem.Branch}: {workLogToAdd.Author} {workLogToAdd.CreateDate}: {workLogToAdd.TimeSpent}");
                }
            }
        }
    }
}
