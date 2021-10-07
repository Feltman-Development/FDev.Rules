//using Autofac;

namespace FDEV.Rules.Demo.Domain.Common.Dynamic
{
    public interface IDependencies
    {

    }

    public class DependencyRegistration
    {
        public void Populate()
        {    
            //var builder = new ContainerBuilder();
            //builder.RegisterType<Entity>().As<IEntity>().InstancePerRequest(); 
            //builder.RegisterType<CommandDispatcher>().As<ICommandDispatcher>();

            //builder.Register(ctx => 
            //{
            //    var id = ctx.Resolve<IEntity>().Uid; 
            //    return new CommandDispatcher();
            //}).As<ICommandDispatcher>();
        }

        //public void PopulateRegistrations()
        //{
        //    var builder = new ContainerBuilder();          
        //    //builder.RegisterControllers(Assembly.GetExecutingAssembly());
        //    builder.RegisterType<CommandDispatcher>().As<ICommandDispatcher>().As().InstancePerLifetimeScope();
        //    //builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerHttpRequest();
        //    //builder.RegisterType<DatabaseFactory>().As<IDatabaseFactory>().InstancePerHttpRequest();
        //    builder.RegisterAssemblyTypes(typeof(Repository).Assembly)
        //        .Where(t => t.Name.EndsWith("Repository"))
        //        .AsImplementedInterfaces().InstancePerHttpRequest();          
        //    var services = Assembly.Load("EFMVC.Domain");
        //    builder.RegisterAssemblyTypes(services)
        //        .AsClosedTypesOf(typeof(ICommandHandler<>)).InstancePerHttpRequest();
        //    builder.RegisterAssemblyTypes(services)
        //        .AsClosedTypesOf(typeof(IValidationHandler<>)).InstancePerHttpRequest();
        //    builder.RegisterType<DefaultFormsAuthentication>().As<IFormsAuthentication>()
        //        .InstancePerHttpRequest();
        //    builder.RegisterFilterProvider();
        //    IContainer container = builder.Build();                  
        //    DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        //}      

            
        
    }
}