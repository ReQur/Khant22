using System;

namespace dotnetserver.Models
{
	public class Request
	{
		public uint requestId { get; set; }
		public string status { get; set; }
		public DateTime startDate { get; set; }
		public DateTime endDate { get; set; }
		public DateTime requestDate { get; set; }
		public User user { get; set; }
		public Organization organization { get; set; }
	}
}
