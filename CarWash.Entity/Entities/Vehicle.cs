using CarWash.Core.Entity;

namespace CarWash.Entity.Entities
{
    public class Vehicle : EntityBase
    {
        public int Id { get; set; }
        public int BrandId { get; set; }
        public int CustomerId { get; set; }
        public string Model { get; set; }
        public string PlateNumber { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
        public DateTime? LastWashDate { get; set; }
        public Customer Customer { get; set; }
        public Brand Brand { get; set; }
    }
}
