using GC_PlanMyMeal.Areas.Identity.Data;
using GC_PlanMyMeal.Configuration;
using GC_PlanMyMeal.PreferencesService;
using GC_PlanMyMeal.RecipeService;
using GC_PlanMyMeal.RecipeService.Models;
using GC_PlanMyMeal.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GC_PlanMyMeal
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
            services.AddControllersWithViews();
            services.AddRazorPages();
            var apiKey = Configuration["apiKey"];
            var spoonacularConfig = new SpoonacularConfiguration()
            {
                ApiKey = apiKey
            };
            services.AddSingleton(spoonacularConfig);
            services.AddSingleton<ConcurrentDictionary<int, Recipe>>();
            services.AddHttpClient<ISearchRecipe, RecipeClientWithCache>(client =>
            {
                client.BaseAddress = new Uri("https://api.spoonacular.com/");
            });

            services.AddDbContext<GC_PlanMyMealIdentityDbContext>(options =>
                options.UseSqlServer(Configuration["ConnectionStrings:PlanMyMealDb"]));

            services.AddScoped<IRepositoryClient, RepositoryClient>();
            services.AddScoped<IPreferencesClient, PreferencesClient>();

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<GC_PlanMyMealIdentityDbContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
