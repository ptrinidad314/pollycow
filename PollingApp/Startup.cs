using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using PollingApp.Entities;
using PollingApp.Security;
using PollingApp.Services;

namespace PollingApp
{
    public class Startup
    {
        private IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var connectionString = _configuration["ConnectionStrings:DefaultConnectionLocal"];

            services.AddDbContext<PollDataContext>(o=>o.UseSqlServer(connectionString));
            services.AddScoped<IPollDataRepository, PollDataRepository>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddAuthentication(options =>
            {
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddOpenIdConnect(options =>
            {
                options.Authority = _configuration["AzureAD:Authority"]; //"https://login.microsoftonline.com/trinidadplive.onmicrosoft.com";
                options.ClientId = _configuration["AzureAD:ClientId"]; //"1a07a460-363d-47e1-bfee-386ba8101996";
                options.ResponseType = OpenIdConnectResponseType.IdToken;
                options.CallbackPath = "/auth/signin-callback";
                options.SignedOutRedirectUri = _configuration["AzureAD:SignedOutRedirectUriLocal"];
            })
            .AddCookie();
            
            services.AddAuthorization(options =>
            {
               
                options.AddPolicy("RequireElevatedRights", policy =>
                {
                    policy.AddRequirements(new RequireElevatedRights(_configuration["AzureAD:AdminObjectId"], new HttpContextAccessor()));
                    
                });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
           
            app.UseDeveloperExceptionPage();

            app.UseRewriter(new RewriteOptions().AddRedirectToHttpsPermanent());

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(ConfigureRoutes);

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }

        private void ConfigureRoutes(IRouteBuilder routeBuilder)
        {
            routeBuilder.MapRoute("Default", "{controller=Home}/{action=Index}/{id?}");
        }

    }
}
