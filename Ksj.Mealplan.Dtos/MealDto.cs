using System;
using System.Collections.Generic;
using System.Security.AccessControl;

namespace Ksj.Mealplan.Dtos
{
    
    public class MealDto
    {
        
        public IEnumerable<GroceryLineDto> Lines { get; set; }
        
        public string Name { get; set; }
        
        public IEnumerable<InstructionDto> Instructions { get; set; }
        
        public int NumberOfPersons { get; set; }

        public IEnumerable<CategoryDto> Category { get; set; }
        public string Id { get; set; }
    }
}
