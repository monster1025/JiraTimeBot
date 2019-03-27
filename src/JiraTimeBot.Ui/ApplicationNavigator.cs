using JiraTimeBot.Ui.ViewModels;

namespace JiraTimeBot.Ui
{
    public interface IRegisteredViewModels
    {
        SettingsViewModel Settings { get; set; }
        MainPageViewModel MainPage { get; set; }
    }

    public class ApplicationNavigator : IApplicationNavigator
    {
        private readonly ApplicationViewModel _applicationViewModel;
        private readonly IRegisteredViewModels _viewModels;

        public ApplicationNavigator(ApplicationViewModel applicationViewModel, 
                                    IRegisteredViewModels viewModels)
        {
            _applicationViewModel = applicationViewModel;
            _viewModels = viewModels;
        }

        public void NavigateToMain()
        {
            _applicationViewModel.CurrentViewModel = _viewModels.MainPage;
        }

        public void NavigateToSettings()
        {
            _applicationViewModel.CurrentViewModel = _viewModels.Settings;
        }
    }
}