namespace dotnetserver.Models
{
    public class IUser
    {
        public uint userId { get; set; }
        public string username { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string password { get; set; }
        public string avatarUrl { get; set; }
    }

    public class TbUser : IUser
    {

    }

}
