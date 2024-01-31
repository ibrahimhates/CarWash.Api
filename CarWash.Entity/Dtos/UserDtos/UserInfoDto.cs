namespace CarWash.Entity.Dtos.UserDtos
{
    public record UserInfoDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName => $"{FirstName} {LastName}";
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
    }
}
