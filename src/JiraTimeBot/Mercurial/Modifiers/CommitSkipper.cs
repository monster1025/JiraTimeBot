using System;

namespace JiraTimeBot.Mercurial.Modifiers
{
    public class CommitSkipper : ICommitSkipper
    {
        public bool IsNeedToSkip(string branch, string commitMessage)
        {
            if (branch.StartsWith("release") && commitMessage.StartsWith("Merge with", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }
            
            if (!branch.Contains("-"))
            {
                return true;
            }

            //Пропускаем Close и Merge коммиты
            if (commitMessage.StartsWith($"Close {branch}", StringComparison.InvariantCultureIgnoreCase) ||
                commitMessage.StartsWith($"Merge with", StringComparison.InvariantCultureIgnoreCase) ||
                commitMessage.StartsWith($"Merge branch", StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }

            return false;
        }
    }
}