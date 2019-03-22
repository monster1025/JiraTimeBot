using JiraTimeBot.UI;
using System;
using System.Threading;
using System.Windows.Forms;
using JiraTimeBot.UI.Tray;

namespace JiraTimeBot
{
    static class Program
    {
        private static readonly Mutex Mutex = new Mutex(true, "{44426B3E-B901-4792-ACEA-1385D79DBAD1}");
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (Mutex.WaitOne(TimeSpan.Zero, true))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmMain());
                Mutex.ReleaseMutex();
            }
            else
            {
                SingleInstance.ShowFirstInstance();
            }
        }
    }
}
