using System.Threading.Tasks;

namespace Ksj.Mealplan.Notification.Hubs
{
    public interface ITimeHub
    {
        Task OnConnectedAsync();
        void StartTime();
    }
}