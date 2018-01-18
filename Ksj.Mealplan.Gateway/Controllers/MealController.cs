using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using Ksj.Mealplan.Dtos;
using Ksj.Mealplan.Messages;
using Ksj.Service.Client;
using Rebus.Bus;
using ServiceFabric.AutoRest.Communication.Client;

namespace Ksj.Mealplan.Gateway.Controllers
{

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("meals")]
    public class MealController : ApiController
    {
        private readonly IRestServicePartitionClient<Mealservice> _servicePartitionClient;
        private readonly IBus _bus;

        public MealController(IRestServicePartitionClient<Mealservice> servicePartitionClient, IBus bus)
        {
            _servicePartitionClient = servicePartitionClient;
            _bus = bus;
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> Get(CancellationToken cancelRequest)
        {
            var response = await _servicePartitionClient.InvokeWithRetryAsync(client =>
                client.RestApi.Meal.GetMealsAsync(cancellationToken: cancelRequest), cancelRequest);
            if (response.Any())
                return Ok(response);
            return NotFound();
        }

        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> Create([FromBody] MealDto meal)
        {
            await _bus.Publish(new AddMealMessage() {Meal = meal});
            return Ok();
        }

        [Route("search/{phrase}")]
        [HttpGet]
        public async Task<IHttpActionResult> Search(string phrase, CancellationToken cancelRequest)
        {
            var response = await _servicePartitionClient.InvokeWithRetryAsync(client =>
                client.RestApi.Meal.FindMealsAsync(phrase, cancellationToken: cancelRequest), cancelRequest);
            if (response.Any())
                return Ok(response);
            return NotFound();
        }
    }
}
