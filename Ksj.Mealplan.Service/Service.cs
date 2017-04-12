using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Fabric;
using System.Fabric.Description;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ksj.Mealplan.Infrastructure;
using Ksj.Mealplan.Messages;
using Ksj.Mealplan.Service.Handlers;
using Ksj.Mealplan.Service.IoC;
using Ksj.Mealplan.Service.Messages;
using Microsoft.Owin.Hosting;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Rebus.Activation;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Retry.Simple;
using Owin;
using Rebus.Activation;
using Rebus.LightInject;

namespace Ksj.Mealplan.Service
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class Service : StatefulService
    {
        private const string InputQueue = "mealplan-input";
        private const string ErrorQueue = "mealplan-error";
        
        private readonly ServiceFabricConfigurationSettings configuration;
        private readonly Type[] eventTypes = { typeof(AddGroceryMessage), typeof(AddMealMessage) };
        private IBus bus;

        public Service(StatefulServiceContext context)
            : base(context)
        {
            Bootstrapper.Bootstrap();
            configuration = GetServiceConfiguration();
        }

        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see https://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            //return  new ServiceReplicaListener[0];
            return new[]
            {
                 new ServiceReplicaListener(context => new OwinCommunicationListener(
                        appRoot: "mealplan",
                        endpointName: "serviceEndpoint",
                        startup: Startup.Configuration,
                        serviceContext: context,
                        stateManager: StateManager),
                    name: $"{nameof(Service)} HTTP Endpoint",
                    listenOnSecondary: true)
            };
        }

        /// <summary>
        /// This is the main entry point for your service replica.
        /// This method executes when this replica of your service becomes primary and has write status.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {

            if (!cancellationToken.IsCancellationRequested)
            {
                var adaptor = new BuiltinHandlerActivator();
                adaptor.Register(() => new AddGroceryMessageHandler(new GroceryRepository(StateManager)));
                adaptor.Register(() => new AddMealMessageHandler(new MealRepository(StateManager)));

                ServiceEventSource.Current.ServiceMessage(this.Context, $"{nameof(Service)} Configure Rebus");
                var connectionString = configuration.GetConnectionString("AzureServiceBus");
                bus = ConfigureRebus(adaptor, connectionString, InputQueue);
                
                var subscriptionTasks = eventTypes.Select(eventType => bus.Subscribe(eventType));
                await Task.WhenAll(subscriptionTasks);
                Bootstrapper.GlobalContainer.RegisterInstance(typeof(IBus), bus);
            }
        }
        private ServiceFabricConfigurationSettings GetServiceConfiguration()
        {
            var configurationPackageObject = Context.CodePackageActivationContext.GetConfigurationPackageObject("Config");
            return new ServiceFabricConfigurationSettings(configurationPackageObject.Settings);
        }
        private IBus ConfigureRebus(IContainerAdapter builtinHandlerActivator, string connectionString, string inputQueueName)
        {
            return Configure.With(builtinHandlerActivator)
                    .Transport(t => t.UseAzureServiceBus(connectionString, inputQueueName))
                    .Options(o => o.SimpleRetryStrategy(ErrorQueue))
                    .Start();
        }
    }
    
}
