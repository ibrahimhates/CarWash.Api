using CarWash.Core.Entity;

namespace CarWash.Entity.Entities
{
    public class Brand : EntityBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Vehicle> Vehicles { get; set; }
    }
}
