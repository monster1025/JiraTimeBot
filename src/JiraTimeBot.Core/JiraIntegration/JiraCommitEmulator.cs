using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Atlassian.Jira;
using JiraTimeBot.Core.Configuration;
using JiraTimeBot.Core.Mercurial;
using JiraTimeBot.Core.Mercurial.Objects;

namespace JiraTimeBot.Core.JiraIntegration
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
                var comments = issue.GetCommentsAsync(cancellationToken).Result?.ToList() ?? new List<Comment>();

                var user = settings.JiraUserName;
                //user = "Zoya.Aleksandridi";
                var userComments = comments.Where(f => f.Author.Equals(user, StringComparison.InvariantCultureIgnoreCase)).ToList();
                _log.Trace($"Получены комментарии по задаче {issue.Key} ({issue.Type?.Name}): {userComments.Count}.");

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
