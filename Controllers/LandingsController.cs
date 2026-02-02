using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LandingMakerBack.Data;
using LandingMakerBack.Models;

namespace LandingMakerBack.Controllers;

/// <summary>
/// API pública para obtener landings (consumida por el frontend Vue)
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class LandingsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    
    public LandingsController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    /// <summary>
    /// Obtiene una landing pública por su ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<LandingPublicDto>> GetLanding(int id)
    {
        var landing = await _context.Landings
            .Include(l => l.Sections.OrderBy(s => s.Order))
            .FirstOrDefaultAsync(l => l.Id == id && l.IsPublished);
        
        if (landing == null)
        {
            return NotFound(new { message = "Landing no encontrada o no publicada" });
        }
        
        return Ok(new LandingPublicDto
        {
            Id = landing.Id,
            Name = landing.Name,
            PresetType = landing.PresetType.ToString(),
            StyleType = landing.StyleType.ToString(),
            ThemeMode = landing.ThemeMode.ToString(),
            Sections = landing.Sections.Select(s => new SectionDto
            {
                SectionType = s.SectionType,
                Order = s.Order,
                Config = s.ConfigJson
            }).ToList()
        });
    }
    
    /// <summary>
    /// Health check del API
    /// </summary>
    [HttpGet("health")]
    public ActionResult<object> Health()
    {
        return Ok(new { status = "healthy", timestamp = DateTime.UtcNow });
    }
}

/// <summary>
/// DTO para landing pública
/// </summary>
public class LandingPublicDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string PresetType { get; set; } = string.Empty;
    public string StyleType { get; set; } = string.Empty;
    public string ThemeMode { get; set; } = string.Empty;
    public List<SectionDto> Sections { get; set; } = new();
}

/// <summary>
/// DTO para sección
/// </summary>
public class SectionDto
{
    public string SectionType { get; set; } = string.Empty;
    public int Order { get; set; }
    public string Config { get; set; } = "{}";
}
