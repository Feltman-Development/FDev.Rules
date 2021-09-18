using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FRules.Demo.Engine.Domain.Base
{
    /// Enums that can be used across the different layers and classes of the application, helps to ensure less redundancy, and more
    /// importantly: ambiguity(!) while providing better consistency in choices, spellings, order and selection.
    /// Not all Enums belong here, but most do - as it also forces us to think in standards :-)
    public abstract class Enums { }

    public enum YesOrNo
    {
        Yes,
        No
    }

    public enum EnabledOrDisabled
    {
        Enabled,
        Disabled
    }

    public enum OnOrOff
    {
        On,
        Off
    }

    public enum ResourceAccess
    {
        NoAccess,
        Write,
        ReadWrite
    }

    public enum DatabaseType
    {
        Access,
        SqLite,
        SqlCompact,
        SqlServer,
        Oracle,
        LiteDb,
        MongoDb
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum Operator
    {
        Undefined = 0,
        [EnumMember(Value = "$all")]
        All,
        [EnumMember(Value = "$exists")]
        Exists,
        [EnumMember(Value = "$eq")]
        Equal,
        [EnumMember(Value = "$ne")]
        NotEqual,
        [EnumMember(Value = "$gt")]
        GreaterThan,
        [EnumMember(Value = "$gte")]
        GreaterThanOrEqualTo,
        [EnumMember(Value = "$lt")]
        LessThan,
        [EnumMember(Value = "$lte")]
        LessThanOrEqualTo,
        [EnumMember(Value = "$in")]
        In,
        [EnumMember(Value = "$nin")]
        NotIn,
        [EnumMember(Value = "$regex")]
        RegularExpression,
        [EnumMember(Value = "$mod")]
        Modulus,
        [EnumMember(Value = "$size")]
        Size,
        [EnumMember(Value = "$type")]
        Type,
        [EnumMember(Value = "$dateFormat")]
        DateFormat,
    }
}
