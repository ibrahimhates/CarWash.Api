using CarWash.Entity.Dtos.VehicleDtos;
using CarWash.Entity.Enums;

namespace CarWash.Entity.Dtos.Appointment
{
    public record AppointmentForManagerDto
    {
        public int Id { get; set; }
        public VehicleListDto Vehicle { get; set; }
        public string PackageName { get; set; }
        public DateTime AppointmentDate { get; init; }
        public Rating? Rating { get; set; }
        public CarWashStatus CarWashStatus { get; set; }
        public string WasherName { get; set; }

    }
}
