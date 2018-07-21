using System;
using DryIoc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using DryIoc.Microsoft.DependencyInjection;
using Scrumify.Api.DI;

namespace Scrumify.Api
{
    public class Startup
    {
	    public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvcCore()
                .AddCors()
                .AddJsonFormatters();

	        var container = new Container().WithDependencyInjectionAdapter(services).ConfigureServiceProvider<CompositionRoot>();
	        return container;
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app
                .UseDefaultFiles()
                .UseStaticFiles()
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