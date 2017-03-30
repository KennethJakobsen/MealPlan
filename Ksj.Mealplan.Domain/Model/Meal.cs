using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.AccessControl;

namespace Ksj.Mealplan.Domain.Model
{
    [DataContract]
    public class Meal
    {
        [DataMember]
        public IEnumerable<Grocery> Groceries { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public IEnumerable<Instruction> Instructions { get; set; }
        [DataMember]
        public int NumberOfPersons { get; set; }
        [DataMember]
        public IEnumerable<Category> Category { get; set; }
        [DataMember]
        public Guid Id { get; set; }

    }
}
