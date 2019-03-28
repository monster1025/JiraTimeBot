using JiraTimeBot.Ui.Models;
using System;

namespace JiraTimeBot.Ui.ViewModels
{
    public class ApplicationViewModel : ViewModelBase<ApplicationModel>
    {
        private readonly Func<IApplicationNavigator> _appNavigator;
        
        private ViewModelBase _currentViewModel;

        public ApplicationViewModel(ApplicationModel model) : base(model)
        {
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
    }
}