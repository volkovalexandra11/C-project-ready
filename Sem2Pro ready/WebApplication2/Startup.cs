using System;
using DomainLayer.Drawing;
using FunctionGraph;
using Infrastructure.TopDowns;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserLayer.Controllers;
using UserLayer.Controllers.Auxiliary;
using WebApplication2.Controllers;
using ZedGraph;



namespace WebApplication2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddTransient<ZedGraphControl>();
            services.AddSingleton<IDrawer, GraphDrawer>();
            services.AddSingleton<IPointsGetter, PointsGetter>();
            services.AddSingleton<FunctionDrawer>();
            services.AddSingleton<EmailService>();
            services.AddSingleton<DrawingService>();
            services.AddSingleton<PointsDrawingService>();
            services.AddSingleton<Email>();
            services.AddSingleton<Drawer>();
            services.AddSingleton<DrawerForPoints>();
            services.AddSingleton<Cache>();
            services.AddDistributedMemoryCache();
            RegisterParsers(services);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            env.EnvironmentName = "Release";
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private static void RegisterParsers(IServiceCollection services)
        {
            services.AddSingleton<ParserCombinator, ParserCombinator>();
            services.AddSingleton(provider =>
                new Lazy<ParserCombinator>(provider.GetService<ParserCombinator>)
            );
            services.AddSingleton<IParser, AdditionParser>();
            services.AddSingleton<IParser, BracketParser>();
            services.AddSingleton<IParser, CallParser>();
            services.AddSingleton<IParser, ConstantParser>();
            services.AddSingleton<IParser, DivisionParser>();
            services.AddSingleton<IParser, MultiplicationParser>();
            services.AddSingleton<IParser, NumericParser>();
            services.AddSingleton<IParser, ParameterParser>();
            services.AddSingleton<IParser, PowerParser>();
            services.AddSingleton<IParser, SubtractionParser>();
            services.AddSingleton<IParser, WhereParser>();
            services.AddSingleton<IParser, UnarySignParser>();
        }
    }
}
