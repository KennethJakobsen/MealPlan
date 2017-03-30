using System;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Client;
using Newtonsoft.Json.Linq;

namespace Ksj.Mealplan.Gateway.Helpers
{
    public class ServiceUriHelper
    {
        private readonly ServicePartitionResolver servicePartitionResolver = ServicePartitionResolver.GetDefault();
        private readonly Uri serviceUri;

        public ServiceUriHelper(Uri serviceUri)
        {
            if (serviceUri == null) throw new ArgumentNullException(nameof(serviceUri));
            this.serviceUri = serviceUri;
        }

        public async Task<Uri> GetInternalServiceUriAsync(long partitionKey, CancellationToken cancellationToken)
        {
            var servicePartition = await GetServicePartitionAsync(partitionKey, cancellationToken);

            var endpoint = servicePartition.GetEndpoint();
            var addresses = JObject.Parse(endpoint.Address);
            string primaryReplicaAddress = (string)addresses["Endpoints"].First();
            return new Uri(primaryReplicaAddress);
        }

        private async Task<ResolvedServicePartition> GetServicePartitionAsync(long partitionKey,
            CancellationToken cancellationToken)
        {
            try
            {

                var int64PartitionKey = new ServicePartitionKey(partitionKey);
                return await this.servicePartitionResolver.ResolveAsync(this.serviceUri, int64PartitionKey, cancellationToken);
            }
            catch (FabricException) { } // Catch if we tried to resolve a stateful service, but the requested service is singleton

            var singletonPartitionKey = ServicePartitionKey.Singleton;
            return await this.servicePartitionResolver.ResolveAsync(this.serviceUri, singletonPartitionKey, cancellationToken);
        }
    }
}