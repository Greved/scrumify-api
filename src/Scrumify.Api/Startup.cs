using System;
using DryIoc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using DryIoc.Microsoft.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Scrumify.Api.Infrastructure.DI;
using Scrumify.Api.Infrastructure.Filters;
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
                .AddMvcCore(options => { options.Filters.Add(typeof(HttpGlobalExceptionFilter)); })
                .AddCors()
                .AddJsonFormatters();

            var mongoConnectionSection = Configuration.GetSection("MongoConnection");

            services.Configure<IMongoSettings>(mongoConnectionSection);

            var container = new Container();
            var serviceProvider = container.WithDependencyInjectionAdapter(services).ConfigureServiceProvider<CompositionRoot>();
            return serviceProvider;
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