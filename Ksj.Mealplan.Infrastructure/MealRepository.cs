using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ksj.Mealplan.Domain.Model;
using Microsoft.ServiceFabric.Data;

namespace Ksj.Mealplan.Infrastructure
{
    public class MealRepository : BaseRepository<Meal>, IRepository<Meal>
    {
        public MealRepository(IReliableStateManager stateManager) : base(stateManager, "meal-repository")
        {
        }

        public async Task Add(Meal entity)
        {
            await SaveAsync(entity, entity.Id.ToString());
        }

        public async Task<IEnumerable<Meal>> GetAll()
        {
            return await GetAllAsync();
        }

        public Task<Meal> Find(string name)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<Meal>> Search(string name)
        {
            var phrase = name.ToLower();
            var all = await GetAllAsync();
            return all.Where(g => g.Name.ToLower().Contains(phrase));
        }
    }
}
