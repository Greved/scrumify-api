using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Scrumify.Api.Controllers
{
	[Route("api/[controller]")]
	public class ValuesController : ControllerBase
	{
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
			return "value";
		}
	}
}