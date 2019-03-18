using JiraTimeBotForm.TaskTime.Objects;

namespace JiraTimeBotForm.JiraIntegration.Comments
{
    public interface IJiraDescriptionSource
    {
        string GetDescription(TaskTimeItem item, bool addCommentsToWorklog);
    }
}
