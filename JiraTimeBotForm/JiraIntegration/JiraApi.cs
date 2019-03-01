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
        private readonly Settings _settings;
        private readonly ILog _log;
        private Jira _jira;

        public JiraApi(Settings settings, ILog log = null)
        {
            _settings = settings;
            _log = log;
            _jira = Jira.CreateRestClient(_settings.JiraUrl, _settings.JiraUserName, _settings.JiraPassword);
        }
        
        public void SetTodayWorklog(List<TaskTimeItem> taskTimeItems, DateTime? date = null)
        {
            if (date == null)
            {
                date = DateTime.Now.Date;
            }
            date = date.Value.Date;

            foreach (var taskTimeItem in taskTimeItems)
            {
                var issue = _jira.Issues.Queryable.FirstOrDefault(f => f.Key == taskTimeItem.Branch);
                if (issue == null)
                {
                    _log.Error($"Не могу найти ветку {taskTimeItem.Branch}, пропускаю!");
                    continue;
                }

                var hasTodayWorklog = false;
                var workLogs = issue.GetWorklogsAsync().Result;
                foreach (var workLog in workLogs)
                {
                    var timeSpent = TimeSpan.FromSeconds(workLog.TimeSpentInSeconds);
                    if (workLog.CreateDate.GetValueOrDefault().Date == date && workLog.Author == _settings.JiraUserName)
                    {
                        var timeDiff = (timeSpent - taskTimeItem.Time).TotalMinutes;
                        if (timeDiff > 1)
                        {
                            _log.Trace($"Время отличается на {timeDiff} минут, удаляю worklog: {taskTimeItem.Branch} {workLog.Author} {workLog.CreateDate}: {workLog.TimeSpent}");
                            issue.DeleteWorklogAsync(workLog, WorklogStrategy.RetainRemainingEstimate);
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
                    var addedWorklog = issue.AddWorklogAsync(new Worklog(timeSpentJira, date.Value)).Result;
                    _log.Trace($"Добавили Worklog для {taskTimeItem.Branch}: {addedWorklog.Author} {addedWorklog.CreateDate}: {addedWorklog.TimeSpent}");
                }
            }
        }
    }
}
