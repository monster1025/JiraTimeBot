using JiraTimeBot.Core.Configuration;
using JiraTimeBot.Core.TaskTime.Objects;

namespace JiraTimeBot.Core.JiraIntegration.Comments
{
    public interface IJiraDescriptionSource
    {
        string GetDescription(TaskTimeItem item, bool addCommentsToWorklog, Settings settings);
    }
}
