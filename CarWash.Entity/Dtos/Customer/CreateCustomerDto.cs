using System.Text.Json.Serialization;

namespace CarWash.Entity.Dtos.Customer
{
    public record CreateCustomerDto
    {
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
    }
}
