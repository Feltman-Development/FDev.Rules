using System;
using System.Dynamic;
using System.Linq;

namespace FDEV.Rules.Demo.Domain.Common.Notification
{
    /// <summary>
    /// Resolves your viewmodel and gives you instant access to it
    /// EXAMPLE OF BINDING:
    /// <Border DataContext="{Binding YourViewModelName, Source={StaticResource ViewModelLocator}}"> 
    /// </summary>
    public class ViewModelLocator : DynamicObject
    {
        public ViewModelLocator() => Resolver = new SimpleViewModelResolver();

        public IViewModelResolver Resolver { get; set; }

        public object this[string viewModelName] => Resolver.Resolve(viewModelName);

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = this[binder.Name];
            return true;
        }
    }

    /// <summary>
    /// Resolves and creates an instance of the view model if it exists in the assembly
    /// </summary>
    public class SimpleViewModelResolver : IViewModelResolver
    {
        public object Resolve(string viewModelNameName)
            => GetType().Assembly.GetTypes().FirstOrDefault(type => type.Name == viewModelNameName)== null ? null
                : Activator.CreateInstance(GetType().Assembly.GetTypes().FirstOrDefault(type => type.Name == viewModelNameName));
    }

    /// <summary>
    /// Interface for an ViewModelResolver
    /// </summary>
    public interface IViewModelResolver
    {
        object Resolve(string viewModelName);
    }
}
