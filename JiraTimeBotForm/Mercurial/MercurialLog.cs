using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using JiraTimeBotForm.Configuration;
using Mercurial;

namespace JiraTimeBotForm.Mercurial
{
    public class MercurialLog
    {
        private readonly ILog _log;

        public MercurialLog(ILog log)
        {
            _log = log;
        }

        public List<MercurialCommitItem> GetMercurialLog(Settings settings, DateTime? date = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            date = date.GetValueOrDefault(DateTime.Now.Date);

            var workTasks = new List<MercurialCommitItem>();
            foreach (var repoDirectory in Directory.GetDirectories(settings.RepositoryPath))
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return new List<MercurialCommitItem>();
                }

                //var project = new DirectoryInfo(repoDirectory).Name;
                if (!Directory.Exists(Path.Combine(repoDirectory, ".hg")))
                {
                    continue;
                }

                var repo = new Repository(repoDirectory);
                var logCommand = new LogCommand
                {
                    Date = date,
                    Users = { settings.MercurialAuthorEmail },
                    AdditionalArguments = { "--encoding=utf-8"},
                };

                var log = repo.Log(logCommand);

                foreach (var changeset in log)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return new List<MercurialCommitItem>();
                    }

                    var commitMessage = FixEncoding(changeset.CommitMessage);
                    if (IsNeedToSkip(changeset.Branch, commitMessage))
                    {
                        continue;
                    }
                    commitMessage = StripTechnicalInfo(commitMessage);

                    workTasks.Add(new MercurialCommitItem
                    {
                        Description = commitMessage,
                        Branch = changeset.Branch,
                        Time = changeset.Timestamp,
                        FilesAffected = changeset.PathActions.Count
                    });
                    _log?.Trace($" - Найден changeset: {changeset.Timestamp} - {changeset.Branch} - {changeset.AuthorEmailAddress} - {commitMessage}");
                }
            }
            if (!workTasks.Any())
            {
                return new List<MercurialCommitItem>();
            }

            return workTasks;
        }

        
        private string StripTechnicalInfo(string commitMessage)
        {
            if (!commitMessage.Contains("Signed-by:"))
            {
                return commitMessage;
            }

            var arr = commitMessage.Split('\n');
            var sb = new StringBuilder();
            foreach (var itm in arr)
            {
                if (string.IsNullOrEmpty(itm))
                {
                    continue;
                }
                if (itm.StartsWith("Signed-by"))
                {
                    continue;
                }
                if (itm.StartsWith("Jira:"))
                {
                    continue;
                }

                sb.AppendLine(itm);
            }

            var message = sb.ToString();
            message = message.TrimEnd('\n');
            message = message.TrimEnd('\r');

            return message;
        }

        private bool IsNeedToSkip(string branch, string commitMessage)
        {
            if (!branch.Contains("-"))
            {
                return true;
            }

            //Пропускаем Close и Merge коммиты
            if (commitMessage.StartsWith($"Close {branch} ", StringComparison.InvariantCultureIgnoreCase) ||
                commitMessage.StartsWith($"Merge with ", StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }

            return false;
        }

        private string FixEncoding(string source)
        {
            //перекодируем сообщение - ибо оно криво забирается в 1252
            Encoding srcEncodingFormat = Encoding.GetEncoding("windows-1252");
            byte[] originalByteString = srcEncodingFormat.GetBytes(source);
            var commitMessage = Encoding.UTF8.GetString(originalByteString);
            return commitMessage;
        }
    }
}
