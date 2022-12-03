namespace dotnetserver.Models
{
    public class IVehicle
    {
        public uint vehicleId { get; set; }
        public Organization organization { get; set; }
        public string model { get; set; }
        public string vehicleNumber { get; set; }
        public vehicleType vehicleType { get; set; }
        public serviceType serviceType { get; set; }
        public vehicleTypeExt vehicleTypeExt { get; set; }
        public string vehicleChars { get; set; }
        public string country { get; set; }
        public string fuelType { get; set; }
        public bool subOrganization { get; set; }
        public string ownershipType { get; set; }
    }

    public class Vehicle: IVehicle
    {
    }
}
