using System.Windows.Forms;

namespace JiraTimeBot.UI.Tray
{
    public interface ITrayMenu
    {
        NotifyIcon Create(frmMain form);
        void Show();
        void Hide();
    }
}