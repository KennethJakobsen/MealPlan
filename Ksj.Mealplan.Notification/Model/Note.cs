using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Ksj.Mealplan.Notification.Model
{
    [Serializable]
    public class Note
    {
        
        public int Top { get; set; }
        public int Left { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Id { get; set; }
    }
}
