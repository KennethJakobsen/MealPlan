using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ksj.Mealplan.Notification.Services
{
    
    public class TimeService
    {
        private static TimeService _instance;

        private TimeService()
        {
        }

        public static TimeService Instance => _instance ?? (_instance = new TimeService());
        public bool PublishTime  { get; set; }  
    }
}
