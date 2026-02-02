using Microsoft.AspNetCore.Identity;

namespace LandingMakerBack.Models;

/// <summary>
/// Usuario del sistema que puede crear y gestionar landings
/// Extiende IdentityUser para autenticación
/// </summary>
public class ApplicationUser : IdentityUser
{
    /// <summary>
    /// Nombre completo del usuario
    /// </summary>
    public string FullName { get; set; } = string.Empty;
    
    /// <summary>
    /// Fecha de creación del usuario
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Landings creadas por este usuario
    /// </summary>
    public ICollection<Landing> Landings { get; set; } = new List<Landing>();
}
