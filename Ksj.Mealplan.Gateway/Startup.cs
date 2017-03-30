using System;
using System.ComponentModel.Design;
using System.Runtime.Remoting.Contexts;
using System.Web.Http;
using Ksj.Mealplan.Service;
using Ksj.Mealplan.Service.Messages;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Services.Runtime;
using Owin;
using Rebus.Activation;
using Rebus.Config;
using Rebus.Retry.Simple;
using Rebus.Routing.TypeBased;

namespace Ksj.Mealplan.Gateway
{
    public static class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public static void ConfigureApp(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();
            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            config.EnableCors();
            appBuilder.UseWebApi(config);
        }

    }
}
