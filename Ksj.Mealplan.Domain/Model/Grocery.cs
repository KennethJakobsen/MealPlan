using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Ksj.Mealplan.Domain.Model
{
    [DataContract]
    public class Grocery
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string PluralName { get; set; }
        [DataMember]
        public int KcalPer100g { get; set; }
        [DataMember]
        public int AverageGramsPerUnit { get; set; }
        [DataMember]
        public string UnitName { get; set; }
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public IEnumerable<Category> Category { get; set; }
    }
}
