using CarWash.Core.Entity;

namespace CarWash.Entity.Entities
{
    public class Payment : EntityBase
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public double Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public Appointment Appointment { get; set; }
    }
}
