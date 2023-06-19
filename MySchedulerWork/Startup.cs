using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MySchedulerWork.Data;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.AspNetCore.Http;
using MySchedulerWork.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace MySchedulerWork
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
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(AuthorizationService.LIFETIME);
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthorizationService.ISSUER,

                        ValidateAudience = true,
                        ValidAudience = AuthorizationService.AUDIENCE,

                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(AuthorizationService.LIFETIME),

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = AuthorizationService.GetSymmetricSecurityKey()
                    };
                });

            services.AddDbContext<MyAppContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            
            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddScoped<IAuthorizationService, AuthorizationService>();
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

            app.UseSession();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.Use(async (context, next) =>
            {
                var strToken = context.Session.GetString("JwToken");

                if (!string.IsNullOrEmpty(strToken))
                {
                    context.Request.Headers.Add("Authorization", $"bearer {strToken}");
                }

                await next.Invoke();
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=HelloPage}/{id?}");
            });
        }
    }
}
