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
            int minutesPerWorkDay = settings.MinuterPerWorkDay + GetRandomMinutes(settings);
            int workHours = (settings.MinuterPerWorkDay / 60);
            int totalCommitsCount = commits.Count;

            if (!commits.Any())
            {
                return new List<TaskTimeItem>();
            }

            //если кол-во коммитов более чем кол-во интервалов - то уменьшим интервал вдвое.
            while (totalCommitsCount > (workHours * (60.0 / settings.RoundToMinutes)))
            {
                settings.RoundToMinutes = (int)RoundTo((decimal)(settings.RoundToMinutes / 2.0), 5);
                _log.Info($"Слишком много задач - уменьшаю интервал до {settings.RoundToMinutes}.");
                if (settings.RoundToMinutes == 5)
                {
                    break;
                }
            }

            List<TaskTimeItem> workTimeItems = new List<TaskTimeItem>();
            //Если указана задача контроля времени - то спишем туда 30 минут и вычеркнем их из общего рабочего времени.
            if (!string.IsNullOrEmpty(settings.TimeControlTask))
            {
                minutesPerWorkDay = minutesPerWorkDay - 30;
                workTimeItems.Add(new TaskTimeItem
                {
                    Time = TimeSpan.FromMinutes(30),
                    Branch = settings.TimeControlTask,
                    Commits = 1,
                    Description = "Ведение учета времени",
                    StartTime = DateTime.Now.Date
                });
            }

            int remainMinutes = minutesPerWorkDay;
            //Нам нужно раскидать 480 минут в день.
            foreach (var taskGroup in commits.GroupBy(f => f.Branch).OrderByDescending(f=>f.Count()))
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return new List<TaskTimeItem>();
                }

                int currentTaskCommits = taskGroup.Count();
                int currentTaskTime = (int)RoundTo(minutesPerWorkDay / totalCommitsCount * currentTaskCommits, settings.RoundToMinutes);
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
            workTimeItems = workTimeItems.OrderByDescending(f => f.Time).ToList();

            if (remainMinutes != 0)
            {
                _log.Trace($"Погрешность распределения времени: {remainMinutes}. Добавляю к первой задаче.");
            }

            if (workTimeItems.First().Time.TotalMinutes > Math.Abs(remainMinutes) && remainMinutes < 0)
            {
                //если переборщили или не достаточно добавили до 8 часов - скореектируем остаток в первой задаче (она самая трудозатратная).
                workTimeItems.First().Time += TimeSpan.FromMinutes(remainMinutes);
            }

            PrintTotal(workTimeItems);

            return workTimeItems;
        }

        private int GetRandomMinutes(Settings settings)
        {
            var rand = new Random();
            int randomIntervals = settings.RandomWorkMinutes / settings.RoundToMinutes;
            int randomMinutes = rand.Next(0, randomIntervals + 1) * settings.RoundToMinutes;
            return randomMinutes;
        }

        private void PrintTotal(List<TaskTimeItem> workTimeItems)
        {
            var totalTime = TimeSpan.Zero;
            foreach (var workTimeItem in workTimeItems)
            {
                totalTime += workTimeItem.Time;
            }

            _log.Trace($"Итоговое реально проставляемое время: {totalTime}");
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
