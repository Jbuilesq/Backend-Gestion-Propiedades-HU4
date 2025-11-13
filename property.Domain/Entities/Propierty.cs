using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace property.Domain.Entities;

public class Propierty
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double Price { get; set; }
    public string Localization { get; set; } = string.Empty;
    public string ImagesJson { get; set; } = string.Empty;
    
    [NotMapped]
    public List<string> Images
    {
        get => string.IsNullOrEmpty(ImagesJson)
            ? new List<string>()
            : JsonSerializer.Deserialize<List<string>>(ImagesJson)!;

        set => ImagesJson = JsonSerializer.Serialize(value);
    }
}