using JiraTimeBot.Configuration;
using JiraTimeBot.RepositoryProviders.Interfaces;
using JiraTimeBot.RepositoryProviders.Modifiers;
using JiraTimeBot.TaskTime.Objects;
using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace JiraTimeBot.RepositoryProviders
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

        public List<TaskTimeItem> GetRepositoryLog(Settings settings, string currentRepository, DateTime? date = null, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(currentRepository) || !Directory.Exists(currentRepository))
            {
                _log.Error("Папка с репо не сушествует.");
                return new List<TaskTimeItem>();
            }

            date = date.GetValueOrDefault(DateTime.Now.Date);

            var workTasks = new List<TaskTimeItem>();
            foreach (var repoDirectory in Directory.GetDirectories(currentRepository, ".git", SearchOption.AllDirectories))
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return new List<TaskTimeItem>();
                }

                //var project = new DirectoryInfo(repoDirectory).Name;
                var repoRootDirectory = repoDirectory.Replace(".git", "");
                var directoryInfo = new DirectoryInfo(repoRootDirectory);
                if (!directoryInfo.Exists)
                {
                    continue;
                }


                var repo = new LibGit2Sharp.Repository(repoRootDirectory);
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

                var allCommits = new List<Commit>();

                var reflog = repo.Refs.Log(repo.Refs.Head);
                foreach (var reflogItem in reflog)
                {

                    var dateMatch = date.Value.Date == reflogItem.Committer.When.Date;
                    var committerEmail = reflogItem.Committer.Email;

                    var userMatch = committerEmail.Equals(settings.MercurialAuthorEmail, StringComparison.CurrentCultureIgnoreCase)
                                    || committerEmail.Equals(settings.AlternativeEmail, StringComparison.CurrentCultureIgnoreCase);

                    if (!dateMatch || !userMatch)
                    {
                        continue;
                    }

                    var commit = repo.Lookup<LibGit2Sharp.Commit>(reflogItem.To.Sha);
                    if (commit != null && allCommits.All(f => f.Sha != commit.Sha))
                    {
                        allCommits.Add(commit);
                    }
                }

                foreach (Commit commit in allCommits)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return new List<TaskTimeItem>();
                    }

                    var userMatch = commit.Author.Email.Equals(settings.MercurialAuthorEmail, StringComparison.CurrentCultureIgnoreCase)
                                    || commit.Author.Email.Equals(settings.AlternativeEmail, StringComparison.CurrentCultureIgnoreCase);


                    if (commit.Author.When.DateTime.Date != date.Value.Date 
                        || !userMatch)
                    {
                        continue;
                    }

                    var regex = new Regex("[a-zA-Z0-9]{1,4}-[0-9]{1,6}");
                    var branch = ListBranchesContainsCommit(repo, commit.Sha)
                                 .Select(f => f.FriendlyName)
                                 .FirstOrDefault(f => regex.IsMatch(f));
                                 

                        //.Where(f => f.Contains("-") && !f.Contains("/")).Distinct().FirstOrDefault();
                    if (branch == null)
                    {
                        continue;
                    }

                    branch = regex.Match(branch).Value;

                    var commitMessage = commit.MessageShort;
                    commitMessage = _technicalInfoSkipper.StripBranchPrefix(branch, commitMessage);

                    if (_commitSkipper.IsNeedToSkip(branch, commitMessage))
                    {
                        continue;
                    }

                    commitMessage = _technicalInfoSkipper.StripTechnicalInfo(commitMessage);
                    var files = FilesToMerge(commit, repo);

                    var task = new TaskTimeItem(branch,
                        commitMessage,
                        directoryInfo.Name,
                        commit.Committer.When.DateTime,
                        TimeSpan.Zero,
                        1,
                        files.Length, //changeset.PathActions.Count,
                        "",
                        GetCommitType(branch));
                    workTasks.Add(task);

                    _log?.Trace($" - Найден commit: {commit.Committer.When.DateTime} - {branch} - {commit.Author.Email} - {commitMessage}");
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

        public String[] FilesToMerge(Commit commit, Repository repo)
        {
            var fileList = new List<String>();
            foreach (var parent in commit.Parents)
            {
                foreach (TreeEntryChanges change in repo.Diff.Compare<TreeChanges>(parent.Tree, commit.Tree))
                {
                    fileList.Add(change.Path);
                }
            }
            return fileList.ToArray();
        }

        private IEnumerable<Branch> ListBranchesContainsCommit(Repository repo, string commitSha)
        {
            bool directBranchHasBeenFound = false;
            foreach (var branch in repo.Branches)
            {
                if (branch.Tip?.Sha != commitSha)
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