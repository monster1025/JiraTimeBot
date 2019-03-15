using System;

namespace JiraTimeBotForm.Mercurial.Modifiers
{
    public class CommitSkipper : ICommitSkipper
    {
        public bool IsNeedToSkip(string branch, string commitMessage)
        {
            if (!branch.Contains("-"))
            {
                return true;
            }

            //Пропускаем Close и Merge коммиты
            if (commitMessage.StartsWith($"Close {branch} ", StringComparison.InvariantCultureIgnoreCase) ||
                commitMessage.StartsWith($"Merge with ", StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }

            return false;
        }
    }
}