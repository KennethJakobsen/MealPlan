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
    [RoutePrefix("notes")]
    public class NoteController : BaseApiController
    {
        public NoteController() : base(new Uri("fabric:/Ksj.Mealplan/Notification"), "notes/")
        {
        }

        [Route("api/note/id")]
        [HttpGet]
        public async Task<IHttpActionResult> Get(CancellationToken cancelRequest)
        {
            return await SendAsync(this.Request, cancelRequest);
        }
    }
}
