using System.Windows.Forms;

namespace JiraTimeBotForm.UI.Tray
{
    public interface ITrayMenu
    {
        NotifyIcon Create(frmMain form);
        void Show();
        void Hide();
    }
}