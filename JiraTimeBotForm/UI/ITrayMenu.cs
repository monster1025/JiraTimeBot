using System.Windows.Forms;

namespace JiraTimeBotForm.UI
{
    public interface ITrayMenu
    {
        NotifyIcon Create(frmMain form);
        void Show();
        void Hide();
    }
}