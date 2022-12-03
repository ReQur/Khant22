namespace dotnetserver.Models
{
	public class User
    {
	    public uint userId { get; set; }
	    public string firstName { get; set; }
	    public string lastName { get; set; }
	    public string Login { get; set; }
	    public string password { get; set; }
	    public uint OrganizationId { get; set; }
	    public string JobTitle { get; set; }
    }
}
