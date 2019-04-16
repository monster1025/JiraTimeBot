using System;
using System.Windows.Controls;

namespace JiraTimeBot.Ui
{
    public interface IHavePassword
    {
        string GetPassword();
        void SetPassword(string password);
    }
    
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : IHavePassword
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        public string GetPassword()
        {
            return PBox.Password;
        }

        public void SetPassword(string password)
        {
            PBox.Password = password;
        }
    }
}
