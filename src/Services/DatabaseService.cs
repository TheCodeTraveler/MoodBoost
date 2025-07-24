using MoodBoost.Models;
using SQLite;

namespace MoodBoost.Services;

public interface IDatabaseService
{
    Task InitializeAsync();
    Task<List<Theme>> GetAllThemesAsync();
    Task<Theme?> GetActiveThemeAsync();
    Task<int> SaveThemeAsync(Theme theme);
    Task<int> DeleteThemeAsync(Theme theme);
    Task<int> SetActiveThemeAsync(int themeId);
}

public class DatabaseService : IDatabaseService
{
    private readonly string _databasePath;
    private SQLiteAsyncConnection? _database;

    public DatabaseService()
    {
        _databasePath = Path.Combine(FileSystem.AppDataDirectory, "MoodBoost.db3");
    }

    public async Task InitializeAsync()
    {
        if (_database is not null)
        {
            return;
        }

        _database = new SQLiteAsyncConnection(_databasePath);
        await _database.CreateTableAsync<Theme>();
        await SeedDefaultThemesAsync();
    }

    public async Task<List<Theme>> GetAllThemesAsync()
    {
        await InitializeAsync();
        
        if (_database is null)
        {
            throw new InvalidOperationException("Database not initialized");
        }
        
        return await _database.Table<Theme>().OrderBy(t => t.Name).ToListAsync();
    }

    public async Task<Theme?> GetActiveThemeAsync()
    {
        await InitializeAsync();
        
        if (_database is null)
        {
            throw new InvalidOperationException("Database not initialized");
        }
        
        return await _database.Table<Theme>().Where(t => t.IsActive).FirstOrDefaultAsync();
    }

    public async Task<int> SaveThemeAsync(Theme theme)
    {
        await InitializeAsync();
        
        if (_database is null)
        {
            throw new InvalidOperationException("Database not initialized");
        }
        
        theme.UpdatedAt = DateTime.UtcNow;
        
        if (theme.Id != 0)
        {
            return await _database.UpdateAsync(theme);
        }
        else
        {
            theme.CreatedAt = DateTime.UtcNow;
            return await _database.InsertAsync(theme);
        }
    }

    public async Task<int> DeleteThemeAsync(Theme theme)
    {
        await InitializeAsync();
        
        if (_database is null)
        {
            throw new InvalidOperationException("Database not initialized");
        }
        
        return await _database.DeleteAsync(theme);
    }

    public async Task<int> SetActiveThemeAsync(int themeId)
    {
        await InitializeAsync();
        
        if (_database is null)
        {
            throw new InvalidOperationException("Database not initialized");
        }
        
        // First, deactivate all themes
        await _database.ExecuteAsync("UPDATE Themes SET IsActive = 0");
        
        // Then activate the selected theme
        return await _database.ExecuteAsync("UPDATE Themes SET IsActive = 1 WHERE Id = ?", themeId);
    }

    private async Task SeedDefaultThemesAsync()
    {
        if (_database is null)
        {
            throw new InvalidOperationException("Database not initialized");
        }
        
        var existingThemes = await _database.Table<Theme>().CountAsync();
        
        if (existingThemes == 0)
        {
            var defaultThemes = MoodData.DefaultThemes.ToList();
            
            // Set the first theme as active
            if (defaultThemes.Count > 0)
            {
                defaultThemes[0].IsActive = true;
            }
            
            await _database.InsertAllAsync(defaultThemes);
        }
    }
}
