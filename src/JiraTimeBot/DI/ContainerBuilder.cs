using Autofac;
using Autofac.Extras.AggregateService;
using JiraTimeBot.JiraIntegration;
using JiraTimeBot.JiraIntegration.Comments;
using JiraTimeBot.RepositoryProviders;
using JiraTimeBot.RepositoryProviders.Interfaces;
using JiraTimeBot.RepositoryProviders.Modifiers;
using JiraTimeBot.TasksProcessors;
using JiraTimeBot.TaskTime;
using JiraTimeBot.UI;
using JiraTimeBot.UI.Startup;
using JiraTimeBot.UI.Tray;
using JiraTimeBot.Update;
using System;
using System.Windows.Forms;

namespace JiraTimeBot.DI
{
    public class Bootstrapper
    {
        private readonly ContainerBuilder _builder;
        public Bootstrapper()
        {
            _builder = new ContainerBuilder();
        }

        public IContainer Build(TextBox logTextBox)
        {
            _builder.Register(c => logTextBox).AsSelf();

            _builder.Register(f =>
            {
                var txt = f.Resolve<TextBox>();
                return new Logger(message =>
                {
                    txt.Invoke(new Action<string>(text => txt.AppendText(text)), message + Environment.NewLine);
                });
            }).As<ILog>().AsSelf();

            _builder.Register(f => new TrayMenu()).As<ITrayMenu>().AsSelf();
            _builder.Register(c => new CommitSkipper()).As<ICommitSkipper>();

            _builder.RegisterType<Updater>().As<IUpdater>();

            _builder.RegisterType<TechnicalInfoSkipper>().As<ITechnicalInfoSkipper>().AsSelf();

            _builder.RegisterType<GitLog>().As<IRepositoryLog>().AsSelf();
            _builder.RegisterType<MercurialLog>().As<IRepositoryLog>().AsSelf();
            _builder.RegisterType<JiraCommitEmulator>().As<IRepositoryLog>().AsSelf();
            _builder.RegisterAggregateService<IAllRepositoryProviders>();

            _builder.RegisterType<TaskTimeSpread>().As<ITaskTimeSpread>().AsSelf();
            _builder.RegisterType<TaskTimeByCommitsCalculator>().As<ITaskTimeCalculator>().AsSelf();

            _builder.RegisterType<WorkLogTasksProcessor>().AsSelf().As<ITasksProcessor>();
            _builder.RegisterType<MeetingProcessor>().AsSelf().As<ITasksProcessor>();
            _builder.RegisterAggregateService<IAllTasksProcessors>();

            _builder.RegisterType<JiraDescriptionSource>().AsSelf().As<IJiraDescriptionSource>();
            _builder.RegisterType<JiraApi>().AsSelf().AsImplementedInterfaces();

            _builder.RegisterType<JiraHistory>().AsSelf().As<IJiraHistory>();

            _builder.Register(c => new Job(c.Resolve<IAllRepositoryProviders>(), c.Resolve<ITaskTimeCalculator>(),
                c.Resolve<IJiraApi>(), c.Resolve<ILog>())).AsSelf();

            _builder.RegisterType<AutoStartUp>().AsSelf().AsImplementedInterfaces();
            _builder.RegisterType<frmSettings>().AsSelf().AsImplementedInterfaces();

            var container = _builder.Build();
            return container;
        }
    }
}
