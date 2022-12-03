namespace dotnetserver.Models
{
    public class IVehicle
    {
        public uint vehicleId { get; set; }
        public string organization { get; set; }
        public string model { get; set; }
        public string vehicleNumber { get; set; }
        public string vehicleType { get; set; }
        public string serviceType { get; set; }
        public string vehicleTypeExt { get; set; }
        public string vehicleChars { get; set; }
        public string country { get; set; }
        public string fuelType { get; set; }
        public string subOrganization { get; set; }
        public string ownershipType { get; set; }
    }

    public class Vehicle: IVehicle
    {
    }
}
