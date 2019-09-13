using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JiraTimeBot.Configuration;
using JiraTimeBot.TaskTime.Objects;

namespace JiraTimeBot.JiraIntegration
{
    public interface IJiraHistory
    {
        List<TaskTimeItem> GetDayWorklogs(DateTime date,
                                          Settings settings,
                                          CancellationToken cancellationToken = default(CancellationToken));
    }

    public class JiraHistory: IJiraHistory
    {
        private readonly IJiraApi _jiraApi;

        public JiraHistory(IJiraApi jiraApi)
        {
            _jiraApi = jiraApi;
        }

        public List<TaskTimeItem> GetDayWorklogs(DateTime date,
                                                 Settings settings,
                                                 CancellationToken cancellationToken = default(CancellationToken))
        {
            var tasks = _jiraApi.GetIssuesByJQL("status changed by '%USER%' during (\"%DATE%\",\"%DATE%\")", settings, date, cancellationToken);
            List<TaskTimeItem> resultItems = new List<TaskTimeItem>();
            foreach (var issue in tasks)
            {
                var workLogs = issue.GetWorklogsAsync(cancellationToken).Result;
                var user = settings.JiraUserName;
                //user = "Zoya.Aleksandridi";
                var userWorklogs = workLogs.Where(w =>
                    w.CreateDate.GetValueOrDefault().Date == date && w.Author.Equals(user,
                        StringComparison.InvariantCultureIgnoreCase)).ToList();
                if (!userWorklogs.Any())
                {
                    continue;
                }

                var firstWorklog = userWorklogs.First();

                var description = string.Join("\r\n", userWorklogs.Select(f => f.Comment).Distinct());
                var totalTime = TimeSpan.FromSeconds(userWorklogs.Sum(f => f.TimeSpentInSeconds));
                var startDate = firstWorklog?.CreateDate ?? date;
                var commits = userWorklogs.Sum(f => f.Comment.Count(c => c == '\n'));
                var taskType = CommitType.Task;
                if (description.Contains("Подготовка и публикация"))
                {
                    taskType = CommitType.Release;
                }

                resultItems.Add(new TaskTimeItem(issue.Key.Value, description, issue.Project, startDate, totalTime, commits, 1, "", taskType));   
            }

            return resultItems;
        }
    }
}
