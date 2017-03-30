using Ksj.Mealplan.Domain.Model;
using Ksj.Mealplan.Infrastructure;
using Ksj.Mealplan.Service.Handlers;
using Ksj.Mealplan.Service.Messages;
using LightInject;
using Rebus.Handlers;

namespace Ksj.Mealplan.Service.IoC
{
    public class Bootstrapper
    {
        public static IServiceContainer GlobalContainer { get; private set; }
        public static void Bootstrap()
        {
            var container = new ServiceContainer();
            //container.Register<IHandleMessages<AddGroceryMessage>, AddGroceryMessageHandler>();

            container.Register<IRepository<Grocery>, GroceryRepository>();

            container.RegisterApiControllers();
            GlobalContainer = container;
            container.RegisterInstance(typeof(IServiceContainer), container);
           

        }
    }
}
