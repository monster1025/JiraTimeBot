using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using JiraTimeBotForm.Configuration;
using Mercurial;

namespace JiraTimeBotForm.TaskTime
{
    public class TaskTimeDiscoverer: ITaskTimeDiscoverer
    {
        private readonly ILog _log;

        public TaskTimeDiscoverer(ILog log = null)
        {
            _log = log;
        }

        public List<TaskTimeItem> GetTaskTimes(Settings settings, DateTime? date = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var workTimeItems = new List<TaskTimeItem>();

            date = date.GetValueOrDefault(DateTime.Now.Date);

            var workTasks = new List<string>();

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
                    Users = { settings.MercurialAuthorEmail }
                };

                var log = repo.Log(logCommand);

                foreach (var changeset in log)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        return workTimeItems;
                    }

                    if (!changeset.Branch.Contains("-"))
                    {
                        continue;
                    }
                    //Пропускаем Close коммиты
                    if (changeset.CommitMessage.StartsWith($"Close {changeset.Branch} ", StringComparison.InvariantCultureIgnoreCase))
                    {
                        continue;
                    }

                    workTasks.Add(changeset.Branch);
                    _log?.Trace($" - Найден changeset: {changeset.Timestamp} - {changeset.Branch} - {changeset.AuthorEmailAddress}");
                }
            }
            if (!workTasks.Any())
            {
                return new List<TaskTimeItem>();
            }

            int remainMinutes = settings.MinuterPerWorkDay;
            int totalCommitsCount = workTasks.Count;

            //Нам нужно раскидать 480 минут в день.
            foreach (var taskGroup in workTasks.GroupBy(f => f).OrderByDescending(f=>f.Count()))
            {
                int currentTaskCommits = taskGroup.Count();
                int currentTaskTime = (int)RoundTo(settings.MinuterPerWorkDay / totalCommitsCount * currentTaskCommits);
                remainMinutes = remainMinutes - currentTaskTime;

                var taskTimeItem = new TaskTimeItem
                {
                    Branch = taskGroup.Key,
                    Time = TimeSpan.FromMinutes(currentTaskTime),
                    Commits = taskGroup.Count(),
                };

                workTimeItems.Add(taskTimeItem);
            }
            //если переборщили или не достаточно добавили до 8 часов - скореектируем остаток в первой задаче (она самая трудозатратная).
            workTimeItems.First().Time += TimeSpan.FromMinutes(remainMinutes);

            return workTimeItems;
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
