using JiraTimeBot.Configuration;
using JiraTimeBot.TaskTime.Objects;

namespace JiraTimeBot.JiraIntegration.Comments
{
    public class JiraDescriptionSource : IJiraDescriptionSource
    {
        public string GetDescription(TaskTimeItem taskTimeItem, bool addCommentsToWorklog, Settings settings)
        {
            return addCommentsToWorklog ? taskTimeItem.Description : "";
        }
    }
}