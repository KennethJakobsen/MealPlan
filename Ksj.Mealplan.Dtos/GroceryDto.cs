using System.Collections.Generic;

namespace Ksj.Mealplan.Dtos
{
    public class GroceryDto
    {
        public string Name { get; set; }
        public string PluralName { get; set; }
        public int KcalPer100g { get; set; }
        public int AverageGramsPerUnit { get; set; }
        public string UnitName { get; set; }
        public string Id { get; set; }
        public IEnumerable<CategoryDto> Category { get; set; }
    }
}
