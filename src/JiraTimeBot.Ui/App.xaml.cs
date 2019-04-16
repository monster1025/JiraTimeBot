using Autofac;
using JiraTimeBot.UI.Tray;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Navigation;
using JiraTimeBot.Core.Configuration;
using JiraTimeBot.Ui.ViewModels;


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
            var settingsManager = _container.Resolve<ISettingsManager>();
            var settings = settingsManager.Load();
            var scope = _container.BeginLifetimeScope("Started",
                                                      builder =>
                                                      {
                                                          if (settings != null)
                                                          {
                                                              builder.RegisterInstance(settings);
                                                          }
                                                          else
                                                          {
                                                              builder.RegisterInstance(new Settings());
                                                          }
                                                      });

            var appViewModel = scope.Resolve<ApplicationViewModel>();
            var mainWindow = scope.Resolve<MainWindow>();
            if (settings == null)
            {
                var settingsViewModel = scope.Resolve<SettingsViewModel>();
                appViewModel.CurrentViewModel = settingsViewModel;
            }
            else
            {
                var mainViewModel = scope.Resolve<MainPageViewModel>();
                appViewModel.CurrentViewModel = mainViewModel;
            }

            

            mainWindow.ShowActivated = true;
            mainWindow.Show();
            base.OnStartup(e);
        }

        /// <inheritdoc />
        protected override void OnLoadCompleted(NavigationEventArgs e)
        {
            base.OnLoadCompleted(e);
        }
    }
}
