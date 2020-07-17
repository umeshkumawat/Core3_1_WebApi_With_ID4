using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MVCClient__Authorization_Code_Flow_
{
    public class Startup_MVCClient
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(config => 
            {
                config.DefaultScheme = "cookie";
                config.DefaultChallengeScheme = "oidc";
            })
                .AddCookie("cookie")
                .AddOpenIdConnect("oidc", config => 
                {
                    config.Authority = "https://localhost:44323/";
                    config.ClientId = "client2_id";
                    config.ClientSecret = "client2_secret";
                    config.SaveTokens = true;
                    config.ResponseType = "code"; // the "code", is defied in oids specifications. We must provide response type = "code" for Authorization code flow.
                    config.SignedOutCallbackPath = "/Home/Index";

                    config.Scope.Add("rc.scope"); // The scope we want to request
                    config.Scope.Add("api1.read"); // The scope we want to request

                    config.GetClaimsFromUserInfoEndpoint = true; // This will make another round-trip to server to get claims.

                    config.ClaimActions.MapUniqueJsonKey("umesh.claim", "rc.grandma");
                });

            services.AddHttpClient();

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
