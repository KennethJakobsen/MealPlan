using Ksj.Mealplan.Domain.Model;
using Ksj.Mealplan.Infrastructure;
using LightInject;

namespace Ksj.Mealplan.Service.IoC
{
    public class Bootstrapper
    {
        public static void Bootstrap(IServiceContainer container)
        {
            container.Register<IRepository<Grocery>, GroceryRepository>();
            container.Register<IRepository<Meal>, MealRepository>();

            container.RegisterApiControllers();
            container.RegisterInstance(typeof(IServiceContainer), container);

        }
    }
}
