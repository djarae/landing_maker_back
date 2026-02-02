using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using LandingMakerBack.Models;

namespace LandingMakerBack.Pages.Account;

public class LoginModel : PageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    
    public LoginModel(SignInManager<ApplicationUser> signInManager)
    {
        _signInManager = signInManager;
    }
    
    [BindProperty]
    public InputModel Input { get; set; } = new();
    
    public string? ErrorMessage { get; set; }
    
    public class InputModel
    {
        [Required(ErrorMessage = "El correo es requerido")]
        [EmailAddress(ErrorMessage = "Correo inválido")]
        public string Email { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "La contraseña es requerida")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        
        public bool RememberMe { get; set; }
    }
    
    public void OnGet()
    {
    }
    
    public async Task<IActionResult> OnPostAsync()
    {
        Console.WriteLine($"[LOGIN INTENTO] Email: {Input.Email}");
        
        if (!ModelState.IsValid)
        {
            Console.WriteLine("[LOGIN ERROR] Modelo inválido");
            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    Console.WriteLine($" - {state.Key}: {error.ErrorMessage}");
                }
            }
            return Page();
        }
        
        // Verificar si el usuario existe antes de intentar login
        var user = await _signInManager.UserManager.FindByEmailAsync(Input.Email);
        if (user == null)
        {
            Console.WriteLine("[LOGIN ERROR] Usuario no encontrado en BD");
            ErrorMessage = "Usuario no encontrado.";
            return Page();
        }
        
        var result = await _signInManager.PasswordSignInAsync(
            Input.Email, 
            Input.Password, 
            Input.RememberMe, 
            lockoutOnFailure: false);
            
        Console.WriteLine($"[LOGIN RESULTADO] Éxito: {result.Succeeded}, Bloqueado: {result.IsLockedOut}, No permitido: {result.IsNotAllowed}");
        
        if (result.Succeeded)
        {
            Console.WriteLine("[LOGIN ÉXITO] Redirigiendo a /Index...");
            return RedirectToPage("/Index");
        }
        
        ErrorMessage = "Credenciales inválidas. Por favor intente de nuevo.";
        return Page();
    }
}
