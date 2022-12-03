namespace dotnetserver.Models
{
	public class User
    {
	    public uint userId { get; set; }
	    public string firstName { get; set; }
	    public string lastName { get; set; }
		public string login { get; set; }
	    public string password { get; set; }
	    public Organization organization { get; set; }
	    public string jobTitle { get; set; }
    }
}
