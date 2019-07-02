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
        private readonly ITaskTimeSpread _spreadHelper;

        public TaskTimeByCommitsCalculator(ILog log, ITaskTimeSpread spreadHelper)
        {
            _log = log;
            _spreadHelper = spreadHelper;
        }

        public List<TaskTimeItem> CalculateTaskTime(List<TaskTimeItem> commits, Settings settings, CancellationToken cancellationToken = default(CancellationToken))
        {
            int minutesPerWorkDay = settings.MinuterPerWorkDay + GetRandomMinutes(settings);
            int workHours = (settings.MinuterPerWorkDay / 60);
            int totalCommitsCount = commits.Count;
            var taskCommits = commits.Where(f => f.Type == CommitType.Task).ToList();
            var releaseCommits = commits.Where(f => f.Type == CommitType.Release).ToList();

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

            if (releaseCommits.Any())
            {
                var releaseTasks = _spreadHelper.SpreadTime(releaseCommits, 30, 1);
                workTimeItems.AddRange(releaseTasks);
                remainMinutes -= releaseTasks.Sum(f=>(int)f.TimeSpent.TotalMinutes);
            }

            var workTasks = _spreadHelper.SpreadTime(taskCommits, remainMinutes, settings.RoundToMinutes);
            foreach (var workTask in workTasks)
            {
                var existingItem = workTimeItems.FirstOrDefault(f => f.Branch == workTask.Branch);
                if (existingItem != null)
                {
                    existingItem.TimeSpent += workTask.TimeSpent;
                    existingItem.Description += "\r\n" + workTask.Description;
                    continue;
                }

                workTimeItems.Add(workTask);
            }
            workTimeItems = workTimeItems.OrderByDescending(f => f.TimeSpent).ToList();

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
                return new TaskTimeItem(timeControlTask,
                    "Ведение учета времени",
                    1,
                    TimeSpan.FromMinutes(minutes),
                    DateTime.Now,
                    1,
                    CommitType.Task);
            }

            return null;
        }

        private void FixTooBigInterval(Settings settings, int workHours, int totalCommitsCount)
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
