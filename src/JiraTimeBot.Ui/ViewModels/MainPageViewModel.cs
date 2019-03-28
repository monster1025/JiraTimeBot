using Aeroclub.CL.ASIA.Application.UI.Wpf.Commands;
using JiraTimeBot.Ui.Models;
using System;

namespace JiraTimeBot.Ui.ViewModels
{
    public class MainPageViewModel : ViewModelBase<MainModel>
    {
        private readonly Func<IApplicationNavigator> _navigator;

        public MainPageViewModel(MainModel model, Func<IApplicationNavigator> navigator) : base(model)
        {
            _navigator = navigator;
            OpenSettingsCommand = new RelayCommand(NavigateToSettings);
        }

        public RelayCommand OpenSettingsCommand { get; set; }

        private void NavigateToSettings()
        {
            _navigator().NavigateToSettings();
        }
    }
}