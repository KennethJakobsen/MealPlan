using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ksj.Mealplan.Notification.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Sockets;
using Microsoft.AspNetCore.Sockets.Client;
using Microsoft.Extensions.DependencyInjection;

namespace Ksj.Mealplan.Notification.Controllers
{
    [Route("api/time")]
    public class TimeController : Controller
    {
        private readonly IHubContext<TimeHub> _context;
        private readonly IServiceProvider _serviceProvider;
        private readonly ConnectionManager _manager;

        public TimeController(IHubContext<TimeHub> context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }
        // GET api/values
        [HttpGet]
        public async  Task<IActionResult> Get()
        {
            await _context.Clients.All.InvokeAsync("Change", DateTime.UtcNow.ToString());
           // hub.StartTime();
            return Ok("Time publisher started");
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
