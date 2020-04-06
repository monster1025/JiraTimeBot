namespace JiraTimeBot.Mercurial.Modifiers
{
    public interface ITechnicalInfoSkipper
    {
        string StripBranchPrefix(string branch, string commitMessage);
        string StripTechnicalInfo(string commitMessage);
    }
}
