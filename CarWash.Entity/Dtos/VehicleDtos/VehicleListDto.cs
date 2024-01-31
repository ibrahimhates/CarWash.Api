namespace CarWash.Entity.Dtos.VehicleDtos;

public record VehicleListDto : VehicleDto
{
    public int Id { get; init; }
    public DateTime LastWashDate { get; init; }
    public BrandDto Brand { get; init; }
}