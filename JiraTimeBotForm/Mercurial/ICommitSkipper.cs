using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraTimeBotForm.Mercurial
{
    public interface ICommitSkipper
    {
        bool IsNeedToSkip(string branch, string commitMessage);
    }
}
