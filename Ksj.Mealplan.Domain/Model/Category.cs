using System;
using System.Runtime.Serialization;

namespace Ksj.Mealplan.Domain.Model
{
    [DataContract]
    public class Category
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public Guid Id { get; set; }

    }
}
