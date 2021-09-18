using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace FDEV.Rules.Demo.Domain.Base
{
    public abstract class ValueObjectBase<TValue> : IEquatable<TValue> where TValue : ValueObjectBase<TValue>
    {
        /// <summary>
        /// Returns true objects are same type and all fields have same value
        /// </summary>
        public override bool Equals([AllowNull] object otherObject) => otherObject == null ? false : Equals(otherObject as TValue);

        /// <summary>
        /// Returns true if all fields on object have same value
        /// </summary>
        public virtual bool Equals([AllowNull] TValue otherValue)
        {
            if (otherValue == null) return false;

            Type thisType = GetType();
            Type otherType = otherValue.GetType();
            if (thisType != otherType) return false;

            var fields = thisType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            foreach (FieldInfo field in fields)
            {
                object value1 = field.GetValue(otherValue);
                object value2 = field.GetValue(this);
                if (value1 == null && value2 != null) return false;
                if (!value1.Equals(value2)) return false;
            }
            return true;
        }

        /// <summary>
        /// Returns hashcode based on multiplier on all value fields
        /// </summary>
        public override int GetHashCode()
        {
            IEnumerable<FieldInfo> fields = GetFields();
            int startValue = 17; //TODO: Move number!
            int multiplier = 59; //TODO: Move number!
            int hashCode = startValue;

            foreach (FieldInfo field in fields)
            {
                object value = field.GetValue(this);
                if (value != null) hashCode = hashCode * multiplier + value.GetHashCode();
            }
            return hashCode;
        }

        private IEnumerable<FieldInfo> GetFields()
        {
            var thisType = GetType();
            var fields = new List<FieldInfo>();
            while (thisType != typeof(object))
            {
                fields.AddRange(thisType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public));
                thisType = thisType.BaseType;
            }
            return fields;
        }

        /// <summary>
        /// Makes a deep compare (see 'equals')
        /// </summary>
        public static bool operator ==(ValueObjectBase<TValue> x, ValueObjectBase<TValue> y) => x.Equals(y);

        /// <summary>
        /// Makes a deep compare (see 'equals')
        /// </summary>
        public static bool operator !=(ValueObjectBase<TValue> x, ValueObjectBase<TValue> y) => !(x == y);
    }
}
