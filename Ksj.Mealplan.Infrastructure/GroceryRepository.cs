using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ksj.Mealplan.Domain.Model;
using Microsoft.ServiceFabric.Data;

namespace Ksj.Mealplan.Infrastructure
{
    public class GroceryRepository : BaseRepository<Grocery>, IRepository<Grocery>
    {
        public GroceryRepository(IReliableStateManager stateManager) : base(stateManager, "grocery-repository")
        {
            
        }
        public async Task Add(Grocery entity)
        {
            await SaveAsync(entity, entity.Id);
        }

        public async Task<IEnumerable<Grocery>> GetAll()
        {
            var all = await GetAllAsync();
            return all;
        }

        public Task<Grocery> Find(string name)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<Grocery>> Search(string name)
        {
            var phrase = name.ToLower();
            var all = await GetAllAsync();
            return all.Where(g => g.Name.ToLower().Contains(phrase) || g.PluralName.ToLower().Contains(phrase));
        }
    }
}
