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
        private readonly TimeService _timeService;

        public TimeHub(TimeService timeService)
        {
            _timeService = timeService;
        }
        
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public async Task StartTime()
        {
            _timeService.PublishTime = true;
            while (_timeService.PublishTime)
            {
                await Clients.All.InvokeAsync("Change", DateTime.UtcNow);
                await Task.Delay(100);
            }

        }
        public void StopTime()
        {
            _timeService.PublishTime = false;

        }
    }
}
