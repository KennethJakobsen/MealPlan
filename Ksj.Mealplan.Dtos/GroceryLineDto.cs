using System.Runtime.Serialization;

namespace Ksj.Mealplan.Domain.Model
{
    [DataContract]
    public class GroceryLine
    {
        [DataMember]
        public int Amount { get; set; }
        [DataMember]
        public Grocery Grocery { get; set; }
        [DataMember]
        public string Unit { get; set; }
    }
}
