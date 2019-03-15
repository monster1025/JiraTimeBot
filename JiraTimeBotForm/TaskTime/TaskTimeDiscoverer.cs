using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using JiraTimeBotForm.CommitWorks;
using JiraTimeBotForm.Configuration;
using Mercurial;

namespace JiraTimeBotForm.TaskTime
{
    public class TaskTimeDiscoverer: ITaskTimeDiscoverer
    {
        private readonly BuzzwordReplacer _buzzwordReplacer;
        private readonly ILog _log;

        public TaskTimeDiscoverer(BuzzwordReplacer buzzwordReplacer, ILog log = null)
        {
            _buzzwordReplacer = buzzwordReplacer;
            _log = log;
        }

        public List<TaskTimeItem> GetTaskTimes(Settings settings, DateTime? date = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var workTimeItems = new List<TaskTimeItem>();

            date = date.GetValueOrDefault(DateTime.Now.Date);

            var workTasks = new List<TaskTimeItem>();
            foreach (var repoDirectory in Directory.GetDirectories(settings.RepositoryPath))
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return workTimeItems;
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
                        return workTimeItems;
                    }

                    var commitMessage = FixEncoding(changeset.CommitMessage);
                    if (IsNeedToSkip(changeset.Branch, commitMessage))
                    {
                        continue;
                    }
                    commitMessage = StripTechnicalInfo(commitMessage);
                    commitMessage = _buzzwordReplacer.FixBuzzwords(commitMessage);

                    workTasks.Add(new TaskTimeItem
                    {
                        Description = commitMessage,
                        Branch = changeset.Branch,
                        Commits = 1,
                    });
                    _log?.Trace($" - Найден changeset: {changeset.Timestamp} - {changeset.Branch} - {changeset.AuthorEmailAddress} - {commitMessage}");
                }
            }
            if (!workTasks.Any())
            {
                return new List<TaskTimeItem>();
            }

            int remainMinutes = settings.MinuterPerWorkDay;
            int totalCommitsCount = workTasks.Count;

            //Нам нужно раскидать 480 минут в день.
            foreach (var taskGroup in workTasks.GroupBy(f => f.Branch).OrderByDescending(f=>f.Count()))
            {
                int currentTaskCommits = taskGroup.Count();
                int currentTaskTime = (int)RoundTo(settings.MinuterPerWorkDay / totalCommitsCount * currentTaskCommits);
                remainMinutes = remainMinutes - currentTaskTime;

                StringBuilder sb = new StringBuilder();
                foreach (var task in taskGroup)
                {
                    sb.AppendLine($"- {task.Description}");
                }

                var taskTimeItem = new TaskTimeItem
                {
                    Branch = taskGroup.Key,
                    Time = TimeSpan.FromMinutes(currentTaskTime),
                    Commits = taskGroup.Count(),
                    Description = sb.ToString()
                };

                workTimeItems.Add(taskTimeItem);
            }
            //если переборщили или не достаточно добавили до 8 часов - скореектируем остаток в первой задаче (она самая трудозатратная).
            workTimeItems.First().Time += TimeSpan.FromMinutes(remainMinutes);

            return workTimeItems;
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

        private decimal RoundTo(decimal value, decimal to = 15, bool up = true)
        {
            if ((value % to) == 0)
                return value;
            if (up)
                return (value - (value % to) + to);

            return (value - (value % to));
        }
    }
}
