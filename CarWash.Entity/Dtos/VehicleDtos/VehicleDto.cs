namespace CarWash.Entity.Dtos.VehicleDtos;

public record VehicleDto
{
    public int BrandId { get; init; }
    public int CustomerId { get; init; }
    public int Model { get; init; }
    public string PlateNumber { get; init; }
}