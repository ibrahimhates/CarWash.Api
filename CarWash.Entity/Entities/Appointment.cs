using CarWash.Core.Entity;

namespace CarWash.Entity.Entities
{
    public class Appointment : EntityBase
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int PackageId { get; set; }
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        public DateTime AppointmentDate { get; set; }
        public WashPackage WashPackage { get; set; }
        public Customer Customer { get; set; }
        public Payment Payment { get; set; }
        public WashProcess WashProcess { get; set; }
    }
}
