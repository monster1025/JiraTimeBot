namespace JiraTimeBotForm.TasksProcessors
{
    public interface ITasksProcessors
    {
        WorkLogTasksProcessor WorkLogTasksProcessor { get; }

        MeetingProcessor MeetingProcessor { get; }
    }
}
