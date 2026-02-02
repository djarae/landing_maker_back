using System.ComponentModel.DataAnnotations;
using LandingMakerBack.Models.Enums;

namespace LandingMakerBack.Models;

/// <summary>
/// Representa una landing page configurada
/// </summary>
public class Landing
{
    public int Id { get; set; }
    
    /// <summary>
    /// Nombre identificador de la landing
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Tipo de preset (MobileApp o Pyme)
    /// </summary>
    public PresetType PresetType { get; set; }
    
    /// <summary>
    /// Estilo visual seleccionado
    /// </summary>
    public StyleType StyleType { get; set; }
    
    /// <summary>
    /// Modo del tema (Toggle, DayOnly, NightOnly)
    /// </summary>
    public ThemeMode ThemeMode { get; set; } = ThemeMode.Toggle;
    
    /// <summary>
    /// Si la landing está publicada
    /// </summary>
    public bool IsPublished { get; set; } = false;
    
    /// <summary>
    /// Fecha de creación
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Fecha de última modificación
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Usuario propietario
    /// </summary>
    [Required]
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser? User { get; set; }
    
    /// <summary>
    /// Secciones de la landing
    /// </summary>
    public ICollection<LandingSection> Sections { get; set; } = new List<LandingSection>();
}
