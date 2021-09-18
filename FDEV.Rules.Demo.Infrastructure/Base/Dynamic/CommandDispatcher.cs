using System;
using System.Linq;
using System.Reflection;

namespace FSites.Core.Domain.Dynamic
{
    public interface ICommandDispatcher
    {
        public void Dispatch<TCommand>(TCommand command) where TCommand : ICommand;
    }

    public class CommandDispatcher :  ICommandDispatcher
    {
        public CommandDispatcher()
        {
            //Write membername and parameters to log...
        }

        public void Dispatch<TCommand>(TCommand command) where TCommand : ICommand
        {
            var interfaceType = typeof(ICommandHandler<>);
            var genericType = interfaceType.MakeGenericType(command.GetType());

            var concreteTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsClass && t.GetInterfaces().Contains(genericType)).ToList();
            if (!concreteTypes.Any()) return;

            foreach (var concreteType in concreteTypes)
            {
                var concreteHandler = Activator.CreateInstance(concreteType) as ICommandHandler<TCommand>;
                concreteHandler?.HandleCommand(command);
            }
        }
    }

}