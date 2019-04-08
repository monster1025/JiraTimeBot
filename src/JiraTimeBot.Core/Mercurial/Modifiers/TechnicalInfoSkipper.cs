using System.Text;

namespace JiraTimeBot.Core.Mercurial.Modifiers
{
    public class TechnicalInfoSkipper : ITechnicalInfoSkipper
    {
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