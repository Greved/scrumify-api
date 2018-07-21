using Microsoft.Extensions.Configuration;
using Scrumify.DataAccess.Core;

namespace Scrumify.Api.DataAccess
{
	public class DbConnectionStringProvider : IDbConnectionStringProvider
	{
		private readonly IConfiguration configuration;

		public DbConnectionStringProvider(IConfiguration configuration)
		{
			this.configuration = configuration;
		}

		public string Get()
		{
			return configuration["DbConnectionString"];
		}
	}
}