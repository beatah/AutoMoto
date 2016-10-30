﻿using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using AutoMoto.Contracts.Interfaces;
using AutoMoto.Model.Models;
using AutoMoto.Service;
using Repository.Pattern.DataContext;
using Repository.Pattern.Ef6;
using Repository.Pattern.Repositories;
using Repository.Pattern.UnitOfWork;
using System.Reflection;
using System.Web.Mvc;

namespace AutoMoto.Web.App_Start
{
    public class AutofacConfig
    {
        public static void Register()
        {
            var builder = new ContainerBuilder();
            //var config = System.Web.Http.GlobalConfiguration.Configuration;
            // Register your MVC controllers.
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // OPTIONAL: Register model binders that require DI.
            builder.RegisterModelBinders(Assembly.GetExecutingAssembly());
            builder.RegisterModelBinderProvider();

            // OPTIONAL: Register web abstractions like HttpContextBase.
            builder.RegisterModule<AutofacWebTypesModule>();

            // OPTIONAL: Enable property injection in view pages.
            builder.RegisterSource(new ViewRegistrationSource());

            // OPTIONAL: Enable property injection into action filters.
            builder.RegisterFilterProvider();


            builder.RegisterType<PizzaDbContext>().As<IDataContextAsync>().InstancePerRequest();
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepositoryAsync<>)).InstancePerRequest();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWorkAsync>().InstancePerRequest();

            //Services
            builder.RegisterType<ManufacturerService>().As<IManufacturerService>().InstancePerRequest();


            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());


            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            //WebApi
            //config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}