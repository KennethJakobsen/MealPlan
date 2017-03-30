using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Ksj.Mealplan.Gateway.Helpers;

namespace Ksj.Mealplan.Gateway.Controllers
{
    [RoutePrefix("api")]
    public abstract class BaseApiController : ApiController
    {
        private readonly string routePrefix;
        private readonly ServiceUriHelper serviceUriHelper;
        private readonly HttpClient httpClient = new HttpClient();

        protected BaseApiController(Uri serviceUri, string routePrefix)
        {
            this.routePrefix = routePrefix;
            this.serviceUriHelper = new ServiceUriHelper(serviceUri);
        }

        protected async Task<IHttpActionResult> SendAsync(HttpRequestMessage incomingRequestMessage, CancellationToken cancellationToken)
        {
            if (incomingRequestMessage == null) throw new ArgumentNullException(nameof(incomingRequestMessage));

            var serviceUri = await serviceUriHelper.GetInternalServiceUriAsync(long.MinValue, cancellationToken);

            var actionUri = incomingRequestMessage.RequestUri.ExtractAction($"{routePrefix}");
            var fullServiceUri = serviceUri.AppendAction(actionUri); 
            
            var outgoingRequestMessage = new HttpRequestMessage(incomingRequestMessage.Method, fullServiceUri) { Content = incomingRequestMessage.Method == HttpMethod.Get ? null : incomingRequestMessage.Content };

            // TODO: implement retry logic if resolved servicepartion has the wrong url, by passing in the partition and getting a new result and back out if failing more than 3 times.
            var response = await httpClient.SendAsync(outgoingRequestMessage, cancellationToken);
            return this.ResponseMessage(response);
        }
    }
}