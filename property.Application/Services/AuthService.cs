using System.IdentityModel.Tokens.Jwt;
using System.Security;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using property.Application.DTOs;
using property.Application.Interfaces;
using property.Domain.Entities;
using property.Domain.Infrastructure;

namespace property.Application.Services;

public class AuthService : IAuthService
{
    private IRepository<User> _repository;
    private readonly IMapper _mapper;
    private IConfiguration _config;

    public AuthService(IRepository<User> repository, IMapper mapper, IConfiguration config)
    {
        _mapper = mapper;
        _repository = repository;
        _config = config;
    }
    
    public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
    {
        var users = await _repository.GetAllAsync();
        var exist = users.FirstOrDefault(u => u.Email == registerDto.Email);

        if (exist != null)
            throw new SecurityException($"Ya existe un usuario registrado con este correo {registerDto.Email}");

        var user = _mapper.Map<User>(registerDto);
        user.CreatedAt = DateTime.UtcNow;
        user.UpatedAt = DateTime.UtcNow;
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.PasswordHash);
        
        // Para registro, primero crear sin refresh token
        user.RefreshToken = null;
        user.RefreshTokenExpire = null;

        await _repository.CreateAsync(user);
        
        // Luego generar tokens (que actualizará el refresh token)
        return await GenerateTokens(user);
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
    {
        var users = await _repository.GetAllAsync();
        var exist = users.FirstOrDefault(u => u.Email == loginDto.Email);

        if (exist == null || !BCrypt.Net.BCrypt.Verify(loginDto.PasswordHash, exist.PasswordHash))
            throw new SecurityException("Credenciales invalidas");

        return await GenerateTokens(exist);
    }

    public async Task<bool> RevokeTokenAsync(RevokeTokenDto revokeTokenDto)
    {
        var users = await _repository.GetAllAsync();
        var exist = users.FirstOrDefault(u => u.Email == revokeTokenDto.Email);
        if (exist == null) return false;

        exist.RefreshToken = null;
        exist.RefreshTokenExpire = null;
        exist.UpatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(exist);
        return true;
    }

    public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto refreshTokenDto)
    {
        var principal = GetPrincipalFromExpireToken(refreshTokenDto.Token);
        var email = principal.FindFirst(ClaimTypes.Email);

        if (email == null)
            throw new SecurityException("Token no valido");

        var users = await _repository.GetAllAsync();
        var exist = users.FirstOrDefault(u => u.Email == email.Value);

        if (exist == null || exist.RefreshToken != refreshTokenDto.RefreshToken ||
            exist.RefreshTokenExpire <= DateTime.UtcNow)
            throw new SecurityException("RefreshToken invalido o expiro");

        return await GenerateTokens(exist);
    }

    private JwtSecurityToken GenerateJwtToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.userName), 
            new Claim(ClaimTypes.Role, user.Role)
        };

        var securityT = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(1),
            signingCredentials: credentials
        );

        return securityT;
    }

    private ClaimsPrincipal GetPrincipalFromExpireToken(string token)
    {
        var validateParams = new TokenValidationParameters{
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])),
            ValidateLifetime = false
        };

        var handler = new JwtSecurityTokenHandler();
        var principal = handler.ValidateToken(token, validateParams, out var securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityException("Token invalido");

        return principal;
    }

    private string GenerateRefreshToken()
    {
        var a = new byte[32];
        using var rgn = RandomNumberGenerator.Create();
        rgn.GetBytes(a);
        return Convert.ToBase64String(a);
    }

    private async Task<AuthResponseDto> GenerateTokens(User user)
    {
        var token = GenerateJwtToken(user);
        var refresh = GenerateRefreshToken();

        user.RefreshToken = refresh;
        user.RefreshTokenExpire = DateTime.UtcNow.AddDays(7);
        user.UpatedAt = DateTime.UtcNow;
        
        await _repository.UpdateAsync(user);

        var responseDto = _mapper.Map<AuthResponseDto>(user);
        responseDto.Token = new JwtSecurityTokenHandler().WriteToken(token);

        return responseDto;
    }
}