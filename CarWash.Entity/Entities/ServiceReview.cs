using CarWash.Core.Entity;
using CarWash.Entity.Enums;

namespace CarWash.Entity.Entities
{
    public class ServiceReview : EntityBase
    {
        public int Id { get; set; }
        public int WashProcessId { get; set; }
        public string? Comment { get; set; }
        public Rating Rating { get; set; }
        public WashProcess WashProcess { get; set; }
    }
}
