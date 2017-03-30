using System;
using System.Fabric.Description;

namespace Ksj.Mealplan.Service
{
    public class ServiceFabricConfigurationSettings
    {
        private readonly ConfigurationSettings configurationSettings;
        private const string ConnectionsStringConfigurationSection = "ConnectionStrings";
        private const string AppSettingsConfigurationSection = "AppSettings";
        private const string EnvironmentConfigurationKey = "environment";

        public ServiceFabricConfigurationSettings(ConfigurationSettings configurationSettings)
        {
            if (configurationSettings == null)
                throw new ArgumentNullException("configurationSettings");
            this.configurationSettings = configurationSettings;
        }

        public T GetAppSetting<T>(string key, T defaultValue)
        {
            ConfigurationProperty parameter = this.GetAppSettingsConfigurationSection().Parameters[key];
            string str = parameter != null ? parameter.Value : (string)null;
            if (str == null)
                return default(T);
            Type conversionType = typeof(T);
            return (T)Convert.ChangeType((object)str, conversionType);
        }

        private ConfigurationSection GetAppSettingsConfigurationSection()
        {
            ConfigurationSection section = this.configurationSettings.Sections["AppSettings"];
            if (section != null)
                return section;
            throw new ArgumentException(string.Format("Unable to find a ConfigurationSection named {0} in service configuration settings.", (object)"AppSettings"));
        }

        public string GetConnectionString(string connectionStringName)
        {
            if (string.IsNullOrEmpty(connectionStringName))
                throw new ArgumentNullException(connectionStringName);
            ConfigurationSection section = this.configurationSettings.Sections["ConnectionStrings"];
            if (section == null)
                throw new ArgumentException(string.Format("Unable to find a ConfigurationSection named {0} in service configuration settings.", (object)"ConnectionStrings"));
            ConfigurationProperty parameter = section.Parameters[connectionStringName];
            if (parameter == null)
                return (string)null;
            return parameter.Value;
        }

        public EnvironmentEnum GetEnvironment()
        {
            ConfigurationProperty parameter = this.GetAppSettingsConfigurationSection().Parameters["environment"];
            if (parameter == null)
                throw new InvalidOperationException(string.Format("Could not retrieve environment from configuration. Missing '{0}' Parameter in {1}", (object)"environment", (object)"AppSettings") + "Possible values for entry: local, development, staging, production");
            try
            {
                return (EnvironmentEnum)Enum.Parse(typeof(EnvironmentEnum), parameter.Value);
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException(string.Format("Could not convert value '{0}' to type of {1}", (object)parameter.Value, (object)typeof(EnvironmentEnum).Name) + "Possible values for entry: local, development, staging, production");
            }
        }
        public enum EnvironmentEnum
        {
            Local,
            Development,
            Integration,
            Staging,
            Production,
        }
    }
}
