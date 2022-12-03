using System;

namespace dotnetserver.Models
{
	public class Request
	{
		public uint RequestId { get; set; }
		public string Status { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public DateTime RequestDate { get; set; }
		public uint UserId { get; set; }
		public uint OrganizationId { get; set; }
	}
}
