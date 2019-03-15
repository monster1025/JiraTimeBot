using JiraTimeBotForm.TaskTime;

namespace JiraTimeBotForm.JiraIntegration
{
    public interface IJiraDescriptionSource
    {
        string GetDescription(TaskTimeItem item, bool addCommentsToWorklog);
    }
}
