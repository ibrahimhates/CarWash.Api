namespace CarWash.Entity.Dtos.VehicleDtos;

public record VehicleListForAppointmentDto
{
    public int Id { get; set; }
    public string PlateNumber { get; init; }
    public string BrandName { get; init; }
}