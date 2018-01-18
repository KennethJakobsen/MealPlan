using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ksj.Mealplan.Messages;
using Ksj.Mealplan.Service;
using Ksj.Mealplan.Service.Messages;
using LightInject;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.ServiceFabric;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Rebus.Activation;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using ServiceFabric.AutoRest.Communication.Client;
using Ksj.Service.Client;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;

namespace Ksj.Mealplan.Gateway
{
    /// <summary>
    /// The FabricRuntime creates an instance of this class for each service type instance. 
    /// </summary>
    internal sealed class Gateway : StatelessService
    {

        private const string InputQueue = "mealplan-input";
        private const string ErrorQueue = "mealplan-error";
        private readonly ServiceFabricConfigurationSettings _configurationSettings;
        private IBus _bus;
        private readonly ServiceContainer _container;

        public Gateway(StatelessServiceContext context)
            : base(context)
        {
            var client = new FabricClient();
            var communicationClientFactory = new RestCommunicationClientFactory<Mealservice>(new ServicePartitionResolver(() => client));
            var partitionClient = new RestServicePartitionClient<Mealservice>(communicationClientFactory, new Uri("fabric:/Ksj.Mealplan/Service"), new ServicePartitionKey(long.MinValue), TargetReplicaSelector.RandomReplica);
            var telemetryConfig = TelemetryConfiguration.Active;
            FabricTelemetryInitializerExtension.SetServiceCallContext(context);
            var config = context.CodePackageActivationContext.GetConfigurationPackageObject("Config");
            var appInsights = config.Settings.Sections["ApplicationInsights"];
            telemetryConfig.InstrumentationKey = appInsights.Parameters["InstrumentationKey"].Value;
            _configurationSettings = GetServiceConfiguration();
            _container = new ServiceContainer();
            _container.RegisterInstance<IRestServicePartitionClient<Mealservice>>(partitionClient);

        }

        /// <summary>
        /// Optional override to create listeners (like tcp, http) for this service instance.
        /// </summary>
        /// <returns>The collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new[]
            {
                new ServiceInstanceListener(serviceContext => new OwinCommunicationListener((builder) =>
                {
                    Startup.ConfigureApp(builder, _container);
                },
                serviceContext, 
                null, 
                "ServiceEndpoint", 
                "api"))
            };
        }

        protected override Task RunAsync(CancellationToken cancellationToken)
        {

            if (!cancellationToken.IsCancellationRequested)
            {
                _bus = Configure.With(new BuiltinHandlerActivator())
                    .Logging(x => x.Trace())
                    .Transport(x =>
                        x.UseAzureServiceBusAsOneWayClient(
                            _configurationSettings.GetConnectionString("AzureServiceBus")))
                    .Routing(r =>
                        r.TypeBased()
                            .Map<AddGroceryMessage>(InputQueue)
                            .Map<AddMealMessage>(InputQueue)
                    )
                    .Start();
                _container.RegisterInstance<IBus>(_bus);
            }
            return base.RunAsync(cancellationToken);
        }

        protected override Task OnCloseAsync(CancellationToken cancellationToken)
        {

            _bus.Dispose();
            return base.OnCloseAsync(cancellationToken);
        }

        private ServiceFabricConfigurationSettings GetServiceConfiguration()
        {
            var configurationPackageObject = Context.CodePackageActivationContext.GetConfigurationPackageObject("Config");
            return new ServiceFabricConfigurationSettings(configurationPackageObject.Settings);
        }
    }

}
