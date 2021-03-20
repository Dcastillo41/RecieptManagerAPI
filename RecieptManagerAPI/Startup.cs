using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

using RecieptManagerAPI.Services;
using RecieptManagerAPI.Controllers;

namespace RecieptManagerAPI
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
            services.AddSwaggerGen(config => {
                config.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo {
                    Title = "RecieptManagerAPI", Version = "v1"
                });
            });

            services.AddControllers();

            services.AddTransient<UserService>();
            services.AddTransient<RecieptService>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRecieptService, RecieptService>();

            services.AddControllers().AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger(c => c.PreSerializeFilters.Add((swagger, httpRequest) => swagger.Servers =
                new List<OpenApiServer> {
                    new OpenApiServer { Url = $"{httpRequest.Scheme}://{httpRequest.Host.Value}" }
                }));

            app.UseSwaggerUI(e => e.SwaggerEndpoint("/swagger/v1/swagger.json", "RecieptManagerAPI"));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
