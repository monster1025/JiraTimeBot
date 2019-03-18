namespace JiraTimeBotForm.Mercurial.Modifiers
{
    public interface ITechnicalInfoSkipper
    {
        string StripTechnicalInfo(string commitMessage);
    }
}
