using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Ksj.Mealplan.Domain.Model;
using Ksj.Mealplan.Infrastructure;

namespace Ksj.Mealplan.Service.Controllers
{
    [RoutePrefix("groceries")]
    public class GroceryController : ApiController
    {
        private readonly IRepository<Grocery> _repo;

        public GroceryController( IRepository<Grocery> repo)
        {
            _repo = repo;
        }
        [Route("")]
        [HttpGet]
        public async Task<IEnumerable<Grocery>> GetAll()
        {
            var groceries = await _repo.GetAll();
            return groceries;
        }

        [Route("search/{phrase}")]
        [HttpGet]
        public async Task<IEnumerable<Grocery>> Search(string phrase)
        {
            var groceries = await _repo.Search(phrase);
            return groceries;
        }
        
    }
}
