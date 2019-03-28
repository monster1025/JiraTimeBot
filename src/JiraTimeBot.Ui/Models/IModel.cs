using JiraTimeBot.Ui.Annotations;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace JiraTimeBot.Ui.Models
{
    public interface IModel
    {
        
    }

    public interface INotifyModel : IModel, INotifyPropertyChanged
    {

    }

    public abstract class NotifyModelBase : INotifyModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}