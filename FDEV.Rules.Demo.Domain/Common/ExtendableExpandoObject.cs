using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace FDEV.Rules.Demo.Domain.Common
{
    /// <summary>
    /// A special class made to have the functionality of an ExpandoObject that is not sealed, as there is a lot of
    /// possibilities in this, without having to implement 'IDynamicMetaObjectProvider' with every type, which is what
    /// it would take (MS: You can fix this!). In this way, the implementation of getting, setting and consolidating
    /// typed members, pure 'Expando' members and 'Expando-as-Dictionary' members will be done in a consistent way, and since it's implemented
    /// only once, making it worth the effort to implement as easy and intuitive as possible - hiding all complexity.
    /// NOTE: To really get value for money, one more type will be made: the DynamicEntity, with everything that
    ///       makes an entity merged with the possibilities of this beast of a dynamic type. A real kinder egg of an
    ///       entity if you will, providing all of three ways to handle data and methods:
    /// - 1: Directly: any explicitly declared properties are accessible
    /// - 2: Dynamic: dynamic cast allows access to dictionary and native properties/methods
    /// - 3: Dictionary: Any of the extended properties are accessible via IDictionary interface
    /// The toys are here for sure, we're only missing the sorry excuse for chocolate :-)
    /// </summary>
    public class ExtendableExpandoObject : DynamicObject
    {
        protected ExtendableExpandoObject(object instanceToInsert = null, Dictionary<string, object> properties = null)
        {
            ExpandoInstance = new ExpandoObject();
            ExpandoInstance.HasProperty = new Func<string, bool>(name => ((IDictionary<string, object>)ExpandoInstance).ContainsKey(name));
            ExpandoInstance.GetValue = new Func<string, object>(name =>
            {
                ((IDictionary<string, object>)ExpandoInstance).TryGetValue(name, out var value);
                return value;
            });
            InitializeInjectedObject(instanceToInsert);
            InitializePropertyDictionary(properties);
        }

        #region The three property containers

        /// <summary>
        /// Get the internal ExpandoObject, that you can use for true dynamic implentation, making a complex object
        /// graph and nest anything your away.
        /// </summary>
        public dynamic ExpandoInstance { get; }

       

        /// <summary>
        /// Instance of object passed in.
        /// </summary>
        public object InjectedInstance { get; private set; }

        /// <summary>
        /// Cached type of the instance.
        /// </summary>
        public Type InjectedInstanceType { get; private set; }

        /// <summary>
        /// Get all the public properties on the dynamic object in a list.
        /// </summary>
        public IEnumerable<PropertyInfo> InjectedInstancePropertyList =>  InjectedInstance.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
        
        /// <summary>
        /// Get all the public properties on the dynamic object in a dictionary, with the name of the property as key.
        /// </summary>
        public Dictionary<string, PropertyInfo> InjectedInstancePropertyDictionary => InjectedInstancePropertyList.ToDictionary(x => x.Name, x => x);

        /// <summary>
        /// String Dictionary that contains the extra non-dynamic values stored on this object/instance
        /// </summary>
        public Dictionary<string, object> Properties { get; private set; }

        #endregion The three property containers


        #region Initialising and populating thos containers

        /// <summary>
        /// Initialize inner dynamic object, which means that it will be assigned to the ExpandoInstance. Note that 
        /// the object does not have to be a dynamic, It can be any object and you dont even need to know the type.
        /// </summary>
        protected void InitializeInjectedObject(object instanceToInject)
        {
            InjectedInstance = instanceToInject;
            if (InjectedInstance != null)
                InjectedInstanceType = InjectedInstance.GetType();
        }

        /// <summary>
        /// Initialize property dictionary.
        /// TEST: Check if an assignment is good enough or if we have to use recursive method outcommnted below?!
        /// </summary>
        protected void InitializePropertyDictionary(Dictionary<string, object> properties) => Properties = properties ?? new Dictionary<string, object>();

        //TODO: Check if the recursive of the method below is necessary and implement if it is...
        ///// <summary>
        ///// Instantiate an Expando from a dictionary
        ///// </summary>
        //protected ExpandoObject InitializePropertyDictionary(IDictionary<string, object> properties)
        //{
        //    var dynamic = this;
        //    SetDynamicObject(dynamic);
        //    foreach (var property in properties)
        //    {
        //        var propertyValue = property.Value;
        //        if (propertyValue is IDictionary<string, object>)
        //        {
        //            dynamic[property.Key] = CreateDynamicEntity(propertyValue);
        //        }
        //        else if (property.Value is ICollection collection)
        //        {
        //            var objList = new List<object>();
        //            foreach (var item in collection)
        //            {
        //                if (item is IDictionary<string, object> itemVals)
        //                {
        //                    var expandoItem = new ExpandoExt(itemVals);
        //                    objList.Add(expandoItem);
        //                }
        //                else
        //                {
        //                    objList.Add(item);
        //                }
        //            }
        //            dynamic[property.Key] = objList;
        //        }
        //        else
        //        {
        //            dynamic[property.Key] = propertyValue;
        //        }
        //    }
        //}


        #endregion Initialising and populating thos containers


        #region All the diffrent ways of getting data from this type
        
        /// <summary>
        /// Method that provides a string Indexer to the Properties collection AND the strongly typed properties of the object by name.
        /// - Enter Dynamic:   exp["Address"] = "112 nowhere lane";
        /// - Fetch Strong:    var name = exp["StronglyTypedProperty"] as string;
        /// - The getter checks the Properties dictionary first then looks in PropertyInfo for properties.
        /// - The setter checks the instance properties before  checking the Properties dictionary.
        /// </summary>
        public object this[string key]
        {
            get
            {
                try
                {
                    return Properties[key];
                }
                catch (KeyNotFoundException)
                {
                    if (GetProperty(InjectedInstance, key, out object result)) return result;

                    //TODO: Log/Handle ex.
                    throw;
                }
            }
            set
            {
                if (Properties.ContainsKey(key))
                {
                    Properties[key] = value;
                    return;
                }

                var members = InjectedInstanceType.GetMember(key, BindingFlags.Public | BindingFlags.GetProperty);
                if (members is { Length: > 0 })
                {
                    SetProperty(InjectedInstance, key, value);
                }
                else
                {
                    Properties[key] = value;
                }
            }
        }

        /// <summary>
        /// Returns and the properties of the inner instance.
        /// </summary>
        public IEnumerable<KeyValuePair<string,object>> GetProperties(bool includeInjectedInstanceProperties = false)
        {
            if (includeInjectedInstanceProperties && InjectedInstance != null)
            {
                foreach (var prop in InjectedInstancePropertyList)
                {
                    yield return new KeyValuePair<string, object>(prop.Name, prop.GetValue(InjectedInstance, null));
                }
            }

            foreach (var key in Properties.Keys)
            {
                yield return new KeyValuePair<string, object>(key, Properties[key]);
            }
        }

        /// <summary>
        /// Return both instance and dynamic names. Important to return both so JSON serialization with Json.NET works.
        /// </summary>
        public override IEnumerable<string> GetDynamicMemberNames() => GetProperties(true).Select(prop => prop.Key);

        /// <summary>
        /// Generic helper to get named members of the given type (property/field/method...). 
        /// </summary>
        protected TMember GetMemberInfo<TMember>(string memberName, BindingFlags definingFlags) where TMember : MemberInfo
        {
            try
            {
                return InjectedInstance.GetType().GetMember(memberName, definingFlags | BindingFlags.Public | BindingFlags.Instance)[0] as TMember;
            }
            catch (ArgumentNullException)
            {
                return null;
            }
        }

        /// <summary>
        /// Checks whether a property exists in the Property collection, as a property on the instance or as a property
        /// on the ExpandoObject.
        /// </summary>
        public bool Contains(KeyValuePair<string, object> item, bool includeTypedProperties = true, bool includeInstanceProperties = true, bool includeExpandoProperties = true)
        {
            if (includeTypedProperties) return this.GetProperties().Any(x => x.Key == item.Key);
            if (includeExpandoProperties) return ExpandoInstance != null && ((IDictionary<string, object>)ExpandoInstance).ContainsKey(item.Key);
            return (includeInstanceProperties || InjectedInstance != null) && InjectedInstancePropertyList.Any(x => x.Name == item.Key);
        }

        /// <summary>
        /// Reflection Helper method to retrieve a property.
        /// </summary>
        protected bool GetProperty(object instance, string name, out object result)
        {
            instance ??= this;

            var members = InjectedInstanceType.GetMember(name, BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance);
            if (members is { Length: > 0 })
            {
                var member = members[0];
                if (member.MemberType == MemberTypes.Property)
                {
                    result = ((PropertyInfo)member).GetValue(instance,null);
                    return true;
                }
            }
            result = null;
            return false;
        }

        /// <summary>
        /// Try to retrieve a member by name first from instance properties followed by the collection entries.
        /// </summary>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (Properties.Keys.Contains(binder.Name))
            {
                result = Properties[binder.Name];
                return true;
            }

            if (InjectedInstance != null)
            {
                try{return GetProperty(InjectedInstance, binder.Name, out result);}
                catch {}//TODO: Nothing? / Log?
            }
            result = null;
            return false;
        }
        
        #endregion All the diffrent ways of getting data from this type

        
        /// <summary>
        /// Property setter implementation tries to retrieve value from instance first then into this object.
        /// </summary>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (InjectedInstance != null)
            {
                try
                {
                    var result = SetProperty(InjectedInstance, binder.Name, value);
                    if (result) return true;
                }
                catch
                {
                    return false;
                }
            }
            Properties[binder.Name] = value;
            return true;
        }

        /// <summary>
        /// Dynamic invocation method. Currently allows only for Reflection based operation.
        /// </summary>
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            if (InjectedInstance != null)
            {
                try{return InvokeMethod(binder.Name, args, out result);}
                catch {}//TODO: Nothing?
            }
            result = null;
            return false;
        }

        /// <summary>
        /// Set a property value.
        /// </summary>
        protected bool SetProperty(object instance, string name, object value)
        {
            instance ??= this;

            var members = InjectedInstanceType.GetMember(name, BindingFlags.Public | BindingFlags.SetProperty | BindingFlags.Instance);
            if (members.Length == 0) return false;

            var member = members[0];
            if (member.MemberType != MemberTypes.Property) return false;

            ((PropertyInfo)member).SetValue(instance, value, null);
            return true;
        }

        /// <summary>
        /// Invoke a method, if it exists on the type
        /// </summary>
        protected bool InvokeMethod(string name, object[] args, out object result)
        {
            var method = GetMemberInfo<MethodInfo>(name, BindingFlags.InvokeMethod);
            if (method != null)
            {
                result = method.Invoke(InjectedInstance, args);
                return true;
            }
            result = null;
            return false;
        }
    }
}