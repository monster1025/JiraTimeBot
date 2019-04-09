using Autofac;
using Autofac.Extras.AggregateService;
using JiraTimeBot.UI.Tray;
using System;
using System.Windows.Forms;
using JiraTimeBot.Core;
using JiraTimeBot.Core.Configuration;
using JiraTimeBot.Core.JiraIntegration;
using JiraTimeBot.Core.JiraIntegration.Comments;
using JiraTimeBot.Core.Mercurial;
using JiraTimeBot.Core.Mercurial.Modifiers;
using JiraTimeBot.Core.StartUp;
using JiraTimeBot.Core.TasksProcessors;
using JiraTimeBot.Core.TaskTime;
using JiraTimeBot.Core.Update;
using JiraTimeBot.Core.Update.Update;
using JiraTimeBot.UI;

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

            _builder.RegisterType<MercurialLog>().As<IMercurialLog>().AsSelf();
            _builder.RegisterType<JiraCommitEmulator>().As<IMercurialLog>().AsSelf();
            _builder.RegisterAggregateService<IAllMercurialProviders>();

            _builder.RegisterType<TaskTimeByCommitsCalculator>().As<ITaskTimeCalculator>().AsSelf();

            _builder.RegisterType<WorkLogTasksProcessor>().AsSelf().As<ITasksProcessor>();
            _builder.RegisterType<MeetingProcessor>().AsSelf().As<ITasksProcessor>();
            _builder.RegisterAggregateService<IAllTasksProcessors>();

            _builder.RegisterType<JiraDescriptionSource>().AsSelf().As<IJiraDescriptionSource>();
            _builder.RegisterType<JiraApi>().AsSelf().AsImplementedInterfaces();
            
            _builder.Register(c => new Job(c.Resolve<IAllMercurialProviders>(), c.Resolve<ITaskTimeCalculator>(), c.Resolve<ILog>())).AsSelf();

            _builder.RegisterType<AutoStartUp>().AsSelf().AsImplementedInterfaces();
            _builder.RegisterType<frmSettings>().AsSelf().AsImplementedInterfaces();
            _builder.RegisterType<SettingsManager>().AsSelf().AsImplementedInterfaces().SingleInstance();

            var container = _builder.Build();
            return container;
        }
    }
}
