using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Atlassian.Jira;
using JiraTimeBot.Configuration;
using JiraTimeBot.Mercurial;
using JiraTimeBot.Mercurial.Objects;

namespace JiraTimeBot.JiraIntegration
{
    public class JiraCommitEmulator : IMercurialLog
    {
        private readonly ILog _log;
        private readonly JiraApi _jiraApi;

        public JiraCommitEmulator(ILog log, JiraApi jiraApi)
        {
            _log = log;
            _jiraApi = jiraApi;
        }

        public List<MercurialCommitItem> GetMercurialLog(Settings settings,
                                    DateTime? date = null,
                                    CancellationToken cancellationToken = default(CancellationToken))
        {
            date = date.GetValueOrDefault(DateTime.Now.Date);
            var workTasks = new List<MercurialCommitItem>();
            var issues = _jiraApi.GetIssuesByJQL(settings.JiraQuery, settings, date, cancellationToken);

            foreach (var issue in issues)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return new List<MercurialCommitItem>();
                }

                List<Comment> comments = new List<Comment>();
                if (issue.Type?.Name != "Дубликат")
                {
                    _log.Trace($"Получаю комментарии по задаче {issue.Key} ({issue.Type?.Name}).");
                    comments = issue.GetCommentsAsync(cancellationToken).Result?.ToList() ?? new List<Comment>();
                }
                else
                {
                    _log.Trace($"Пропускаю комментарии по задаче {issue.Key} ({issue.Type?.Name}).");
                }

                var user = settings.JiraUserName;
                var userComments = comments.Where(f => f.Author.Equals(user, StringComparison.InvariantCultureIgnoreCase)).ToList();

                if (userComments.Any())
                {
                    foreach (var comment in userComments)
                    {
                        workTasks.Add(new MercurialCommitItem
                        {
                            Description = issue.Summary,
                            Time = issue.Updated.GetValueOrDefault(date.Value),
                            Branch = issue.Key.Value,
                            FilesAffected = 1
                        });
                    }
                }
                else
                {
                    workTasks.Add(new MercurialCommitItem
                    {
                        Description = issue.Summary,
                        Time = issue.Updated.GetValueOrDefault(date.Value),
                        Branch = issue.Key.Value,
                        FilesAffected = 1
                    });
                }
            }

            return workTasks;
        }
    }
}
