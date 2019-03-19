using JiraTimeBot.Configuration;
using JiraTimeBot.TaskTime.Objects;

namespace JiraTimeBot.JiraIntegration.Comments
{
    public interface IJiraDescriptionSource
    {
        string GetDescription(TaskTimeItem item, bool addCommentsToWorklog, Settings settings);
    }
}
