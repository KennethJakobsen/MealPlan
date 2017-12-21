using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Ksj.Mealplan.Dtos;
using Ksj.Mealplan.Messages;
using Rebus.Bus;

namespace Ksj.Mealplan.Gateway.Controllers
{

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("meals")]
    public class MealController : BaseApiController
    {
        private readonly IBus _bus;

        public MealController(IBus bus) : base(new Uri("fabric:/Ksj.Mealplan/Service"), "meals")
        {
            _bus = bus;
        }
        [Route("meal")]
        [HttpGet]
        public async Task<IHttpActionResult> Get(CancellationToken cancelRequest)
        {
            return await SendAsync(this.Request, cancelRequest);
        }

        [Route("meal")]
        [HttpPost]
        public async Task<IHttpActionResult> Create([FromBody]MealDto meal)
        {
            await _bus.Publish(new AddMealMessage() { Meal = meal});
            return Ok();
        }

        [Route("meal/search/{phrase}")]
        [HttpGet]
        public async Task<IHttpActionResult> Search(string phrase, CancellationToken cancelRequest)
        {
            return await SendAsync(this.Request, cancelRequest);
        }
    }
}
