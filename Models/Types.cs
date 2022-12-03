namespace dotnetserver.Models
{
    public class IType
    {
        public uint typeId { get; set; }
        public string name { get; set; }
    }

    public class vehicleType : IType
    {
    }
    public class serviceType : IType
    {
    }
    public class vehicleTypeExt : IType
    {
    }

}
