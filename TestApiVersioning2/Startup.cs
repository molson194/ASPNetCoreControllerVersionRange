using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc.Versioning;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace TestApiVersioning2
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
            services.AddControllers();
            services.AddMvc();
            services.AddApiVersioning(options => {
                options.AssumeDefaultVersionWhenUnspecified = false;
                options.ReportApiVersions = true;
                options.ApiVersionReader = new QueryStringApiVersionReader("api-version");
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("2020-01-01-preview", new OpenApiInfo { Title = "My API 0", Version = "v0" });
                c.SwaggerDoc("2020-01-01", new OpenApiInfo { Title = "My API 1", Version = "v1" });
                c.SwaggerDoc("2021-01-01-preview", new OpenApiInfo { Title = "My API 2", Version = "v2" });
                c.SwaggerDoc("2021-01-01", new OpenApiInfo { Title = "My API 3", Version = "v3" });
                c.SwaggerDoc("2022-01-01-preview", new OpenApiInfo { Title = "My API 4", Version = "v4" });
                c.DocInclusionPredicate((version, desc) =>
                {
                    if (!desc.TryGetMethodInfo(out MethodInfo methodInfo)) return false;
                    // var versions = methodInfo.DeclaringType.GetCustomAttributes(true).OfType<StableApiRangeAttribute>().SelectMany(attr => attr.Versions); // Class Level Attributes
                    var versions = methodInfo.GetCustomAttribute<ApiRangeAttribute>().Versions;
                    var a = versions[0].ToString();
                    return versions.Any(v => v.ToString() == version);
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("2020-01-01/swagger.json", "My API V1");
            });
        }
    }
}
