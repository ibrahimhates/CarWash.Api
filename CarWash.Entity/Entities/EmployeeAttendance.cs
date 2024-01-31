using CarWash.Core.Entity;
using CarWash.Entity.Enums;

namespace CarWash.Entity.Entities
{
    public class EmployeeAttendance : EntityBase
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Days[]? OffDays { get; set; }
        public TimeSpan? BreakDurationBegin { get; set; }
        public TimeSpan? BreakDurationEnd { get; set; }
        public TimeSpan ClockOutDate { get; set; }
        public TimeSpan ClockInDate { get; set; }
        public DateTime HireDate { get; set; }
        public Employee Employee { get; set; }
    }
}
 