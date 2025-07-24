using MoodBoost.Models;

namespace MoodBoost.Services;

public interface IThemeService
{
    Task<List<Theme>> GetAllThemesAsync();
    Task<Theme?> GetActiveThemeAsync();
    Task<Theme> CreateThemeAsync(string name, string primaryColor, string secondaryColor, string? accentColor = null, string? backgroundColor = null, string? textColor = null);
    Task<bool> SetActiveThemeAsync(int themeId);
    Task<bool> DeleteThemeAsync(int themeId);
    Task<bool> UpdateThemeAsync(Theme theme);
}

public class ThemeService : IThemeService
{
    private readonly IDatabaseService _databaseService;

    public ThemeService(IDatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task<List<Theme>> GetAllThemesAsync()
    {
        return await _databaseService.GetAllThemesAsync();
    }

    public async Task<Theme?> GetActiveThemeAsync()
    {
        return await _databaseService.GetActiveThemeAsync();
    }

    public async Task<Theme> CreateThemeAsync(string name, string primaryColor, string secondaryColor, string? accentColor = null, string? backgroundColor = null, string? textColor = null)
    {
        var theme = new Theme
        {
            Name = name,
            PrimaryColor = primaryColor,
            SecondaryColor = secondaryColor,
            AccentColor = accentColor,
            BackgroundColor = backgroundColor,
            TextColor = textColor,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsActive = false
        };

        await _databaseService.SaveThemeAsync(theme);
        return theme;
    }

    public async Task<bool> SetActiveThemeAsync(int themeId)
    {
        try
        {
            await _databaseService.SetActiveThemeAsync(themeId);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteThemeAsync(int themeId)
    {
        try
        {
            var themes = await _databaseService.GetAllThemesAsync();
            var themeToDelete = themes.FirstOrDefault(t => t.Id == themeId);
            
            if (themeToDelete is null)
            {
                return false;
            }

            // Don't allow deletion if it's the only theme or if it's active
            if (themes.Count <= 1 || themeToDelete.IsActive)
            {
                return false;
            }

            await _databaseService.DeleteThemeAsync(themeToDelete);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> UpdateThemeAsync(Theme theme)
    {
        try
        {
            await _databaseService.SaveThemeAsync(theme);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
