using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace JiraTimeBot.UI.Tray
{
    public static class SingleInstance
    {
        public static readonly int WM_SHOWFIRSTINSTANCE = RegisterWindowMessage($"WM_SHOWFIRSTINSTANCE|{AssemblyGuid}");
        
        public static void ShowFirstInstance()
        {
            const int HWND_BROADCAST = 0xffff;
            PostMessage((IntPtr)HWND_BROADCAST, WM_SHOWFIRSTINSTANCE, IntPtr.Zero, IntPtr.Zero);
        }
        
        public static string AssemblyGuid
        {
            get
            {
                object[] attributes = Assembly.GetEntryAssembly().GetCustomAttributes(typeof(System.Runtime.InteropServices.GuidAttribute), false);
                if (attributes.Length == 0) {
                    return string.Empty;
                }
                return ((GuidAttribute)attributes[0]).Value;
            }
        }
        
        [DllImport("user32")]
        public static extern int RegisterWindowMessage(string message);
	
        [DllImport("user32")]
        public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);
    }
}
