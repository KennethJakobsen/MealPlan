using System.Runtime.Serialization;

namespace Ksj.Mealplan.Domain.Model
{
    [DataContract]
    public class Instruction
    {
        [DataMember]
        public string Headline { get; set; }
        [DataMember]
        public string Description { get; set; }
    }
}
