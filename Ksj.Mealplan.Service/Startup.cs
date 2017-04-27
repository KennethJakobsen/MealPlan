using System;
using System.ComponentModel;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.UI;
using Ksj.Mealplan.Service.Extensions;
using Ksj.Mealplan.Service.IoC;
using LightInject;
using Microsoft.ServiceFabric.Data;
using Swashbuckle.Application;
using Owin;

namespace Ksj.Mealplan.Service
{
    public class Startup
    {
        public static void Configuration(IAppBuilder appBuilder)
        {
            var stateManager = appBuilder.GetReliableStateManager();
            var container = new ServiceContainer();
            Bootstrapper.Bootstrap(container);
            container.RegisterInstance(typeof(IReliableStateManager), stateManager);
            

          

            var httpConfiguration = CreateHttpConfiguration("mealplan");
            httpConfiguration.EnsureInitialized();
            container.EnableWebApi(httpConfiguration);
            appBuilder.UseWebApi(httpConfiguration);
            
        }
        private static HttpConfiguration CreateHttpConfiguration(string appRoot)
        {
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            //config.Routes.MapHttpRoute(
            //    name: "meals",
            //    routeTemplate: "{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
            config.EnableSwagger(c =>
            {
                c.RootUrl(req => $"{req.RequestUri.Scheme}://{req.RequestUri.Authority}/{appRoot}");
                c.SingleApiVersion("v1", "Meal service");
            });



            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter());

            return config;
        }

    }
}