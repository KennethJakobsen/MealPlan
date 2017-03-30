using System.Threading.Tasks;
using System.Web.Http;
using Ksj.Mealplan.Domain.Model;
using Ksj.Mealplan.Infrastructure;

namespace Ksj.Mealplan.Service.Controllers
{
    public class MealController : ApiController
    {
        private readonly IRepository<Meal> _repository;

        public MealController(IRepository<Meal> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetMeals()
        {
            var meals = await _repository.GetAll();
            return Ok(meals);
        }

        [Route("search/{phrase}")]
        [HttpGet]
        public async Task<IHttpActionResult> FindMeals(string phrase)
        {
            var meals = await _repository.Search(phrase);
            return Ok(meals);
        }
    }
}
