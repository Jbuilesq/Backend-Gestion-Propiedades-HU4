namespace property.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string userName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string Role { get; set; } = "User";
    
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpire { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpatedAt { get; set; }
}