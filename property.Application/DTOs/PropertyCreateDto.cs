namespace property.Application.DTOs;

public class PropertyCreateDto
{
    
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double Price { get; set; }
    public string Localization { get; set; } = string.Empty;
}