using System;
using System.Collections.Generic;
using System.Threading;
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
            var issues = _jiraApi.GetTodayIssues(settings, date, cancellationToken);

            foreach (var issue in issues)
            {
                workTasks.Add(new MercurialCommitItem
                {
                    Description = issue.Summary,
                    Time = issue.Updated.GetValueOrDefault(date.Value),
                    Branch = issue.Key.Value,
                    FilesAffected = 1
                });
            }

            return workTasks;
        }
    }
}
