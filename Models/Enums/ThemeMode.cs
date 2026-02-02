namespace LandingMakerBack.Models.Enums;

/// <summary>
/// Modo de tema para la landing
/// </summary>
public enum ThemeMode
{
    /// <summary>El usuario puede alternar entre día y noche</summary>
    Toggle = 0,
    
    /// <summary>Siempre modo día</summary>
    DayOnly = 1,
    
    /// <summary>Siempre modo noche</summary>
    NightOnly = 2
}
