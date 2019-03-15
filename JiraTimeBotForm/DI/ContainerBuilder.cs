using System.Windows.Forms;
using Autofac;
using Autofac.Extras.AggregateService;
using JiraTimeBotForm.JiraIntegration;
using JiraTimeBotForm.Mercurial;
using JiraTimeBotForm.TaskProcessors;
using JiraTimeBotForm.TasksProcessors;
using JiraTimeBotForm.TaskTime;
using JiraTimeBotForm.UI;

namespace JiraTimeBotForm.DI
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
            _builder.Register(f => new Logger(logTextBox)).As<ILog>().AsSelf();
            
            _builder.Register(f => new TrayMenu()).As<ITrayMenu>().AsSelf();
            _builder.Register(c => new CommitSkipper()).As<ICommitSkipper>();

            _builder.Register(c => new MercurialLog(c.Resolve<ILog>(), c.Resolve<ICommitSkipper>())).As<IMercurialLog>().AsSelf();
            _builder.Register(c => new TaskTimeDiscoverer(c.Resolve<ILog>())).As<ITaskTimeDiscoverer>().AsSelf();

            _builder.RegisterType<WorkLogTasksProcessor>().AsSelf().As<ITasksProcessor>();
            _builder.RegisterType<MeetingProcessor>().AsSelf().As<ITasksProcessor>();
            _builder.RegisterAggregateService<ITasksProcessors>();

            _builder.RegisterType<JiraDescriptionSource>().AsSelf().As<IJiraDescriptionSource>();
            _builder.RegisterType<JiraApi>().AsSelf().AsImplementedInterfaces();

            _builder.Register(c => new Job(c.Resolve<IMercurialLog>(), c.Resolve<ITaskTimeDiscoverer>(), c.Resolve<ILog>())).AsSelf();

            var container = _builder.Build();
            return container;
        }
    }
}
