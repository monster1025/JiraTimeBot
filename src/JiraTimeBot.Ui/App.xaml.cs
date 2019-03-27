using Autofac;
using Autofac.Extras.AggregateService;
using JiraTimeBot.Ui.Models;
using JiraTimeBot.Ui.ViewModels;
using System.Windows;

namespace JiraTimeBot.Ui
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IContainer _container;

        public App()
        {
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var builder = new ContainerBuilder();

            builder.Register<MainWindow>(c =>
            {
                var result = new MainWindow();
                result.DataContext = c.Resolve<ApplicationViewModel>();
                return result;
            }).SingleInstance();

            builder.RegisterType<ApplicationModel>().SingleInstance();

            builder.RegisterType<ApplicationViewModel>().SingleInstance();
            builder.RegisterType<MainPageViewModel>().SingleInstance();
            builder.RegisterType<SettingsViewModel>().SingleInstance();
            
            builder.RegisterType<MainPage>().SingleInstance();
            builder.RegisterType<SettingsPage>().SingleInstance();
            
            builder.RegisterAggregateService<IRegisteredViewModels>();

            builder.RegisterType<ApplicationNavigator>()
                   .As<IApplicationNavigator>()
                   .SingleInstance();

            _container = builder.Build();

            var mainWindow = _container.Resolve<MainWindow>();
            var nav = _container.Resolve<IApplicationNavigator>();
            
            nav.NavigateToSettings();
            mainWindow.ShowActivated = true;
            mainWindow.Show();
            base.OnStartup(e);
        }
    }
}
