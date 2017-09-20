using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ksj.Mealplan.Notification.Hubs;
using Ksj.Mealplan.Notification.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Ksj.Mealplan.Notification.Controllers
{
    [Route("api/note")]
    public class NoteController : Controller
    {
        private readonly IHubContext<NoteHub> _context;

        public NoteController(IHubContext<NoteHub> context)
        {
            _context = context;
        }
        
        [HttpGet("id")]
        public IActionResult Get()
        {
            
            return Ok(Guid.NewGuid().ToString());
        }


        
    }
}
