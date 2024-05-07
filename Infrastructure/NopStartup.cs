using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Infrastructure;
using SIDS.Plugin.Misc.BetterBreadCrumb.Factories;

namespace SIDS.Plugin.Misc.BetterBreadCrumb.Infrastructure
{
    public class NopStartup : INopStartup
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new ViewLocationExpander());
            });

            //register services and interfaces
            services.Configure<MvcOptions>(options => options.Filters.Add<BetterBreadCrumbActionFilter>());
            services.AddScoped<Nop.Web.Factories.IProductModelFactory, BetterProductModelFactory>();
            //services.AddScoped<CustomModelFactory, ICustomerModelFactory>();
        }

        public void Configure(IApplicationBuilder application)
        {
        }

        public int Order => 666666;
    }
}