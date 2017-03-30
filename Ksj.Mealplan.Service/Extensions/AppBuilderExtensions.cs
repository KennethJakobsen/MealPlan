using System;
using System.Fabric.Description;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Services.Runtime;
using Owin;

namespace Ksj.Mealplan.Service.Extensions
{
    public static class AppBuilderExtensions
    {
            private const string AppRootKey = "AppRootKey";
            private const string ReliableStateManagerKey = "ReliableStateManagerKey";
            private const string ConfigurationPackageSettingKey = "ConfigurationPackageSetting";

            internal static void SetReliableStateManager(this IAppBuilder builder, IReliableStateManager stateManager)
            {
                if (stateManager == null)
                    return;
                builder.Properties.Add("ReliableStateManagerKey", (object)stateManager);
            }

            public static IReliableStateManager GetReliableStateManager(this IAppBuilder builder)
            {
                if (!builder.Properties.ContainsKey("ReliableStateManagerKey"))
                    throw new InvalidOperationException(string.Format("Tried to retrieve {0} from {1}. ", (object)typeof(IReliableStateManager).Name, (object)typeof(IAppBuilder).Name) + string.Format("The {0} is only available from Service fabric {1}", (object)typeof(IReliableStateManager).Name, (object)typeof(StatefulService).Name));
                return builder.Properties["ReliableStateManagerKey"] as IReliableStateManager;
            }

            internal static void SetApproot(this IAppBuilder builder, string approot)
            {
                if (string.IsNullOrWhiteSpace(approot))
                    return;
                builder.Properties.Add("AppRootKey", (object)approot);
            }

            public static string GetAppRoot(this IAppBuilder builder)
            {
                if (!builder.Properties.ContainsKey("AppRootKey"))
                    throw new InvalidOperationException(string.Format("Tried to retrieve '{0}' from {1}", (object)"AppRootKey", (object)typeof(IAppBuilder).Name));
                return builder.Properties["AppRootKey"] as string;
            }

            internal static void SetServiceFabricConfigurationSettings(this IAppBuilder builder, ConfigurationSettings configurationSettings)
            {
                if (configurationSettings == null)
                    return;
                builder.Properties.Add("ConfigurationPackageSetting", (object)configurationSettings);
            }

            public static ConfigurationSettings GetServicePackageConfigurationSettings(this IAppBuilder builder)
            {
                if (!builder.Properties.ContainsKey("ConfigurationPackageSetting"))
                    throw new InvalidOperationException(string.Format("Tried to retrieve '{0}' from {1}", (object)typeof(ConfigurationSettings).Name, (object)typeof(IAppBuilder).Name));
                return builder.Properties["ConfigurationPackageSetting"] as ConfigurationSettings;
            }
        
    }
}
