using System.ComponentModel.DataAnnotations;

namespace LandingMakerBack.Models;

/// <summary>
/// Representa una sección dentro de una landing
/// Las secciones están ordenadas por Order
/// </summary>
public class LandingSection
{
    public int Id { get; set; }
    
    /// <summary>
    /// Tipo de sección (hero, features, products, cta, footer, etc.)
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string SectionType { get; set; } = string.Empty;
    
    /// <summary>
    /// Orden de la sección en la landing
    /// </summary>
    public int Order { get; set; }
    
    /// <summary>
    /// Configuración JSON de la sección
    /// Contiene textos, imágenes, colores, etc.
    /// </summary>
    public string ConfigJson { get; set; } = "{}";
    
    /// <summary>
    /// Landing a la que pertenece
    /// </summary>
    public int LandingId { get; set; }
    public Landing? Landing { get; set; }
}
