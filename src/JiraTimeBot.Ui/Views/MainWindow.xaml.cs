using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;

namespace JiraTimeBot.Ui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            NotifyIcon trayIcon = new System.Windows.Forms.NotifyIcon();
            trayIcon.Icon = new Icon("app.ico");
            trayIcon.Visible = true;
            trayIcon.DoubleClick += (sender, args) =>
            {
                Show();
                ShowInTaskbar = true;
                WindowState = WindowState.Normal;
            };
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                ShowInTaskbar = false;
                Hide();
            }
            base.OnStateChanged(e);
        }
    }
}
