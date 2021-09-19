using System;
using System.Collections.Generic;
using System.Linq;

namespace FDEV.Rules.Demo.Domain.Common.Dynamic
{
    /// <summary>
    /// Generic interface for implementing dynamic CommandHandlers, injected with a command type as TCommand
    /// </summary>
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        void HandleCommand(TCommand command);
    }

    /// <summary>
    /// ICommandHandler implementation example
    /// </summary>
    public class TraceLogMemberHandler : ICommandHandler<TraceLogMemberCommand>
    {
        public void HandleCommand(TraceLogMemberCommand command)
        {
            var parameters = command.Parameters;
            if (parameters.TryGetValue("MemberName", out var value) == false) throw new ArgumentException("Membername");

            for (var i = 1; i <= parameters.Count; i++)
            {
                var parameterKey = parameters.ElementAt(i).Key;
                var parameterValue = parameters.ElementAt(i).Value;
                //Log membername and parameter names and values
            }
        }
    }

    /// <summary>
    /// A command type (any class) to replace the generic TCommand, in concrete implementations of ICommandHandler
    /// </summary>
    public interface ICommand
    {
        string CommandName { get; }

        Dictionary<string, object> Parameters { get; }
    }

    /// <summary>
    /// Implementation of ICommand to replace the generic TCommand, in concrete implementations of ICommandHandler
    /// </summary>
    public class TraceLogMemberCommand : ICommand
    {
        public TraceLogMemberCommand(string commandName, Dictionary<string, object> parameters)
        {
            CommandName = commandName;
            Parameters = parameters;
        }

        public string CommandName { get; }

        public Dictionary<string, object> Parameters { get; }
    }
}
