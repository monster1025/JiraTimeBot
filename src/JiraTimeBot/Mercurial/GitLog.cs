using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using JiraTimeBot.Configuration;
using JiraTimeBot.Mercurial.Modifiers;
using JiraTimeBot.TaskTime.Objects;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;

namespace JiraTimeBot.Mercurial
{
    public class GitLog : IRepositoryLog
    {
        private readonly ILog _log;
        private readonly ICommitSkipper _commitSkipper;
        private readonly ITechnicalInfoSkipper _technicalInfoSkipper;

        public GitLog(ILog log, ICommitSkipper commitSkipper, ITechnicalInfoSkipper technicalInfoSkipper)
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
                if (!Directory.Exists(Path.Combine(repoDirectory, ".git")))
                {
                    continue;
                }

                var directoryInfo = new DirectoryInfo(repoDirectory);

                
                var repo = new LibGit2Sharp.Repository(repoDirectory);

                if (settings.PullBeforeProcess)
                {
                    try
                    {
                        // Credential information to fetch
                        LibGit2Sharp.PullOptions options = new LibGit2Sharp.PullOptions();
                        options.FetchOptions = new FetchOptions();

                        // User information to create a merge commit
                        var signature = new LibGit2Sharp.Signature(new Identity("MERGE_USER_NAME", "MERGE_USER_EMAIL"), DateTimeOffset.Now);

                        // Pull
                        Commands.Pull(repo, signature, options);
                    }
                    catch (Exception ex)
                    {
                        _log?.Trace($" - Не получается сделать pull для репозитория: {directoryInfo.Name} - {ex.Message}");
                    }
                }
                var filter = new CommitFilter { SortBy = CommitSortStrategies.Time };

                foreach (Commit commit in repo.Commits.QueryBy(filter).Take(200))
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return new List<TaskTimeItem>();
                    }
                    if (commit.Author.When.DateTime.Date != date || commit.Author.Email != settings.MercurialAuthorEmail)
                    {
                        continue;
                    }

                    var branch = ListBranchesContaininingCommit(repo, commit.Sha).Select(f => f.FriendlyName)
                        .Where(f => f.Contains("-") && !f.Contains("/")).Distinct().FirstOrDefault();
                    if (branch == null)
                    {
                        continue;
                    }

                    var commitMessage = commit.MessageShort;
                    if (_commitSkipper.IsNeedToSkip(branch, commitMessage))
                    {
                        continue;
                    }
                    commitMessage = _technicalInfoSkipper.StripTechnicalInfo(commitMessage);

                    var task = new TaskTimeItem(branch,
                        commitMessage,
                        directoryInfo.Name,
                        commit.Committer.When.DateTime,
                        TimeSpan.Zero,
                        1,
                        1, //changeset.PathActions.Count,
                        "",
                        GetCommitType(branch));
                    workTasks.Add(task);

                    _log?.Trace($" - Найден commit: {commit.Committer.When.DateTime} - {branch} - {commit.Author.Email} - {commitMessage}");
                }

                //foreach (var changeset in log)
                //{

                //    var commitMessage = FixEncoding(changeset.CommitMessage);
                //    if (_commitSkipper.IsNeedToSkip(changeset.Branch, commitMessage))
                //    {
                //        continue;
                //    }
                //    commitMessage = _technicalInfoSkipper.StripTechnicalInfo(commitMessage);

                //    var task = new TaskTimeItem(changeset.Branch,
                //        commitMessage,
                //        directoryInfo.Name,
                //        changeset.Timestamp,
                //        TimeSpan.Zero,
                //        1,
                //        changeset.PathActions.Count,
                //        "",
                //        GetCommitType(changeset.Branch));

                //    if (task.Type == CommitType.Release && task.Description.StartsWith("Merge with", StringComparison.CurrentCultureIgnoreCase))
                //    {
                //        var description = task.Description;
                //        if (description.Contains("\n"))
                //        {
                //            description = description.Split('\n')[0];
                //        }
                //        var branch = description.Replace("Merge with", "").Replace(" ", "");
                //        var release = task.Branch.Replace("release", "");

                //        task.Branch = branch;
                //        task.Description = $"Подготовка и публикация версии {task.Project} {release}.";
                //        task.ReleaseVersion = release;
                //    }
                //    workTasks.Add(task);
                //    _log?.Trace($" - Найден changeset: {changeset.Timestamp} - {changeset.Branch} - {changeset.AuthorEmailAddress} - {commitMessage}");
                //}
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


        private IEnumerable<Branch> ListBranchesContaininingCommit(Repository repo, string commitSha)
        {
            bool directBranchHasBeenFound = false;
            foreach (var branch in repo.Branches)
            {
                if (branch.Tip.Sha != commitSha)
                {
                    continue;
                }

                directBranchHasBeenFound = true;
                yield return branch;
            }

            if (directBranchHasBeenFound)
            {
                yield break;
            }

            foreach (var branch in repo.Branches)
            {
                var commits = repo.Commits.QueryBy(new CommitFilter { IncludeReachableFrom = branch }).Where(c => c.Sha == commitSha);

                if (!commits.Any())
                {
                    continue;
                }

                yield return branch;
            }
        }
    }
}