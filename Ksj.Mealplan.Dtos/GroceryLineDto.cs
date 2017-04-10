namespace Ksj.Mealplan.Dtos
{
    public class GroceryLineDto
    {
        public int Amount { get; set; }
        public GroceryDto Grocery { get; set; }
        public string Unit { get; set; }
    }
}
