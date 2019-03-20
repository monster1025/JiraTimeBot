using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using JiraTimeBot.Configuration;
using JiraTimeBot.Mercurial.Objects;
using JiraTimeBot.TaskTime.Objects;

namespace JiraTimeBot.TaskTime
{
    public class TaskTimeByCommitsCalculator: ITaskTimeCalculator
    {
        private readonly ILog _log;

        public TaskTimeByCommitsCalculator(ILog log)
        {
            _log = log;
        }

        public List<TaskTimeItem> CalculateTaskTime(List<MercurialCommitItem> commits, Settings settings, CancellationToken cancellationToken = default(CancellationToken))
        {
            int remainMinutes = settings.MinuterPerWorkDay;
            int totalCommitsCount = commits.Count;

            //если кол-во коммитов более чем кол-во интервалов - то уменьшим интервал вдвое.
            while (totalCommitsCount > (8 * (60.0 / settings.RountToMinutes)))
            {
                settings.RountToMinutes = (int)RoundTo((decimal)(settings.RountToMinutes / 2.0), 5);
                _log.Info($"Слишком много задач - уменьшаю интервал до {settings.RountToMinutes}.");
                if (settings.RountToMinutes == 5)
                {
                    break;
                }
            }

            List<TaskTimeItem> workTimeItems = new List<TaskTimeItem>();

            //Нам нужно раскидать 480 минут в день.
            foreach (var taskGroup in commits.GroupBy(f => f.Branch).OrderByDescending(f=>f.Count()))
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return new List<TaskTimeItem>();
                }

                int currentTaskCommits = taskGroup.Count();
                int currentTaskTime = (int)RoundTo(settings.MinuterPerWorkDay / totalCommitsCount * currentTaskCommits, settings.RountToMinutes);
                remainMinutes = remainMinutes - currentTaskTime;

                var orderedTasks = taskGroup.OrderBy(f => f.Time).ToArray();
                StringBuilder sb = new StringBuilder();
                foreach (var task in orderedTasks)
                {
                    sb.AppendLine($"- {task.Description}");
                }

                var taskTimeItem = new TaskTimeItem
                {
                    Branch = taskGroup.Key,
                    Time = TimeSpan.FromMinutes(currentTaskTime),
                    Commits = taskGroup.Count(),
                    Description = sb.ToString(),

                    StartTime = orderedTasks.First().Time,
                    EndTime = orderedTasks.Last().Time
                };

                workTimeItems.Add(taskTimeItem);
            }
            if (!workTimeItems.Any())
            {
                return new List<TaskTimeItem>();
            }

            if (remainMinutes != 0)
            {
                _log.Trace($"Погрешность распределения времени: {remainMinutes}. Добавляю к первой задаче.");
            }

            if (workTimeItems.First().Time.TotalMinutes > Math.Abs(remainMinutes) && remainMinutes < 0)
            {
                //если переборщили или не достаточно добавили до 8 часов - скореектируем остаток в первой задаче (она самая трудозатратная).
                workTimeItems.First().Time += TimeSpan.FromMinutes(remainMinutes);
            }

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
