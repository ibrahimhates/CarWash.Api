using CarWash.Core.Entity;

namespace CarWash.Entity.Entities
{
    public class Employee : EntityBase
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public EmployeeAttendance EmployeeAttendance { get; set; }
        public ICollection<EmployeeWashProcess>? WashProcesses { get; set; }
        public User User { get; set; }
    }
}
