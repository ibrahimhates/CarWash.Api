using CarWash.Core.Entity;


namespace CarWash.Entity.Entities
{
    public class EmployeeWashProcess : EntityBase
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int WashProcessId { get; set; }
        public Employee Employee{ get; set; }
        public WashProcess WashProcess { get; set; }
    }
}
