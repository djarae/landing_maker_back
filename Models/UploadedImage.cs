using System.ComponentModel.DataAnnotations;

namespace LandingMakerBack.Models;

/// <summary>
/// Imagen subida por un usuario para usar en sus landings
/// </summary>
public class UploadedImage
{
    public int Id { get; set; }
    
    /// <summary>
    /// Nombre original del archivo
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string OriginalFileName { get; set; } = string.Empty;
    
    /// <summary>
    /// Nombre del archivo en el servidor
    /// </summary>
    [Required]
    [MaxLength(255)]
    public string StoredFileName { get; set; } = string.Empty;
    
    /// <summary>
    /// Ruta relativa al archivo
    /// </summary>
    [Required]
    [MaxLength(500)]
    public string FilePath { get; set; } = string.Empty;
    
    /// <summary>
    /// Tipo MIME del archivo
    /// </summary>
    [MaxLength(100)]
    public string ContentType { get; set; } = string.Empty;
    
    /// <summary>
    /// Tamaño en bytes
    /// </summary>
    public long FileSize { get; set; }
    
    /// <summary>
    /// Fecha de subida
    /// </summary>
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Usuario que subió la imagen
    /// </summary>
    [Required]
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser? User { get; set; }
}
