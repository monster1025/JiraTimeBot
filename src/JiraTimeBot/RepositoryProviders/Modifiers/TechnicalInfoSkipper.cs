using System;
using System.Text;

namespace JiraTimeBot.RepositoryProviders.Modifiers
{
    public class TechnicalInfoSkipper : ITechnicalInfoSkipper
    {
        public string StripBranchPrefix(string branch, string commitMessage)
        {
            if (commitMessage.StartsWith(branch, StringComparison.InvariantCultureIgnoreCase))
            {
                return commitMessage.TrimStart(branch.ToCharArray()).Trim();
            }

            var quotedBranch = $"[{branch}]";
            if (commitMessage.StartsWith(quotedBranch, StringComparison.InvariantCultureIgnoreCase))
            {
                return commitMessage.TrimStart(quotedBranch.ToCharArray()).Trim();
            }

            return commitMessage;
        }

        public string StripTechnicalInfo(string commitMessage)
        {
            if (!commitMessage.Contains("Signed-by:"))
            {
                return commitMessage;
            }

            var arr = commitMessage.Split('\n');
            var sb = new StringBuilder();
            foreach (var itm in arr)
            {
                if (string.IsNullOrEmpty(itm))
                {
                    continue;
                }
                if (itm.StartsWith("Signed-by"))
                {
                    continue;
                }
                if (itm.StartsWith("Jira:"))
                {
                    continue;
                }

                sb.AppendLine(itm);
            }

            var message = sb.ToString();
            message = message.TrimEnd('\n');
            message = message.TrimEnd('\r');

            return message;
        }

    }
}