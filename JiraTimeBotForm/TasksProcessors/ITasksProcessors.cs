using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraTimeBotForm.TasksProcessors
{
    public interface ITasksProcessors
    {
        WorkLogTasksProcessor WorkLogTasksProcessor { get; }

        MeetingProcessor MeetingProcessor { get; }
    }
}
