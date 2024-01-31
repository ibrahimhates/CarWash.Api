using CarWash.Entity.Enums;

namespace CarWash.Entity.Dtos.Employee;

public record EmployeeReportDetailListDto
{
    public int Id { get; set; }
    public string CustomerName { get; set; }
    public string PlateNumber { get; set; }
    public string PackageName { get; set; }
    public double Amount { get; set; }
    public string Comment { get; set; }
    public Rating Rating { get; set; }
}