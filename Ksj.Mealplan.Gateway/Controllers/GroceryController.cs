using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Ksj.Mealplan.Dtos;
using Ksj.Mealplan.Service.Messages;
using Rebus.Bus;

namespace Ksj.Mealplan.Gateway.Controllers
{

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("groceries")]
    public class GroceryController : BaseApiController
    {
        private readonly IBus _bus;

        public GroceryController(IBus bus) : base(new Uri("fabric:/Ksj.Mealplan/Service"), "groceries")
        {
            _bus = bus;
        }
        [Route("grocery")]
        [HttpGet]
        public async Task<IHttpActionResult> Get(CancellationToken cancelRequest)
        {
            return await SendAsync(this.Request, cancelRequest);
        }

        [Route("grocery")]
        [HttpPost]
        public async Task<IHttpActionResult> Create([FromBody]GroceryDto grocery)
        {
            await _bus.Publish(new AddGroceryMessage() {Grocery = grocery});
            return Ok();
        }

        [Route("grocery/search/{phrase}")]
        [HttpGet]
        public async Task<IHttpActionResult> Search(string phrase, CancellationToken cancelRequest)
        {
            return await SendAsync(this.Request, cancelRequest);
        }
    }
}
