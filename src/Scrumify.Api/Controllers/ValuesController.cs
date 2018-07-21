using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Scrumify.DataAccess.Core;
using Serilog;

namespace Scrumify.Api.Controllers
{
	[Route("api/[controller]")]
	public class ValuesController : ControllerBase
	{
		private readonly IDbConnectionStringProvider dbConnectionStringProvider;

		public ValuesController(IDbConnectionStringProvider dbConnectionStringProvider)
		{
			this.dbConnectionStringProvider = dbConnectionStringProvider;
		}

		// GET: api/values
		[HttpGet]
		public IEnumerable<string> Get()
		{
            return new[] { "value1", "value2", "value3" };
		}

		// GET api/values/5
		[HttpGet("{id}")]
		public string Get(int id)
		{
			return dbConnectionStringProvider.Get();
		}
	}
}