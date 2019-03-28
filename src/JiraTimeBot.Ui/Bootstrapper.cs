using Autofac;
using Autofac.Extras.AggregateService;
using JiraTimeBot.Ui.Models;
using JiraTimeBot.Ui.ViewModels;

namespace JiraTimeBot.Ui
{
    public class Bootstrapper
    {
        public IContainer Build()
        {
            var builder = new ContainerBuilder();

            builder.Register(c =>
            {
                var result = new MainWindow();
                result.DataContext = c.Resolve<ApplicationViewModel>();
                return result;
            }).SingleInstance();

            builder.RegisterType<ApplicationModel>().SingleInstance();
            builder.RegisterType<SettingsModel>().SingleInstance();
            builder.RegisterType<MainModel>().SingleInstance();

            builder.RegisterType<ApplicationViewModel>().SingleInstance();
            builder.RegisterType<MainPageViewModel>().SingleInstance();
            builder.RegisterType<SettingsViewModel>().SingleInstance();
            
            builder.RegisterType<MainPage>().SingleInstance();
            builder.RegisterType<SettingsPage>().SingleInstance();
            
            builder.RegisterAggregateService<IRegisteredViewModels>();

            builder.RegisterType<ApplicationNavigator>()
                   .As<IApplicationNavigator>()
                   .SingleInstance();

            var container = builder.Build();

            return container;
        }
    }
}