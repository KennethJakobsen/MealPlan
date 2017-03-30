using System;
using System.Fabric;
using System.Fabric.Description;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Ksj.Mealplan.Service.Extensions;
using Microsoft.Owin.Hosting;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Owin;

namespace Ksj.Mealplan.Service
{
    public class OwinCommunicationListener : ICommunicationListener
    {
        private readonly Action<IAppBuilder> startup;
        private readonly ServiceContext serviceContext;
        private readonly string endpointName;
        private readonly IReliableStateManager stateManager;
        private readonly string appRoot;
        private IDisposable webApp;
        private string publishAddress;
        private string listeningAddress;

        public OwinCommunicationListener(Action<IAppBuilder> startup, ServiceContext serviceContext, string endpointName, string appRoot, IReliableStateManager stateManager)
        {
            if (startup == null)
                throw new ArgumentNullException("startup");
            if (serviceContext == null)
                throw new ArgumentNullException("serviceContext");
            if (endpointName == null)
                throw new ArgumentNullException("endpointName");
            this.startup = startup;
            this.serviceContext = serviceContext;
            this.endpointName = endpointName;
            this.stateManager = stateManager;
            this.appRoot = appRoot;
        }

        public Task<string> OpenAsync(CancellationToken cancellationToken)
        {
            this.publishAddress = this.CreatePublicAddress();
            try
            {
                this.webApp = WebApp.Start(this.listeningAddress, (Action<IAppBuilder>)(appBuilder =>
                {
                    if (this.stateManager != null)
                        appBuilder.SetReliableStateManager(this.stateManager);
                    ConfigurationPackage configurationPackageObject = this.serviceContext.CodePackageActivationContext.GetConfigurationPackageObject("Config");
                    this.startup(appBuilder);
                }));
                return Task.FromResult<string>(this.publishAddress);
            }
            catch (Exception ex)
            {
                this.StopWebServer();
                throw;
            }
        }

        private string CreatePublicAddress()
        {
            EndpointResourceDescription endpoint = this.serviceContext.CodePackageActivationContext.GetEndpoint(this.endpointName);
            int port = endpoint.Port;
            string lower = endpoint.Protocol.ToString().ToLower();
            if (this.serviceContext is StatefulServiceContext)
            {
                StatefulServiceContext serviceContext = this.serviceContext as StatefulServiceContext;
                string str1;
                if (!string.IsNullOrWhiteSpace(this.appRoot))
                    str1 = this.appRoot.TrimEnd('/') + "/";
                else
                    str1 = string.Empty;
                string str2 = str1;
                this.listeningAddress = string.Format((IFormatProvider)CultureInfo.InvariantCulture, "{0}://+:{1}/{2}{3}/{4}/{5}", (object)lower, (object)port, (object)str2, (object)serviceContext.PartitionId, (object)serviceContext.ReplicaId, (object)Guid.NewGuid());
            }
            else
            {
                if (!(this.serviceContext is StatelessServiceContext))
                    throw new InvalidOperationException(string.Format("'{0}' could not be handled", (object)typeof(ServiceContext).Name));
                this.listeningAddress = string.Format((IFormatProvider)CultureInfo.InvariantCulture, "{0}://+:{1}/{2}", (object)lower, (object)port, (object)this.appRoot);
            }
            return this.listeningAddress.Replace("+", FabricRuntime.GetNodeContext().IPAddressOrFQDN);
        }

        public Task CloseAsync(CancellationToken cancellationToken)
        {
            this.StopWebServer();
            return (Task)Task.FromResult<bool>(true);
        }

        public void Abort()
        {
            this.StopWebServer();
        }

        private void StopWebServer()
        {
            if (this.webApp == null)
                return;
            try
            {
                this.webApp.Dispose();
            }
            catch (ObjectDisposedException ex)
            {
            }
        }
    }
}
