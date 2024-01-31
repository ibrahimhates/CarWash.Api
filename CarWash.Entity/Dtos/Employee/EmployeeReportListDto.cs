namespace CarWash.Entity.Dtos.Employee;

public record EmployeeReportListDto
{
    public int UserId { get; init; }
    public string FullName { get; init; }
    public double WeeklyIncome { get; init; }
    public double MonthlyIncome { get; init; }
    public float TotalScore { get; init; }
}