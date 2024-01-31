namespace CarWash.Entity.Dtos.WashPackage
{
    public record WashPackageDto
    {
        public int Id { get; set; }
        public string PackageName { get; init; }
        public string? Description { get; init; }
        public double Price { get; init; }
        public int Duration { get; init; } // dakika cinsinden
    }
}
