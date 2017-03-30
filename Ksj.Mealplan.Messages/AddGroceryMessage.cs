using Ksj.Mealplan.Domain.Model;
using Ksj.Mealplan.Dtos;

namespace Ksj.Mealplan.Service.Messages
{
    public class AddGroceryMessage
    {
        public GroceryDto Grocery { get; set; }
    }
}
