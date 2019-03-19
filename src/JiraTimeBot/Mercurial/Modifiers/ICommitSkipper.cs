namespace JiraTimeBot.Mercurial.Modifiers
{
    public interface ICommitSkipper
    {
        bool IsNeedToSkip(string branch, string commitMessage);
    }
}
