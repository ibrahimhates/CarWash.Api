using CarWash.Core.Entity;

namespace CarWash.Entity.Entities
{
    public class Customer : EntityBase
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public ICollection<Vehicle>? Vehicles { get; set; }
        public ICollection<Appointment>? Appointments { get; set; }
    }
}
