using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Bsuir.Core.Models.Context;
using Bsuir.Core.Services;
using Bsuir.Web.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bsuir.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            #region Ioc
            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.RegisterType<ClientService>().SingleInstance();
            builder.RegisterType<ProductService>().SingleInstance();

            builder.RegisterInstance<Func<BsuirDbContext>>(
                () => new BsuirDbContext(Configuration.GetConnectionString("DefaultConnection")));

            var container = builder.Build();

            return new AutofacServiceProvider(container);
            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: null,
                    template: "",
                    defaults: new
                    {
                        controller = "Home",
                        action = nameof(HomeController.Index)
                    });

                routes.MapRoute(
                    name: null,
                    template: "add-client/",
                    defaults: new
                    {
                        controller = "Home",
                        action = nameof(HomeController.AddClient)
                    });

                routes.MapRoute(
                    name: null,
                    template: "add-product/",
                    defaults: new
                    {
                        controller = "Home",
                        action = nameof(HomeController.AddProduct)
                    });

                routes.MapRoute(
                    name: null,
                    template: "buy/",
                    defaults: new
                    {
                        controller = "Home",
                        action = nameof(HomeController.Buy)
                    });

                routes.MapRoute(
                    name: null,
                    template: "get-discount/",
                    defaults: new
                    {
                        controller = "Home",
                        action = nameof(HomeController.GetDiscount)
                    });
            });
        }
    }
}
