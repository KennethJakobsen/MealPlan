using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ksj.Mealplan.Notification.Services;
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
            TimeService.Instance.PublishTime = true;
            while (TimeService.Instance.PublishTime)
            {
                base.Clients.All.InvokeAsync("Change", DateTime.UtcNow);
                Thread.Sleep(100);
            }

        }
        public void StopTime()
        {
            TimeService.Instance.PublishTime = false;

        }
    }
}
