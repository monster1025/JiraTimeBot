using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JiraTimeBot.TaskTime.Objects;

namespace JiraTimeBot.TaskTime
{
    public interface ITaskTimeSpread
    {
        List<TaskTimeItem> SpreadTime(List<TaskTimeItem> source,
                                      int minutes,
                                      int roundToMinutes,
                                      bool roundUp = false,
                                      bool appendTime = true);
    }
    public class TaskTimeSpread: ITaskTimeSpread
    {
        public List<TaskTimeItem> SpreadTime(List<TaskTimeItem> source, int minutes, int roundToMinutes, bool roundUp = false, bool appendTime = true)
        {
            List<TaskTimeItem> newList = new List<TaskTimeItem>();

            //Нам нужно раскидать 480 минут в день.
            foreach (var taskGroup in source.GroupBy(f => f.Branch).OrderByDescending(f => f.Count()))
            {
                int currentTaskCommits = taskGroup.Count();
                int currentTaskTime = (int)RoundTo(minutes / source.Count * currentTaskCommits, roundToMinutes, roundUp);

                var orderedTasks = taskGroup.OrderBy(f => f.StartTime).ToArray();
                StringBuilder sb = new StringBuilder();
                foreach (var taskDescription in orderedTasks.Select(f=>f.Description).Distinct())
                {
                    sb.AppendLine($"- {taskDescription}");
                }

                var timeSpent = appendTime
                    ? TimeSpan.FromMinutes(taskGroup.Sum(f => f.TimeSpent.TotalMinutes)) +
                      TimeSpan.FromMinutes(currentTaskTime)
                    : TimeSpan.FromMinutes(currentTaskTime);

                var taskTimeItem = new TaskTimeItem(
                    taskGroup.Key,
                    sb.ToString(),
                    taskGroup.First().Project,
                    taskGroup.Min(f => f.StartTime),
                    timeSpent,
                    taskGroup.Sum(f => f.Commits),
                    taskGroup.Sum(f => f.FilesAffected),
                    taskGroup.First().ReleaseVersion,
                    taskGroup.First().Type);

                newList.Add(taskTimeItem);
            }

            return newList;
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
