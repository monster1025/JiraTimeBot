using System.Windows.Forms;
using Microsoft.Win32;

namespace JiraTimeBot.UI.Startup
{
    public class AutoStartUp
    {
        readonly RegistryKey _rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

        public bool Get()
        {
            return _rkApp.GetValue(Application.ProductName) != null;
        }

        public void Set(bool autoStart)
        {
            if (autoStart)
            {
                _rkApp.SetValue(Application.ProductName, Application.ExecutablePath);
            }
            else
            {
                _rkApp.DeleteValue(Application.ProductName, false);
            }
        }
    }
}
