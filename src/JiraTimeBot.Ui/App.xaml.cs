using Autofac;
using JiraTimeBot.UI.Tray;
using System;
using System.Threading;
using System.Windows;

namespace JiraTimeBot.Ui
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IContainer _container;
        
        private static readonly Mutex Mutex = new Mutex(true, "{5B245110-FD79-4EC9-AF86-A98F29792B9D}");

        public App()
        {
            if (Mutex.WaitOne(TimeSpan.Zero, true))
            {
                Bootstrapper bootstrapper = new Bootstrapper();
                _container = bootstrapper.Build();
                Mutex.ReleaseMutex();
            }
            else
            {
                Mutex.Dispose();
                SingleInstance.ShowFirstInstance();
                this.Shutdown();
            }

        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = _container.Resolve<MainWindow>();
            var nav = _container.Resolve<IApplicationNavigator>();
            
            nav.NavigateToSettings();
            mainWindow.ShowActivated = true;
            mainWindow.Show();
            base.OnStartup(e);
        }
    }
}
