using System;
using System.Linq;
using System.Threading.Tasks;
using Ksj.Mealplan.Domain.Model;
using Ksj.Mealplan.Infrastructure;
using Ksj.Mealplan.Messages;
using Rebus.Handlers;

namespace Ksj.Mealplan.Service.Handlers
{
    public class AddMealMessageHandler : IHandleMessages<AddMealMessage>
    {
        private readonly IRepository<Meal> _repository;

        public AddMealMessageHandler(IRepository<Meal> repository)
        {
            _repository = repository;
        }
        public async Task Handle(AddMealMessage message)
        {
            var meal = new Meal()
            {
                Id = string.IsNullOrEmpty(message.Meal.Id) ? Guid.NewGuid() : new Guid(message.Meal.Id),
                Groceries = message.Meal.Lines.Select(g => new GroceryLine()
                {
                    Amount = g.Amount,
                    Unit = g.Unit,
                    Grocery = new Grocery() { 
                    AverageGramsPerUnit = g.Grocery.AverageGramsPerUnit,
                    KcalPer100g = g.Grocery.KcalPer100g,
                    UnitName = g.Grocery.UnitName,
                    Id = g.Grocery.Id,
                    Name = g.Grocery.Name,
                    PluralName = g.Grocery.PluralName
}
                }),
                Name = message.Meal.Name,
                Instructions = message.Meal.Instructions.Select(i => new Instruction()
                {
                    Description = i.Description,
                    Headline = i.Headline
                }),
                NumberOfPersons = message.Meal.NumberOfPersons,
                Category = message.Meal.Category?.Select(c => new Category()
                {
                    Name = c.Name,
                    Id = c.Id
                })

            };
            await _repository.Add(meal);
        }
    }
}
