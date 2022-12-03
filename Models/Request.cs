using System;

namespace dotnetserver.Models
{
	public class Request
	{
		public uint requestId { get; set; }
		public string status { get; set; }
		public string comment { get; set; }
		public User user { get; set; }
		public Organization organization { get; set; }
		public Vehicle vehicle { get; set; }
	}
}
