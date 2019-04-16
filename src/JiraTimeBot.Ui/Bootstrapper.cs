using Autofac;
using Autofac.Extras.AggregateService;
using JiraTimeBot.Core.Configuration;
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
            }).InstancePerLifetimeScope();

            builder.RegisterType<SettingsManager>().As<ISettingsManager>();
            builder.RegisterType<ApplicationModel>().InstancePerLifetimeScope();
            builder.RegisterType<SettingsModel>().InstancePerLifetimeScope();
            builder.RegisterType<MainModel>().InstancePerLifetimeScope();

            builder.RegisterType<ApplicationViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<MainPageViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<SettingsViewModel>()
                   .InstancePerLifetimeScope();
            
            builder.RegisterType<MainPage>().InstancePerLifetimeScope();
            builder.RegisterType<SettingsPage>()
                   .AsSelf()
                   .As<IHavePassword>()
                   .InstancePerLifetimeScope();
            
            builder.RegisterAggregateService<IRegisteredViewModels>();

            builder.RegisterType<ApplicationNavigator>()
                   .As<IApplicationNavigator>()
                   .InstancePerLifetimeScope();

            var container = builder.Build();

            return container;
        }
    }
}