using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Ksj.Mealplan.Dtos;
using Ksj.Mealplan.Service.Messages;

namespace Ksj.Mealplan.Gateway.Controllers
{

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("groceries")]
    public class GroceryController : BaseApiController
    {
        public GroceryController() : base(new Uri("fabric:/Ksj.Mealplan/Service"), "groceries")
        {
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
            await Gateway.Bus.Publish(new AddGroceryMessage() {Grocery = grocery});
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
