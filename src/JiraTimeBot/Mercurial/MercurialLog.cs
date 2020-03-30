using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using JiraTimeBot.Configuration;
using JiraTimeBot.Mercurial.Modifiers;
using JiraTimeBot.TaskTime.Objects;
using Mercurial;

namespace JiraTimeBot.Mercurial
{
    public class MercurialLog: IRepositoryLog
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
        
        public List<TaskTimeItem> GetRepositoryLog(Settings settings, DateTime? date = null, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(settings.RepositoryPath) || !Directory.Exists(settings.RepositoryPath))
            {
                _log.Error("Папка с репо не сушествует.");
                return new List<TaskTimeItem>();
            }

            date = date.GetValueOrDefault(DateTime.Now.Date);

            var workTasks = new List<TaskTimeItem>();
            foreach (var repoDirectory in Directory.GetDirectories(settings.RepositoryPath))
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return new List<TaskTimeItem>();
                }

                //var project = new DirectoryInfo(repoDirectory).Name;
                if (!Directory.Exists(Path.Combine(repoDirectory, ".hg")))
                {
                    continue;
                }

                var directoryInfo = new DirectoryInfo(repoDirectory);

                var repo = new Repository(repoDirectory);

                if (settings.PullBeforeProcess)
                {
                    try
                    {
                        repo.Pull();
                    }
                    catch (Exception ex)
                    {
                        _log?.Trace($" - Не получается сделать pull для репозитория: {directoryInfo.Name} - {ex.Message}");
                    }
                }

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
                        return new List<TaskTimeItem>();
                    }

                    var commitMessage = FixEncoding(changeset.CommitMessage);
                    if (_commitSkipper.IsNeedToSkip(changeset.Branch, commitMessage))
                    {
                        continue;
                    }
                    commitMessage = _technicalInfoSkipper.StripTechnicalInfo(commitMessage);

                    var task = new TaskTimeItem(changeset.Branch,
                        commitMessage,
                        directoryInfo.Name,
                        changeset.Timestamp,
                        TimeSpan.Zero,
                        1,
                        changeset.PathActions.Count, 
                        "",
                        GetCommitType(changeset.Branch));

                    if (task.Type == CommitType.Release && task.Description.StartsWith("Merge with", StringComparison.CurrentCultureIgnoreCase))
                    {
                        var description = task.Description;
                        if (description.Contains("\n"))
                        {
                            description = description.Split('\n')[0];
                        }
                        var branch = description.Replace("Merge with", "").Replace(" ", "");
                        var release = task.Branch.Replace("release", "");

                        task.Branch = branch;
                        task.Description = $"Подготовка и публикация версии {task.Project} {release}.";
                        task.ReleaseVersion = release;
                    }
                    workTasks.Add(task);
                    _log?.Trace($" - Найден changeset: {changeset.Timestamp} - {changeset.Branch} - {changeset.AuthorEmailAddress} - {commitMessage}");
                }
            }
            if (!workTasks.Any())
            {
                return new List<TaskTimeItem>();
            }

            return workTasks;
        }

        private CommitType GetCommitType(string branch)
        {
            return branch.StartsWith("release") ? CommitType.Release : CommitType.Task;
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
