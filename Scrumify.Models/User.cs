using System;

namespace Scrumify.Models
{
	public class User
	{
		public Guid Id { get; set; }
		public string OuterId { get; set; }
		public Guid TeamId { get; set; }
		public string Name { get; set; }
    }
}