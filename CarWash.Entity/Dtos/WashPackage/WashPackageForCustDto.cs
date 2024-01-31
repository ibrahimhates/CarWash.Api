
namespace CarWash.Entity.Dtos.WashPackage;

public record WashPackageForCustDto
{
    public int Id { get; init; }
    public double Price { get; init; }
    public int Duration { get; init; }
    public string PackageName { get; init; }
}