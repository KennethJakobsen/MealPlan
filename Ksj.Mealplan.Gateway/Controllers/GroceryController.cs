using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Ksj.Mealplan.Dtos;
using Ksj.Mealplan.Service.Messages;
using Ksj.Service.Client;
using Rebus.Bus;
using ServiceFabric.AutoRest.Communication.Client;

namespace Ksj.Mealplan.Gateway.Controllers
{

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("groceries")]
    public class GroceryController : ApiController
    {
        private readonly IBus _bus;
        private readonly IRestServicePartitionClient<Mealservice> _servicePartitionClient;

        public GroceryController(IBus bus, IRestServicePartitionClient<Mealservice> servicePartitionClient)
        {
            _bus = bus;
            _servicePartitionClient = servicePartitionClient;
        }
        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> Get(CancellationToken cancelRequest)
        {
            var response = await _servicePartitionClient.InvokeWithRetryAsync(
                client => client.RestApi.Grocery.GetAllAsync(cancellationToken: cancelRequest), cancelRequest);
            if (response.Any())
                return Ok(response);
            return NotFound();
        }

        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> Create([FromBody]GroceryDto grocery)
        {
            await _bus.Publish(new AddGroceryMessage() {Grocery = grocery});
            return Ok();
        }

        [Route("search/{phrase}")]
        [HttpGet]
        public async Task<IHttpActionResult> Search(string phrase, CancellationToken cancelRequest)
        {
            var response = await _servicePartitionClient.InvokeWithRetryAsync(
                client => client.RestApi.Grocery.SearchAsync(phrase, cancellationToken: cancelRequest), cancelRequest);
            if (response.Any())
                return Ok(response);
            return NotFound();
        }
    }
}
