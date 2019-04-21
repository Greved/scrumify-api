using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Events;

namespace Scrumify.Api
{
    public class Program
    {
	    public static void Main(string[] args)
	    {
		    Log.Logger = new LoggerConfiguration()
			    .MinimumLevel.Debug()
			    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
			    .Enrich.FromLogContext()
			    .WriteTo.Console()
                .WriteTo.File("logs\\log.txt", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
			    .CreateLogger();

		    try
		    {
			    Log.Information("Starting web host");
			    CreateWebHostBuilder(args).Build().Run();
		    }
		    catch (Exception ex)
		    {
			    Log.Fatal(ex, "Host terminated unexpectedly");
			    throw;
		    }
		    finally
		    {
			    Log.CloseAndFlush();
		    }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder<Startup>(args)
                .UseSerilog();
    }
}