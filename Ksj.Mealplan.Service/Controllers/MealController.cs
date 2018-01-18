using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Ksj.Mealplan.Domain.Model;
using Ksj.Mealplan.Infrastructure;

namespace Ksj.Mealplan.Service.Controllers
{
    [RoutePrefix("meals")]
    public class MealController : ApiController
    {
        private readonly IRepository<Meal> _repository;

        public MealController(IRepository<Meal> repository)
        {
            _repository = repository;
        }
        [Route("")]
        [HttpGet]
        public async Task<IEnumerable<Meal>> GetMeals()
        {
            var meals = await _repository.GetAll();
            return meals;
        }

        [Route("search/{phrase}")]
        [HttpGet]
        public async Task<IEnumerable<Meal>> FindMeals(string phrase)
        {
            var meals = await _repository.Search(phrase);
            return meals;
        }
    }
}
