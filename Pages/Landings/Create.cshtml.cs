using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using LandingMakerBack.Data;
using LandingMakerBack.Models;
using LandingMakerBack.Models.Enums;

namespace LandingMakerBack.Pages.Landings;

[Authorize]
public class CreateModel : PageModel
{
    private readonly ApplicationDbContext _context;
    
    public CreateModel(ApplicationDbContext context)
    {
        _context = context;
    }
    
    [BindProperty]
    public InputModel Input { get; set; } = new();
    
    public string? ErrorMessage { get; set; }
    
    public class InputModel
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        public PresetType PresetType { get; set; } = PresetType.Pyme;
        
        [Required]
        public StyleType StyleType { get; set; } = StyleType.Miel;
        
        public ThemeMode ThemeMode { get; set; } = ThemeMode.Toggle;
    }
    
    public void OnGet()
    {
    }
    
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToPage("/Account/Login");
        }
        
        var landing = new Landing
        {
            Name = Input.Name,
            PresetType = Input.PresetType,
            StyleType = Input.StyleType,
            ThemeMode = Input.ThemeMode,
            UserId = userId,
            IsPublished = false
        };
        
        // Crear secciones por defecto según el preset
        landing.Sections = CreateDefaultSections(Input.PresetType);
        
        _context.Landings.Add(landing);
        await _context.SaveChangesAsync();
        
        return RedirectToPage("/Landings/Edit", new { id = landing.Id });
    }
    
    private List<LandingSection> CreateDefaultSections(PresetType presetType)
    {
        var sections = new List<LandingSection>();
        
        if (presetType == PresetType.MobileApp)
        {
            sections.Add(new LandingSection { SectionType = "hero", Order = 1, ConfigJson = GetDefaultHeroMobileConfig() });
            sections.Add(new LandingSection { SectionType = "features", Order = 2, ConfigJson = GetDefaultFeaturesConfig() });
            sections.Add(new LandingSection { SectionType = "screenshots", Order = 3, ConfigJson = GetDefaultScreenshotsConfig() });
            sections.Add(new LandingSection { SectionType = "cta", Order = 4, ConfigJson = GetDefaultCtaConfig() });
            sections.Add(new LandingSection { SectionType = "footer", Order = 5, ConfigJson = GetDefaultFooterConfig() });
        }
        else // Pyme
        {
            sections.Add(new LandingSection { SectionType = "hero", Order = 1, ConfigJson = GetDefaultHeroPymeConfig() });
            sections.Add(new LandingSection { SectionType = "about", Order = 2, ConfigJson = GetDefaultAboutConfig() });
            sections.Add(new LandingSection { SectionType = "products", Order = 3, ConfigJson = GetDefaultProductsConfig() });
            sections.Add(new LandingSection { SectionType = "contact", Order = 4, ConfigJson = GetDefaultContactConfig() });
        }
        
        return sections;
    }
    
    private string GetDefaultHeroMobileConfig() => @"{
        ""appName"": ""Mi App"",
        ""tagline"": ""Una frase que describe tu app"",
        ""ctaText"": ""Descargar Ahora"",
        ""ctaUrl"": ""#"",
        ""heroImage"": """"
    }";
    
    private string GetDefaultFeaturesConfig() => @"{
        ""title"": ""Características"",
        ""features"": [
            { ""icon"": ""⚡"", ""title"": ""Rápida"", ""description"": ""Rendimiento optimizado"" },
            { ""icon"": ""🔒"", ""title"": ""Segura"", ""description"": ""Datos protegidos"" },
            { ""icon"": ""✨"", ""title"": ""Fácil"", ""description"": ""Interfaz intuitiva"" }
        ]
    }";
    
    private string GetDefaultScreenshotsConfig() => @"{
        ""title"": ""Descubre la app"",
        ""images"": []
    }";
    
    private string GetDefaultCtaConfig() => @"{
        ""title"": ""¿Listo para comenzar?"",
        ""subtitle"": ""Descarga la app ahora"",
        ""buttonText"": ""Descargar"",
        ""buttonUrl"": ""#""
    }";
    
    private string GetDefaultFooterConfig() => @"{
        ""copyright"": ""© 2026 Mi App"",
        ""links"": [
            { ""label"": ""Privacidad"", ""url"": ""#"" },
            { ""label"": ""Términos"", ""url"": ""#"" }
        ]
    }";
    
    private string GetDefaultHeroPymeConfig() => @"{
        ""businessName"": ""Mi Negocio"",
        ""tagline"": ""Los mejores productos para ti"",
        ""ctaText"": ""Contáctanos"",
        ""ctaUrl"": ""#contacto"",
        ""heroImage"": """"
    }";
    
    private string GetDefaultAboutConfig() => @"{
        ""title"": ""Sobre Nosotros"",
        ""description"": ""Somos un negocio dedicado a ofrecer productos de calidad..."",
        ""image"": """"
    }";
    
    private string GetDefaultProductsConfig() => @"{
        ""title"": ""Nuestros Productos"",
        ""products"": [
            { ""name"": ""Producto 1"", ""price"": ""$10.00"", ""image"": """" },
            { ""name"": ""Producto 2"", ""price"": ""$15.00"", ""image"": """" },
            { ""name"": ""Producto 3"", ""price"": ""$20.00"", ""image"": """" }
        ]
    }";
    
    private string GetDefaultContactConfig() => @"{
        ""title"": ""Contáctanos"",
        ""phone"": ""+56 9 1234 5678"",
        ""email"": ""contacto@minegocio.cl"",
        ""whatsapp"": ""+56912345678"",
        ""address"": ""Santiago, Chile""
    }";
}
