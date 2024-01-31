namespace CarWash.Entity.Dtos.Appointment
{
    public record CreateAppointmentDto
    {
        public DateTime AppointmentDate { get; set; }
        public int PackageId { get; set; }
        public int CustomerId { get; set; }
        public int VehicleId { get; set; }

    }
}
