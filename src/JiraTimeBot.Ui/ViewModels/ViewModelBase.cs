using JiraTimeBot.Ui.Annotations;
using JiraTimeBot.Ui.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace JiraTimeBot.Ui.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public abstract class ViewModelBase<T> : ViewModelBase where T : IModel
    {
        // ReSharper disable once MemberCanBePrivate.Global
        protected T Model { get; }

        protected ViewModelBase(T model)
        {
            Model = model;
        }
    }
}