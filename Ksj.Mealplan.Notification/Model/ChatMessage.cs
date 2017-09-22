using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ksj.Mealplan.Notification.Model
{
    public class ChatMessage
    {
        public string Name { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
