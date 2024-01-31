using CarWash.Core.Entity;
using CarWash.Entity.Enums;

namespace CarWash.Entity.Entities
{
    public class WashProcess : EntityBase
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public int WashPackageId { get; set; }
        public CarWashStatus CarWashStatus { get; set; }
        public WashPackage WashPackage { get; set; }
        public ServiceReview ServiceReview { get; set; }
        public Appointment Appointment { get; set; }
        public ICollection<EmployeeWashProcess> Employees { get; set; }
    }
}