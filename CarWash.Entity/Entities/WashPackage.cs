using CarWash.Core.Entity;

namespace CarWash.Entity.Entities
{
    public class WashPackage : EntityBase
    {
        public int Id { get; set; }
        public string PackageName { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public int Duration { get; set; } // dakika cinsinden
        public ICollection<WashProcess> WashProcesses { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}