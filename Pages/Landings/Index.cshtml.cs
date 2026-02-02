using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LandingMakerBack.Data;
using LandingMakerBack.Models;

namespace LandingMakerBack.Pages.Landings;

[Authorize]
public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;
    
    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public List<Landing>? Landings { get; set; }
    
    public async Task OnGetAsync()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        
        if (!string.IsNullOrEmpty(userId))
        {
            Landings = await _context.Landings
                .Where(l => l.UserId == userId)
                .OrderByDescending(l => l.UpdatedAt)
                .ToListAsync();
        }
    }
}
