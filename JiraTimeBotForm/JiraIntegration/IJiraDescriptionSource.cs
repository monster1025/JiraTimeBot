using System.Text;
using System.Threading.Tasks;
using JiraTimeBotForm.TaskTime;

namespace JiraTimeBotForm.JiraIntegration
{
    public interface IJiraDescriptionSource
    {
        string GetDescription(TaskTimeItem item, bool addCommentsToWorklog);
    }
}
