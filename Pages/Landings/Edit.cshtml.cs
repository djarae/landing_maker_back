using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LandingMakerBack.Data;
using LandingMakerBack.Models;
using LandingMakerBack.Models.Enums;

namespace LandingMakerBack.Pages.Landings;

[Authorize]
public class EditModel : PageModel
{
    private readonly ApplicationDbContext _context;
    
    public EditModel(ApplicationDbContext context)
    {
        _context = context;
    }
    
    [BindProperty]
    public Landing? Landing { get; set; }
    
    [BindProperty]
    public List<SectionEditModel>? Sections { get; set; }
    
    public string? SuccessMessage { get; set; }
    
    public class SectionEditModel
    {
        public int Id { get; set; }
        public string SectionType { get; set; } = string.Empty;
        public int Order { get; set; }
        public string ConfigJson { get; set; } = "{}";
    }
    
    public async Task<IActionResult> OnGetAsync(int id)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        
        Landing = await _context.Landings
            .Include(l => l.Sections.OrderBy(s => s.Order))
            .FirstOrDefaultAsync(l => l.Id == id && l.UserId == userId);
        
        if (Landing == null)
        {
            return RedirectToPage("/Landings/Index");
        }
        
        Sections = Landing.Sections.Select(s => new SectionEditModel
        {
            Id = s.Id,
            SectionType = s.SectionType,
            Order = s.Order,
            ConfigJson = FormatJson(s.ConfigJson)
        }).ToList();
        
        return Page();
    }
    
    public async Task<IActionResult> OnPostAsync(int id)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        
        var existingLanding = await _context.Landings
            .Include(l => l.Sections)
            .FirstOrDefaultAsync(l => l.Id == id && l.UserId == userId);
        
        if (existingLanding == null)
        {
            return RedirectToPage("/Landings/Index");
        }
        
        // Actualizar datos de la landing
        existingLanding.Name = Landing?.Name ?? existingLanding.Name;
        existingLanding.ThemeMode = Landing?.ThemeMode ?? existingLanding.ThemeMode;
        existingLanding.IsPublished = Landing?.IsPublished ?? false;
        existingLanding.UpdatedAt = DateTime.UtcNow;
        
        // Actualizar secciones
        if (Sections != null)
        {
            foreach (var sectionEdit in Sections)
            {
                var existingSection = existingLanding.Sections
                    .FirstOrDefault(s => s.Id == sectionEdit.Id);
                
                if (existingSection != null)
                {
                    existingSection.ConfigJson = MinifyJson(sectionEdit.ConfigJson);
                }
            }
        }
        
        await _context.SaveChangesAsync();
        
        // Recargar para mostrar mensaje
        Landing = existingLanding;
        Sections = existingLanding.Sections.OrderBy(s => s.Order).Select(s => new SectionEditModel
        {
            Id = s.Id,
            SectionType = s.SectionType,
            Order = s.Order,
            ConfigJson = FormatJson(s.ConfigJson)
        }).ToList();
        
        SuccessMessage = "¡Cambios guardados correctamente!";
        
        return Page();
    }
    
    private string FormatJson(string json)
    {
        try
        {
            var obj = System.Text.Json.JsonDocument.Parse(json);
            return System.Text.Json.JsonSerializer.Serialize(obj, new System.Text.Json.JsonSerializerOptions 
            { 
                WriteIndented = true 
            });
        }
        catch
        {
            return json;
        }
    }
    
    private string MinifyJson(string json)
    {
        try
        {
            var obj = System.Text.Json.JsonDocument.Parse(json);
            return System.Text.Json.JsonSerializer.Serialize(obj);
        }
        catch
        {
            return json;
        }
    }
}
