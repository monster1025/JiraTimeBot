using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using JiraTimeBot.Configuration;

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

        private List<TaskTimeItem> SpreadTime(List<TaskTimeItem> source, int minutes, int roundToMinutes, bool roundUp = false, bool appendTime = true)
        {
            if (source == null || !source.Any())
            {
                return new List<TaskTimeItem>();
            }

            List<TaskTimeItem> newList = new List<TaskTimeItem>();
            int remainMinutes = minutes;

            //Нам нужно раскидать 480 минут в день.
            foreach (var taskGroup in source.GroupBy(f => f.Branch).OrderByDescending(f => f.Count()))
            {
                int currentTaskCommits = taskGroup.Count();
                int currentTaskTime = (int)RoundTo(minutes / source.Count * currentTaskCommits, roundToMinutes, roundUp);
                remainMinutes = remainMinutes - currentTaskTime;

                var orderedTasks = taskGroup.OrderBy(f => f.StartTime).ToArray();
                StringBuilder sb = new StringBuilder();
                foreach (var task in orderedTasks)
                {
                    sb.AppendLine($"- {task.Description}");
                }
                var taskTimeItem = new TaskTimeItem
                {
                    Branch = taskGroup.Key,
                    TimeSpent = appendTime ?
                        TimeSpan.FromMinutes(taskGroup.Sum(f=>f.TimeSpent.TotalMinutes)) + TimeSpan.FromMinutes(currentTaskTime):
                        TimeSpan.FromMinutes(currentTaskTime)
                    ,
                    Commits = taskGroup.Sum(f=>f.Commits),
                    Description = sb.ToString(),
                    StartTime = taskGroup.Min(f=>f.StartTime),
                    FilesAffected = taskGroup.Sum(f=>f.FilesAffected)
                };

                newList.Add(taskTimeItem);
            }
            newList = newList.OrderByDescending(f => f.TimeSpent).ToList();

            return newList;
        }

        public List<TaskTimeItem> CalculateTaskTime(List<TaskTimeItem> commits, Settings settings, CancellationToken cancellationToken = default(CancellationToken))
        {
            int minutesPerWorkDay = settings.MinuterPerWorkDay + GetRandomMinutes(settings);
            int workHours = (settings.MinuterPerWorkDay / 60);
            int totalCommitsCount = commits.Count;

            if (cancellationToken.IsCancellationRequested)
            {
                return new List<TaskTimeItem>();
            }
            if (!commits.Any())
            {
                return new List<TaskTimeItem>();
            }

            FixTooBigInterval(settings, workHours, totalCommitsCount);

            int remainMinutes = minutesPerWorkDay;

            var workTimeItems = new List<TaskTimeItem>();
            var timeControlTask = GetTimeControlTask(settings.TimeControlTask);
            if (timeControlTask != null)
            {
                workTimeItems.Add(timeControlTask);
                remainMinutes -= (int)timeControlTask.TimeSpent.TotalMinutes;
            }

            var workTasks = SpreadTime(commits, remainMinutes, settings.RoundToMinutes);
            workTimeItems.AddRange(workTasks);

            remainMinutes = minutesPerWorkDay - (int) workTimeItems.Sum(f => f.TimeSpent.TotalMinutes);
            if (remainMinutes != 0)
            {
                _log.Trace($"Погрешность распределения времени: {remainMinutes}. Добавляю к первой задаче.");
            }
            if ((workTimeItems.First().TimeSpent.TotalMinutes > Math.Abs(remainMinutes) && remainMinutes < 0) || remainMinutes > 0)
            {
                //если переборщили или не достаточно добавили до 8 часов - скореектируем остаток в первой задаче (она самая трудозатратная).
                workTimeItems.First().TimeSpent += TimeSpan.FromMinutes(remainMinutes);
            }

            PrintTotal(workTimeItems);

            return workTimeItems.OrderByDescending(f=>f.TimeSpent).ToList();
        }

        private TaskTimeItem GetTimeControlTask(string timeControlTask, int minutes = 30)
        {
            //Если указана задача контроля времени - то спишем туда 30 минут и вычеркнем их из общего рабочего времени.
            if (!string.IsNullOrEmpty(timeControlTask))
            {
                return new TaskTimeItem
                {
                    TimeSpent = TimeSpan.FromMinutes(minutes),
                    Branch = timeControlTask,
                    Commits = 1,
                    Description = "Ведение учета времени"
                };
            }

            return null;
        }

        private void FixTooBigInterval(Settings settings, int totalCommitsCount, int workHours)
        {
            //если кол-во коммитов более чем кол-во интервалов - то уменьшим интервал вдвое.
            while (totalCommitsCount > (workHours * (60.0 / settings.RoundToMinutes)))
            {
                settings.RoundToMinutes = (int) RoundTo((decimal) (settings.RoundToMinutes / 2.0), 5);
                _log.Info($"Слишком много задач - уменьшаю интервал до {settings.RoundToMinutes}.");
                if (settings.RoundToMinutes == 5)
                {
                    break;
                }
            }
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
                totalTime += workTimeItem.TimeSpent;
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
