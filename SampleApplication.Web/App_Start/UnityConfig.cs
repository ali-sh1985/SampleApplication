using System;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Practices.Unity;
using SampleApplication.Data.EntityFramework;
using SampleApplication.Domain;
using SampleApplication.Web.Identity;
using Unity.Mvc5;

namespace SampleApplication.Web
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            container.RegisterType<IUnitOfWork, UnitOfWork>(new HierarchicalLifetimeManager(), new InjectionConstructor("SampleApplication"));
            container.RegisterType<IUserStore<IdentityUser, Guid>, UserStore>(new TransientLifetimeManager());
            container.RegisterType<RoleStore>(new TransientLifetimeManager());

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}