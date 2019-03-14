using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
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

        public List<TaskTimeItem> GetTaskTimes(Settings settings, DateTime? date = null)
        {
            if (date == null)
            {
                date = DateTime.Now.Date;
            }
            date = date.Value.Date;

            List<string> workTasks = new List<string>();
            foreach (var repoDirectory in Directory.GetDirectories(settings.RepositoryPath))
            {
                //var project = new DirectoryInfo(repoDirectory).Name;
                if (!Directory.Exists(Path.Combine(repoDirectory, ".hg")))
                {
                    continue;
                }
                Application.DoEvents();

                var repo = new Repository(repoDirectory);
                var log = repo.Log(new LogCommand {Date = date, Users = { settings.MercurialAuthorEmail } });
                foreach (var changeset in log)
                {
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
                    Application.DoEvents();
                }
            }
            if (!workTasks.Any())
            {
                return new List<TaskTimeItem>();
            }

            int remainMinutes = settings.MinuterPerWorkDay;

            //Нам нужно раскидать 480 минут в день.
            var workTimeItems = new List<TaskTimeItem>();
            foreach (var taskGroup in workTasks.GroupBy(f=>f))
            {
                int currentTaskCommits = taskGroup.Count();
                int currentTaskTime = (int)RoundTo15(settings.MinuterPerWorkDay / workTasks.Count * currentTaskCommits);
                remainMinutes = remainMinutes - currentTaskTime;

                workTimeItems.Add(new TaskTimeItem
                {
                    Branch = taskGroup.Key,
                    Time = TimeSpan.FromMinutes(currentTaskTime),
                    Commits = taskGroup.Count(),
                });
            }
            workTimeItems.First().Time += TimeSpan.FromMinutes(remainMinutes);

            return workTimeItems;
        }

        private decimal RoundTo15(decimal value, bool up = true)
        {
            if ((value % 15) == 0)
                return value;
            if (up)
                return (value - (value % 15) + 15);

            return (value - (value % 15));
        }

    }
}
