using Serilog.Configuration;

namespace FDEV.Rules.Demo.Core.Logging.CONSOLIDATE_CONFIG.Config
{
    interface IConfigurationReader : ILoggerSettings
    {
        void ApplySinks(LoggerSinkConfiguration loggerSinkConfiguration);
        void ApplyEnrichment(LoggerEnrichmentConfiguration loggerEnrichmentConfiguration);
    }
}
