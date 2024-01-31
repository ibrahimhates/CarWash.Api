namespace CarWash.Core.Entity
{
    public abstract class EntityBase 
    {
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
