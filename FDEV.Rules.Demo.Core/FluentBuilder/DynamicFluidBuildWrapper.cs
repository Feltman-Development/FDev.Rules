using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FDEV.Rules.Demo.Core.FluentBuilder
{
    /// Based on the constructors,
    ///  1. get the parameter information for each constructor
    ///  2. with the parameter information, we have the names of each of the
    ///     parameters. These will allow us to use With{name} methods. For setting
    ///     data on the builder to be passed to an applicable constructor.
    ///  3. the temporary data set via the With{name} methods will be stored
    ///     in a dictionary
    ///  4. when Build() is called, the values of all the parameters from the
    ///     constructor are passed to the constructor, and an instance of T
    ///     is returned
    /// </summary>
    /// <typeparam name="T">Type to Build</typeparam>
    public class DynamicFluentBuilder<T> : DynamicObject
    {
        /// <summary>
        /// stores the values passed when using the
        /// - With{name}
        /// - {isBoolean}
        /// named methods
        /// </summary>
        protected Dictionary<string, object> ValuesFromBuilder = new Dictionary<string, object>();

        /// <summary>
        /// Final list of methods to pass to the constructor
        ///
        /// This is a union of manually defined defaults set in the builder,
        /// and C# defaults for any other fields required of the constructor
        /// </summary>
        protected Dictionary<string, object> Values = new Dictionary<string, object>();

        /// <summary>
        /// anything not passed to the constructor is stored here, and will
        /// be set after construction
        /// </summary>
        protected Dictionary<string, object> ValuesNotSetByConstructor = new Dictionary<string, object>();

        private readonly HelpersForT<T> _helpersForT;

        public DynamicFluentBuilder()
        {
            _helpersForT = new HelpersForT<T>();
            ReadManuallySetDefaults();
        }

        #region List of FieldInformation


        /// <summary>
        /// Manually set overrides. These are defined in the builder class and override C#'s defaults.
        /// </summary>
        protected FieldInfo[] DefaultOverideFieldInfos;
        public FieldInfo[] ManuallySetDefaultFields => DefaultOverideFieldInfos ?? GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        #endregion

        #region Method is Missing? Check for With{FieldName} Pattern

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            var firstArgument = args[0];
            var methodName = binder.Name;
            var fieldName = methodName;

            result = this;

            // for booleans, since the field / property should be named as
            // a question, using "With" doesn't make sense.
            // so, this logic is only needed when we are not setting a
            // boolean field.
            if (firstArgument is not bool)
            {
                // if we are not setting a bool, and we aren't starting
                // with "With", this method is not part of the
                // fluent builder pattern.
                if (!methodName.Contains("With")) return false;
                fieldName = FluidUtility.RemoveWith(methodName);
            }

            var variableName = FluidUtility.PublicNameToVariableName(fieldName);

            // store for later
            // - will be used for construction and / or
            // - post-instantiation field/property setting
            ValuesFromBuilder[variableName] = firstArgument;

            return true;
        }

        #endregion

        #region T's Construction
        /// <summary>
        /// Returns the built object
        /// </summary>
        public virtual T Build()
        {
            var model = Construct();
            SetRemainingFieldsOnModel(model);
            return model;
        }

        private T Construct()
        {
            var constructor = _helpersForT.Constructors.First();
            var parameters = constructor.GetParameters();

            BuildParameterValueMap(parameters);
            BuildPostConstructionValueMap();

            var parameterValues = parameters.Select(p => Values[p.Name]).ToArray();
            return (T)constructor.Invoke(parameterValues);
        }

        #endregion

        #region Value Map Preparation

        private void BuildPostConstructionValueMap()
        {
            foreach (var kvp in ValuesFromBuilder)
            {
                var name = kvp.Key;
                var value = kvp.Value;
                if (Values.ContainsKey(name)) continue;
                
                ValuesNotSetByConstructor[name] = value;
            }
        }

        private void ReadManuallySetDefaults()
        {
            foreach (var field in ManuallySetDefaultFields)
            {
                ValuesFromBuilder[field.Name] = field.GetValue(this);
            }
        }

        private void BuildParameterValueMap(ParameterInfo[] parameters)
        {
            _ = FluidUtility.GetParameterNames(parameters);
            foreach (var parameter in parameters)
            {
                var parameterName = parameter.Name;
                var parameterType = parameter.ParameterType;
                if (ValuesFromBuilder.ContainsKey(parameterName)) // does this exist in the values from builder?
                {
                    Values[parameterName] = ValuesFromBuilder[parameterName];
                }
                else // it doesn't exist in the values from builder, so use a default value
                {
                    Values[parameterName] = GetDefaultValueForParameterName(name: parameterName, type: parameterType);
                }
            }
        }

        private object GetDefaultValueForParameterName(string name, Type type)
        {
            var fieldMatchingParameter = (from field in ManuallySetDefaultFields
                                          where field.Name == name
                                          select field).FirstOrDefault();

            if (fieldMatchingParameter != null)
            {
                return fieldMatchingParameter.GetValue(null);
            }
            return FluidUtility.DefaultValueForType(type);
        }

        #endregion

        private void SetRemainingFieldsOnModel(T model)
        {
            foreach (var kvp in ValuesNotSetByConstructor)
            {
                var name = kvp.Key;
                var value = kvp.Value;
                name = FluidUtility.VariableNameToPublicName(name);
                dynamic field = _helpersForT.GetFieldInfo(fromName: name);
                _helpersForT.SetValueOn(instance: model, field: field, value: value);
            }
        }
    }
}
