using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using SampleCode.Interface;
using SampleCode.Models;
using System.Data.Entity;
using System.Reflection;
using System.Web.Http.Controllers;
using System.Web.Mvc;

namespace SampleCode.IoC
{
    public class ControllersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // Register DbContext
            container.Register(Classes.FromThisAssembly()
                .BasedOn<DbContext>().WithService.FromInterface()
                .LifestylePerWebRequest());

            // Register Repository
            container.Register(Classes.FromThisAssembly()
                .BasedOn<IRepositoryBase>().WithService.FromInterface()
                .LifestylePerWebRequest());

            // Register Manager
            container.Register(Classes.FromThisAssembly()
                .BasedOn<IManager>().WithService.FromInterface()
                .LifestylePerWebRequest());

            // Load Elmah.Mvc
            Assembly elmah = Assembly.Load("Elmah.Mvc");
            // Register Elmah Controller
            container.Register(Classes.FromAssembly(elmah)
                .BasedOn<IController>()
                .LifestylePerWebRequest());

            // Register Controller
            container.Register(Classes.FromThisAssembly()
                .BasedOn<IController>()
                .LifestylePerWebRequest());

            // Register Web API Controller
            container.Register(Classes.FromThisAssembly()
                .BasedOn<IHttpController>()
                .LifestylePerWebRequest());
        }
    }
}