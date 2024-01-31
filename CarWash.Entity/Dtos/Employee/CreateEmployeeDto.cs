using CarWash.Entity.Enums;
using System.Text.Json.Serialization;

namespace CarWash.Entity.Dtos.Employee
{
    public record CreateEmployeeDto
    {
        public int RoleId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [JsonIgnore]
        public string FullName => $"{FirstName} {LastName}";
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public CreateEmployeeAttandaceDto? Attandace { get; set; }
    }
}
