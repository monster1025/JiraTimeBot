namespace JiraTimeBot.Ui
{
    public interface IRegisteredPages
    {
        SettingsPage Settings { get; set; }

        MainPage Main { get; set; }
    }
}