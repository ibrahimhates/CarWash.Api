namespace CarWash.Entity.Dtos.VehicleDtos;

public record VehicleUpdateDto : VehicleDto
{
    public int Id { get; init; }
}