using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ksj.Mealplan.Notification.Model;
using Microsoft.AspNetCore.SignalR;

namespace Ksj.Mealplan.Notification.Hubs
{
    public class NoteHub : Hub
    {
        public async Task Create(Note note)
        {
            await Clients.AllExcept(new[] { Context.ConnectionId }).InvokeAsync("createNote", note);
        }
        public async Task Update(Note note)
        {
            await Clients.AllExcept(new []{Context.ConnectionId}).InvokeAsync("updateNote", note);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);

        }

        public async Task Remove(Note note)
        {
            await Clients.AllExcept(new[] { Context.ConnectionId }).InvokeAsync("removeNote", note);
        }
    }
}