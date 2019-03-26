using JiraTimeBot.Ui.ViewModels;

namespace JiraTimeBot.Ui
{
    public interface IRegisteredViewModels
    {
        MainViewModel Main { get; set; }
        SettingsViewModel Settings { get; set; }
    }

    public class ApplicationNavigator : IApplicationNavigator
    {
        private readonly MainWindow _mainWindow;
        private readonly IRegisteredPages _pages;
        private readonly IRegisteredViewModels _viewModels;

        public ApplicationNavigator(MainWindow mainWindow, 
                                    IRegisteredPages pages, 
                                    IRegisteredViewModels viewModels)
        {
            _mainWindow = mainWindow;
            _pages = pages;
            _viewModels = viewModels;
        }

        public void NavigateToMain()
        {
            _pages.Main.DataContext = _viewModels.Main;
            _mainWindow.Navigate(_pages.Main);
        }

        public void NavigateToSettings()
        {
            _pages.Settings.DataContext = _viewModels.Settings;
            _mainWindow.Navigate(_pages.Settings);
        }
    }
}