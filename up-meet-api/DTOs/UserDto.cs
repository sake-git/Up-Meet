namespace up_meet_api.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }

        public string LoginId { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string? Name { get; set; }

        public string Email { get; set; } = null!;

        public string? Phone { get; set; }

        public string? Token { get; set; }
    }
}
