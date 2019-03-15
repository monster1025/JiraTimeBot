namespace JiraTimeBotForm.Mercurial
{
    public interface ICommitSkipper
    {
        bool IsNeedToSkip(string branch, string commitMessage);
    }
}
