using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using JiraTimeBot.Core.Configuration;
using JiraTimeBot.Core.Mercurial.Modifiers;
using JiraTimeBot.Core.Mercurial.Objects;
using Mercurial;

namespace JiraTimeBot.Core.Mercurial
{
    public class MercurialLog: IMercurialLog
    {
        private readonly ILog _log;
        private readonly ICommitSkipper _commitSkipper;
        private readonly ITechnicalInfoSkipper _technicalInfoSkipper;

        public MercurialLog(ILog log, ICommitSkipper commitSkipper, ITechnicalInfoSkipper technicalInfoSkipper)
        {
            _log = log;
            _commitSkipper = commitSkipper;
            _technicalInfoSkipper = technicalInfoSkipper;
        }

        public List<MercurialCommitItem> GetMercurialLog(Settings settings, DateTime? date = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrEmpty(settings.RepositoryPath) || !Directory.Exists(settings.RepositoryPath))
            {
                _log.Error("Папка с репо не сушествует.");
                return new List<MercurialCommitItem>();
            }

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
                    if (_commitSkipper.IsNeedToSkip(changeset.Branch, commitMessage))
                    {
                        continue;
                    }
                    commitMessage = _technicalInfoSkipper.StripTechnicalInfo(commitMessage);

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
