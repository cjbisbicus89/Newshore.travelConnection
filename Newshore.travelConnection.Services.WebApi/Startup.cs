using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using AutoMapper;
using Newshore.travelConnection.Transversal.Mapper;
using Newshore.travelConnection.Transversal.Common;
using Newshore.travelConnection.Infrastructure.Data;
using Newshore.travelConnection.Infrastructure.Repository;
using Newshore.travelConnection.Infrastructure.Interface;
using Newshore.travelConnection.Domain.Interface;
using Newshore.travelConnection.Domain.Core;
using Newshore.travelConnection.Application.Interface;
using Newshore.travelConnection.Application.Main;
using Swashbuckle.AspNetCore.Swagger;
using System.Reflection;
using System.IO;
using Newshore.travelConnection.Services.WebApi.Modules.Swagger;
using Newshore.travelConnection.Services.WebApi.Modules.Authentication;
using Newshore.travelConnection.Services.WebApi.Modules.Mapper;
using Newshore.travelConnection.Services.WebApi.Modules.Feature;
using Newshore.travelConnection.Services.WebApi.Modules.Injection;
//using Newtonsoft.Json.Serialization;

namespace Newshore.travelConnection.Services.WebApi
{
    public class Startup
    {
        readonly string myPolicy = "policy";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMapper();
            services.AddFeature(this.Configuration);
            services.AddInjection(this.Configuration);
            services.AddAuthentication(this.Configuration);
            services.AddSwagger();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", " Prueba Ingreso NEWSHORE AIR- v1"));
            }

            app.UseRouting();
            app.UseCors(myPolicy);
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
