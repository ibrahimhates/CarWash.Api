using CarWash.Entity.Enums;

namespace CarWash.Entity.Dtos.Appointment;

public record AppointmentScoreDto
{
    public int Id { get; init; }
    public Rating Rating{ get; init; }
    public string? Comment { get; init; }
}