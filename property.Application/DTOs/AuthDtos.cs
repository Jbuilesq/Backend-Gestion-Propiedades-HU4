namespace property.Application.DTOs;

public class AuthResponseDto
{
    public int Id { get; set; }
    public string userName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string Role { get; set; }
    
    public string Token { get; set; }
    public DateTime RefreshTokenExpire { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpatedAt { get; set; }
}

public class LoginDto
{
    public string Email { get; set; }
    public string PasswordHash { get; set; }
}

public class RegisterDto
{
    public string userName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string Role { get; set; }
}

public class RefreshTokenDto
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}

public class RevokeTokenDto
{
    public string Email { get; set; }
}

