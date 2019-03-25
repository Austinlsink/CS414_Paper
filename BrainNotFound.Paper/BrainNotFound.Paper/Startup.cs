using BrainNotFound.Paper.Models.BusinessModels;
using BrainNotFound.Paper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BrainNotFound.Paper
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

            services.AddDbContext<PaperDbContext>(options =>
            {

            //options.UseSqlServer(Configuration.GetConnectionString("PaperBrainTestKara"),             // VisualStudios MSSql Server For Kara to BLOW UP!
<<<<<<< HEAD
            //options.UseSqlServer(Configuration.GetConnectionString("PaperBrainTest"),                      // PCC Server
            options.UseSqlServer(Configuration.GetConnectionString("PaperBrainTestBima"),                // Bima Pc Server
=======
            options.UseSqlServer(Configuration.GetConnectionString("PaperBrainTest"),                      // PCC Server
            //options.UseSqlServer(Configuration.GetConnectionString("PaperBrainTestBima"),                // Bima Pc Server
>>>>>>> e90dcea6edc28f8cfe14632bea950cd85bafb3a9
            optionsBuilders => optionsBuilders.MigrationsAssembly("BrainNotFound.Paper"));
            });

            // This adds a referance to our actuall database
            services.AddIdentity<ApplicationUser, IdentityRole>()
               .AddRoles<IdentityRole>()
               .AddEntityFrameworkStores<PaperDbContext>()
               .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Default Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddMvc().AddJsonOptions(options => {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Data}/{action=Index}/{id?}");
            });

            app.UseCookiePolicy();

        }
    }
}
