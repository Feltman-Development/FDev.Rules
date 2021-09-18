using System;

namespace FDEV.Rules.Demo.Core.Logging.CONSOLIDATE_CONFIG.Config
{
    interface IConfigurationArgumentValue
    {
        object ConvertTo(Type toType, ResolutionContext resolutionContext);
    }
}
