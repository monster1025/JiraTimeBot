using Aeroclub.CL.ASIA.Application.UI.Wpf.Commands;
using JiraTimeBot.Ui.Models;
using System;

namespace JiraTimeBot.Ui.ViewModels
{
    public class MainPageViewModel : ViewModelBase<MainModel>
    {
        private readonly IApplicationNavigator _navigator;
        private DateTime _selectedDate;

        public MainPageViewModel(MainModel model, IApplicationNavigator navigator) : base(model)
        {
            _navigator = navigator;
            SelectedDate = DateTime.Today;
            OpenSettingsCommand = new RelayCommand(NavigateToSettings);
            SetTimeCommand = new RelayCommand(SetTime);
            ShowMeetingInfoCommand = new RelayCommand(ShowMeeting);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void Cancel()
        {
        }

        private void ShowMeeting()
        {
        }

        private void SetTime()
        {
            
        }

        public RelayCommand OpenSettingsCommand { get; set; }

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand SetTimeCommand { get; set; }

        public RelayCommand ShowMeetingInfoCommand { get; set; }

        public RelayCommand CancelCommand { get; set; }

        private void NavigateToSettings()
        {
            _navigator.NavigateToSettings();
        }
    }
}