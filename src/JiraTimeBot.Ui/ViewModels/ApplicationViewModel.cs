using Aeroclub.CL.ASIA.Application.UI.Wpf.Commands;
using System;

namespace JiraTimeBot.Ui.ViewModels
{
    public class ApplicationViewModel : ViewModelBase
    {
        private readonly Func<IApplicationNavigator> _appNavigator;
        private ViewModelBase _currentViewModel;

        public ApplicationViewModel(Func<IApplicationNavigator> appNavigator)
        {
            _appNavigator = appNavigator;

            NavigateToSettings = new RelayCommand(() =>
            {
                _appNavigator().NavigateToSettings();
            });

            NavigateToMain = new RelayCommand(() =>
            {
                _appNavigator().NavigateToMain();
            });
        }

        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand NavigateToSettings { get; set; }

        public RelayCommand NavigateToMain { get; set; }
    }
}