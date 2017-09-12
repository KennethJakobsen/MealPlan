using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Ksj.Mealplan.Gateway.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("times")]
    public class TimeController : BaseApiController
    {
        public TimeController() : base(new Uri("fabric:/Ksj.Mealplan/Notification"), "times/")
        {
        }
        [Route("api/time")]
        [HttpGet]
        public async Task<IHttpActionResult> Get(CancellationToken cancelRequest)
        {
            return await SendAsync(this.Request, cancelRequest);
        }
    }
}
