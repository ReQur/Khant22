namespace dotnetserver.Models
{
    public class IVehicle
    {
        public uint vehicleId { get; set; }
        public uint organizationId { get; set; }
        public string model { get; set; }
        public string vehicleNumber { get; set; }
        public uint vehicleTypeId { get; set; }
        public uint serviceTypeId { get; set; }
        public uint vehicleTypeExtId { get; set; }
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
