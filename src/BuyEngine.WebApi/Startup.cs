using BuyEngine.Catalog;
using BuyEngine.Checkout;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using NLog.Web;
using System.Diagnostics.CodeAnalysis;

namespace BuyEngine.WebApi;

[ExcludeFromCodeCoverage]
public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        //TODO Add Sql Data Configuration

        services.AddControllers().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        );

        services.AddLogging(loggingBuilder =>
        {
            // configure Logging with NLog
            loggingBuilder.ClearProviders();
            loggingBuilder.SetMinimumLevel(LogLevel.Trace);
            loggingBuilder.AddNLogWeb();
        });

        services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "BuyEngine API Documentation", Version = "v1" }); });

        //services.AddControllers();
        services.AddCatalogServices();
        services.AddCheckoutServices();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
            app.UseDeveloperExceptionPage();

        app.UseStaticFiles();
        app.UseHttpsRedirection();
        app.UsePathBase("/be-api"); //TODO Make PathBase Configurable
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapSwagger();
        });
        app.UseSwagger(sw => sw.RouteTemplate = "/docs/{documentName}/swagger.json");
        app.UseSwaggerUI(ui =>
        {
            ui.SwaggerEndpoint("/docs/v1/swagger.json", "BuyEngine API Documentation");
            ui.RoutePrefix = "docs";
        });
    }
}