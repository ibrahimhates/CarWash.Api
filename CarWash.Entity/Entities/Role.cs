using CarWash.Core.Entity;

namespace CarWash.Entity.Entities
{
    public class Role : EntityBase
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public ICollection<Employee> Users { get; set; }
    }
}
