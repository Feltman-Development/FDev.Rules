using System;
using System.Linq;
using System.Reflection;

namespace FDEV.Rules.Demo.Core.FluentBuilder
{
    internal class HelpersForT<T>
    {
        private ConstructorInfo[] _constructorInfo;
        public ConstructorInfo[] Constructors => _constructorInfo ??= typeof(T).GetConstructors();

        /// <summary>
        /// Properties that we may want to set after construction if a constructor parameter doesn't exist with the same name
        /// </summary>
        private PropertyInfo[] _propertyInfos;
        public PropertyInfo[] PropertyInformation => _propertyInfos ??= typeof(T).GetProperties();

        /// <summary>
        /// Fields that we may want to set after construction if a constructor parameter doesn't exist with the same name
        /// </summary>
        protected FieldInfo[] FieldInfos;
        public FieldInfo[] FieldInformation => FieldInfos ?? typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);

        /// <summary>
        /// Because fields, properties and methods must all have a unique name, we can check each set of fields / properties to see if there is a match
        /// </summary>
        public dynamic GetFieldInfo(string fromName)
        {
            dynamic result = Array.Find(FieldInformation, field => field.Name == fromName);
            if (result != null) return result;

            // if the fields don't have what we are looking for, try the properties.
            return Array.Find(PropertyInformation, field => field.Name == fromName);
        }

        /// <summary>
        /// sets the value on the specified field for the specified object
        /// </summary>
        /// <returns>true if the field has been set with the value</returns>
        public bool SetValueOn(T instance, dynamic field, object value)
        {
            if (field == null) return false;

            if (field is FieldInfo fi)
            {
                value = Convert.ChangeType(value, fi.FieldType);
                fi.SetValue(instance, value);
            }
            else if (field is PropertyInfo propertyInfo)
            {
                value = Convert.ChangeType(value, propertyInfo.PropertyType);
                propertyInfo.SetValue(instance, value, null);
            }
            else
            {
                return false;
            }

            return true;
        }
    }
}
