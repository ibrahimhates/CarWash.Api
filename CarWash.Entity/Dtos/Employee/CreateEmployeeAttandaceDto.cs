using CarWash.Entity.Enums;

namespace CarWash.Entity.Dtos.Employee
{
    public record CreateEmployeeAttandaceDto
    {
        public int EmployeeId { get; init; }
        public List<Days>? OffDays { get; init; }
        public TimeSpan? BreakDurationBegin { get; init; } // Mola baslangic 
        public TimeSpan? BreakDurationEnd { get; init; } // mola bitis
        public TimeSpan ClockOutDate { get; init; } // mesai baslangic
        public TimeSpan ClockInDate { get; init; } // mesai bitis
        public DateTime HireDate { get; } = DateTime.Now; 
    }
}
