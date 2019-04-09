using System;

namespace JiraTimeBot.Core.Configuration
{
    public interface ISettingsManager
    {
        Settings Load();
        Settings LoadAndCheck(Action settingsSource, Action<string> errorMessageReporter);

        void Save(Settings settings);
    }
}