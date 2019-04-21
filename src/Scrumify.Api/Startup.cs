using System;
using DryIoc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using DryIoc.Microsoft.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Scrumify.Api.DI;
using Scrumify.DataAccess.Mongo;

namespace Scrumify.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvcCore()
                .AddCors()
                .AddJsonFormatters();

            var mongoConnectionSection = Configuration.GetSection("MongoConnection");

            services.Configure<IMongoSettings>(mongoConnectionSection);

            var container = new Container().WithDependencyInjectionAdapter(services).ConfigureServiceProvider<CompositionRoot>();
	        return container;
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app
                .UseCors(builder =>
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                )
                .UseMvcWithDefaultRoute();
        }
    }
}