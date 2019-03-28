using Aeroclub.CL.ASIA.Application.UI.Wpf.Commands;
using JiraTimeBot.Ui.Models;
using System;

namespace JiraTimeBot.Ui.ViewModels
{
    public class SettingsViewModel : ViewModelBase<SettingsModel>
    {
        private readonly Func<IApplicationNavigator> _navigator;

        public SettingsViewModel(SettingsModel model, Func<IApplicationNavigator> navigator) : base(model)
        {
            _navigator = navigator;
            SaveSettingsCommand = new RelayCommand(SaveSettings);
        }

        public RelayCommand SaveSettingsCommand { get; set; }

        private void SaveSettings()
        {
            //TODO: SaveSettingsHere
            _navigator().NavigateToMain();
        }
    }
}