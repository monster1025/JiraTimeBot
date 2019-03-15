using System;
using System.Windows.Forms;

namespace JiraTimeBotForm.UI
{
    public class TrayMenu: ITrayMenu
    {
        private NotifyIcon  _trayIcon;

        public NotifyIcon Create(frmMain form)
        {
            var trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Exit", OnExit);

            _trayIcon = new NotifyIcon
            {
                Text = "JiraTimeBot", 
                Icon = form.Icon, 
                ContextMenu = trayMenu,
                Visible = false,
                Tag = form
            };
            _trayIcon.Click += btnTray_Click;
            _trayIcon.DoubleClick += btnTray_Click;

            return _trayIcon;
        }

        public void Show()
        {
            _trayIcon.Visible = true;
        }

        public void Hide()
        {
            _trayIcon.Visible = false;
        }

        private void OnExit(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnTray_Click(object sender, EventArgs e)
        {
            var icon = (NotifyIcon) sender;
            var form = (frmMain) icon.Tag;
            form.Visible = true;
            form.Activate();
            form.BringToFront();
            Extensions.Extensions.Restore(form);
        }
    }

    public interface ITrayMenu
    {
        NotifyIcon Create(frmMain form);
        void Show();
        void Hide();
    }
}
