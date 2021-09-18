using System;
using FDEV.Rules.Demo.Core.Logging.CONSOLIDATE_CONFIG.Assemblies;
using FDEV.Rules.Demo.Core.Logging.CONSOLIDATE_CONFIG.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyModel;
using Serilog;
using Serilog.Configuration;
using ConfigurationAssemblySource = Serilog.Settings.Configuration.ConfigurationAssemblySource;

namespace FDEV.Rules.Demo.Core.Logging.CONSOLIDATE_CONFIG
{
    /// <summary>
    /// Extends <see cref="LoggerConfiguration"/> with support for System.Configuration appSettings elements.
    /// </summary>
    public static class ConfigurationLoggerExtensions
    {
        /// <summary>
        /// Configuration section name required by this package.
        /// </summary>
        public const string DefaultSectionName = "Serilog";

        /// <summary>
        /// Reads logger settings from the provided configuration object using the provided section name. Generally this is preferable over the other method that takes a configuration section. 
        /// Only this version will populate IConfiguration parameters on target methods.
        /// </summary>
        /// <param name="settingConfiguration">Logger setting configuration.</param>
        /// <param name="configuration">A configuration object which contains a Serilog section.</param>
        /// <param name="sectionName">A section name for section which contains a Serilog section.</param>
        /// <param name="dependencyContext">The dependency context from which sink/enricher packages can be located. If not supplied, the platform default will be used.</param>
        /// <returns>An object allowing configuration to continue.</returns>
        public static LoggerConfiguration Configuration(this LoggerSettingsConfiguration settingConfiguration, IConfiguration configuration, string sectionName, DependencyContext dependencyContext = null)
        {
            if (settingConfiguration == null) throw new ArgumentNullException(nameof(settingConfiguration));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (sectionName == null) throw new ArgumentNullException(nameof(sectionName));

            AssemblyFinder assemblyFinder = dependencyContext == null ? AssemblyFinder.Auto() : AssemblyFinder.ForDependencyContext(dependencyContext);
            return settingConfiguration.Settings(new ConfigurationReader(configuration.GetSection(sectionName), assemblyFinder, configuration));
        }

        /// <summary>
        /// Reads logger settings from the provided configuration object using the default section name. Generally this is preferable over the other method that takes a configuration section. 
        /// Only this version will populate IConfiguration parameters on target methods.
        /// </summary>
        /// <param name="settingConfiguration">Logger setting configuration.</param>
        /// <param name="configuration">A configuration object which contains a Serilog section.</param>
        /// <param name="dependencyContext">The dependency context from which sink/enricher packages can be located. If not supplied, the platform default will be used.</param>
        /// <returns>An object allowing configuration to continue.</returns>
        public static LoggerConfiguration Configuration(this LoggerSettingsConfiguration settingConfiguration, IConfiguration configuration, DependencyContext dependencyContext = null)
            => settingConfiguration.Configuration(configuration, DefaultSectionName, dependencyContext);

        /// <summary>
        /// Reads logger settings from the provided configuration section. Generally it is preferable to use the other extension method that takes the full configuration object.
        /// </summary>
        /// <param name="settingConfiguration">Logger setting configuration.</param>
        /// <param name="configSection">The Serilog configuration section</param>
        /// <param name="dependencyContext">The dependency context from which sink/enricher packages can be located. If not supplied, the platform default will be used.</param>
        /// <returns>An object allowing configuration to continue.</returns>
        [Obsolete("Use ReadFrom.Configuration(IConfiguration configuration, string sectionName, DependencyContext dependencyContext) instead.")]
        public static LoggerConfiguration ConfigurationSection(this LoggerSettingsConfiguration settingConfiguration, IConfigurationSection configSection, DependencyContext dependencyContext = null)
        {
            if (settingConfiguration == null) throw new ArgumentNullException(nameof(settingConfiguration));
            if (configSection == null) throw new ArgumentNullException(nameof(configSection));

            var assemblyFinder = dependencyContext == null ? AssemblyFinder.Auto() : AssemblyFinder.ForDependencyContext(dependencyContext);

            return settingConfiguration.Settings(new ConfigurationReader(configSection, assemblyFinder, configuration: null));
        }

        /// <summary>
        /// Reads logger settings from the provided configuration object using the provided section name. Generally this is preferable over the other method that takes a configuration section. 
        /// Only this version will populate IConfiguration parameters on target methods.
        /// </summary>
        /// <param name="settingConfiguration">Logger setting configuration.</param>
        /// <param name="configuration">A configuration object which contains a Serilog section.</param>
        /// <param name="sectionName">A section name for section which contains a Serilog section.</param>
        /// <param name="configurationAssemblySource">Defines how the package identifies assemblies to scan for sinks and other Types.</param>
        /// <returns>An object allowing configuration to continue.</returns>
        public static LoggerConfiguration Configuration(this LoggerSettingsConfiguration settingConfiguration, IConfiguration configuration, string sectionName, ConfigurationAssemblySource configurationAssemblySource)
        {
            if (settingConfiguration == null) throw new ArgumentNullException(nameof(settingConfiguration));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (sectionName == null) throw new ArgumentNullException(nameof(sectionName));

            var assemblyFinder = AssemblyFinder.ForSource((Assemblies.ConfigurationAssemblySource)configurationAssemblySource);
            return settingConfiguration.Settings(new ConfigurationReader(configuration.GetSection(sectionName), assemblyFinder, configuration));
        }

        /// <summary>
        /// Reads logger settings from the provided configuration object using the default section name. Generally this is preferable over the other method that takes a configuration section. 
        /// Only this version will populate IConfiguration parameters on target methods.
        /// </summary>
        /// <param name="settingConfiguration">Logger setting configuration.</param>
        /// <param name="configuration">A configuration object which contains a Serilog section.</param>
        /// <param name="configurationAssemblySource">Defines how the package identifies assemblies to scan for sinks and other Types.</param>
        /// <returns>An object allowing configuration to continue.</returns>
        public static LoggerConfiguration Configuration(this LoggerSettingsConfiguration settingConfiguration, IConfiguration configuration, ConfigurationAssemblySource configurationAssemblySource)
            => settingConfiguration.Configuration(configuration, DefaultSectionName, configurationAssemblySource);
    }
}

