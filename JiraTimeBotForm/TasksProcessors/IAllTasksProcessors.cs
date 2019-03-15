namespace JiraTimeBotForm.TasksProcessors
{
    public interface IAllTasksProcessors
    {
        WorkLogTasksProcessor WorkLogTasksProcessor { get; }

        MeetingProcessor MeetingProcessor { get; }
    }
}
