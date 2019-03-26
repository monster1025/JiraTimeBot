using Aeroclub.CL.ASIA.Application.UI.Wpf.Commands;
using System;

namespace JiraTimeBot.Ui.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly Func<IApplicationNavigator> _appNavigator;

        public MainViewModel(Func<IApplicationNavigator> appNavigator)
        {
            _appNavigator = appNavigator;

            NavigateToSettings = new RelayCommand(() =>
            {
                _appNavigator().NavigateToSettings();
            });
        }

        public RelayCommand NavigateToSettings { get; set; }
    }
}