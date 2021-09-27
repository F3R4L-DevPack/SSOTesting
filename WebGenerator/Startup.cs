using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using F3R4L.DevPack.Api.DependencyInjection;
using F3R4L.DevPack.SSO.DependencyInjection;
using F3R4L.DevPack.Eve.SSO;
using WebGenerator.Models;

namespace WebGenerator
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
            services.AddF3R4LApiBindings();
            services.AddF3R4LSSOBindings();

            services.AddScoped(typeof(IEveOnlineSSOService), typeof(EveOnlineSSOService));
            services.AddSingleton<SSOSettings>(GetSSOSettings());
        }

        private SSOSettings GetSSOSettings()
        {
            var ssoSettings = new SSOSettings();

            Configuration.GetSection("SSOSettings").Bind(ssoSettings);

            return ssoSettings;
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
