using Aeroclub.CL.ASIA.Application.UI.Wpf.Commands;
using JiraTimeBot.Ui.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using JiraTimeBot.Core.Configuration;

namespace JiraTimeBot.Ui.ViewModels
{
    public class SettingsViewModel : ViewModelBase<SettingsModel>
    {
        private readonly ISettingsManager _settingsManager;
        private readonly IHavePassword _passwordAccessor;
        private readonly IApplicationNavigator _navigator;

        public SettingsViewModel(SettingsModel model, 
                                 ISettingsManager settingsManager,
                                 IHavePassword passwordAccessor,
                                 IApplicationNavigator navigator) : base(model)
        {
            _settingsManager = settingsManager;
            _passwordAccessor = passwordAccessor;
            _navigator = navigator;
            SaveSettingsCommand = new RelayCommand(SaveSettings);
            var workModes = Enum.GetValues(typeof(WorkType));
            WorkModes = new ObservableCollection<WorkType>(workModes.OfType<WorkType>());
        }

        public RelayCommand SaveSettingsCommand { get; set; }

        public ObservableCollection<WorkType> WorkModes { get; set; }

        public WorkType CurrentWorkType
        {
            get => Model.WorkType;
            set
            {
                Model.WorkType = value;
                OnPropertyChanged();
            } 
        }

        public string RepositoryPath
        {
            get => Model.RepositoryPath;
            set
            {
                Model.RepositoryPath = value;
                OnPropertyChanged();
            }
        }

        public string JiraLogin
        {
            get => Model.JiraUserName;
            set
            {
                Model.JiraUserName = value;
                OnPropertyChanged();
            }
        }

        private void SaveSettings()
        {
            if(!_settingsManager.SettingsIsValid(Model.Settings, out string msg))
            {
                return;
            }

            Model.JiraPassword = _passwordAccessor.GetPassword();
            _settingsManager.Save(Model.Settings);
            _navigator.NavigateToMain();
        }
    }
}