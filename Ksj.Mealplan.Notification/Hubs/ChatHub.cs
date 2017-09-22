using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ksj.Mealplan.Notification.Model;
using Microsoft.AspNetCore.SignalR;

namespace Ksj.Mealplan.Notification.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(ChatMessage msg)
        {
            await Clients.All.InvokeAsync("messageReceived", msg);
        }
    }
}
