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
            builder.Register(c => new MainWindow())
                   .SingleInstance();

            builder.Register(c => new ApplicationModel()).SingleInstance();
            builder.Register(c => new SettingsViewModel()).SingleInstance();

            builder.RegisterType<SettingsPage>().SingleInstance();
            builder.RegisterType<MainPage>().SingleInstance();

            builder.RegisterType<MainViewModel>().SingleInstance();
            builder.RegisterType<SettingsViewModel>().SingleInstance();


            builder.RegisterAggregateService<IRegisteredPages>();
            builder.RegisterAggregateService<IRegisteredViewModels>();

            builder.RegisterType<ApplicationNavigator>()
                   .As<IApplicationNavigator>()
                   .SingleInstance();

            _container = builder.Build();

            var mainWindow = _container.Resolve<MainWindow>();

            var nav = _container.Resolve<IApplicationNavigator>();
            nav.NavigateToMain();

            //var mainPage = _container.Resolve<MainPage>();
            //mainWindow.Navigate(mainPage);
            mainWindow.ShowDialog();
            base.OnStartup(e);
        }
    }
}
