using System.Threading.Tasks;
using Ksj.Mealplan.Domain.Model;
using Ksj.Mealplan.Infrastructure;
using Ksj.Mealplan.Service.Messages;
using Rebus.Handlers;

namespace Ksj.Mealplan.Service.Handlers
{
    public class AddGroceryMessageHandler : IHandleMessages<AddGroceryMessage>
    {
        private readonly IRepository<Grocery> _repo;

        public AddGroceryMessageHandler(IRepository<Grocery> repo)
        {
            _repo = repo;
        }
        public async Task Handle(AddGroceryMessage message)
        {
            await _repo.Add(new Grocery()
            {
                AverageGramsPerUnit = message.Grocery.AverageGramsPerUnit,
                Id = message.Grocery.Id,
                KcalPer100g = message.Grocery.KcalPer100g,
                Name = message.Grocery.Name,
                PluralName = message.Grocery.PluralName,
                UnitName = message.Grocery.UnitName
            });
        }
    }
}
