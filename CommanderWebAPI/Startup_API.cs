using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CommanderWebAPI.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;

namespace CommanderWebAPI
{
    public class Startup_API
    {
        private string _key = "this_is_My_very_dumb_Secret_Key_hence_please_do_not_use_this";
        private string _issuer = "mywebsite.com";
        private string _audience = "clientaudience";

        public Startup_API(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Simple JWT Token based authentication
            /*
            services.AddAuthentication("jwttoken")
                .AddJwtBearer("jwttoken", opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = _issuer,
                        ValidAudience = _audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key))

                    };
                });
            */

            // Identity server 4 based Client Credential
            services.AddAuthentication("ID4ClientCredentialToken")
                .AddJwtBearer("ID4ClientCredentialToken", conf =>
                {
                    conf.Authority = "https://localhost:44323/";
                    conf.Audience = "CommanderWebAPI";
                });

            services.AddAuthorization();

            services.AddDbContext<CommanderContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("CommanderConnection"));
            });

            services.AddControllers()
                .AddNewtonsoftJson(s => s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver());

            services.AddScoped<ICommanderRepo, SqlCommanderRepo>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
