namespace CarWash.Entity.Dtos.Employee;

public record EmployeeListDto
{
    public int UserId { get; init; }
    public int RoleId { get; init; }
    public string RoleName { get; init; }
    public DateTime HireDate { get; init; }
    public string FullName { get; init; }
}