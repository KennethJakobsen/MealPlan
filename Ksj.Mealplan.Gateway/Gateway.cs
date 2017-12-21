using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ksj.Mealplan.Messages;
using Ksj.Mealplan.Service;
using Ksj.Mealplan.Service.Messages;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Rebus.Activation;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Routing.TypeBased;

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
        public static IBus Bus;
        public Gateway(StatelessServiceContext context)
            : base(context)
        {
            _configurationSettings = GetServiceConfiguration();
        }
        
        /// <summary>
        /// Optional override to create listeners (like tcp, http) for this service instance.
        /// </summary>
        /// <returns>The collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new ServiceInstanceListener[]
            {
                new ServiceInstanceListener(serviceContext => new OwinCommunicationListener(Startup.ConfigureApp, serviceContext, null, "GatewayServiceEndpoint", "api"))
            };
        }

        protected override Task RunAsync(CancellationToken cancellationToken)
        {

            if (cancellationToken.IsCancellationRequested)
            {
                Bus = Configure.With(new BuiltinHandlerActivator())
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
            }
            return base.RunAsync(cancellationToken);
        }

        protected override Task OnCloseAsync(CancellationToken cancellationToken)
        {
            
            Bus.Dispose();
            return base.OnCloseAsync(cancellationToken);
        }

        private ServiceFabricConfigurationSettings GetServiceConfiguration()
    {
        var configurationPackageObject = Context.CodePackageActivationContext.GetConfigurationPackageObject("Config");
        return new ServiceFabricConfigurationSettings(configurationPackageObject.Settings);
    }
}

}
