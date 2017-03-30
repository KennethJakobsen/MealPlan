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
                Groceries = message.Meal.Groceries.Select(g => new Grocery()
                {
                    AverageGramsPerUnit = g.AverageGramsPerUnit,
                    KcalPer100g = g.KcalPer100g,
                    UnitName = g.UnitName,
                    Id = g.Id,
                    Name = g.Name,
                    PluralName = g.PluralName
                }),
                Name = message.Meal.Name,
                Instructions = message.Meal.Instructions.Select(i => new Instruction()
                {
                    Description = i.Description,
                    Headline = i.Headline
                }),
                NumberOfPersons = message.Meal.NumberOfPersons,
                Category = message.Meal.Category.Select(c => new Category()
                {
                    Name = c.Name,
                    Id = c.Id
                })

            };
            await _repository.Add(meal);
        }
    }
}
