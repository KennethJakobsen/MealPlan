using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Ksj.Mealplan.Notification.Hubs
{
    public class TimeHub : Hub
    {
        public TimeHub()
        {

        }
        
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public void StartTime()
        {
            while (true)
            {
                base.Clients.All.InvokeAsync("Change", DateTime.UtcNow);
                Thread.Sleep(100);
            }

        }
    }
}
