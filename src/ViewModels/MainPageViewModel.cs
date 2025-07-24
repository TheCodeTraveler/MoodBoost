using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoodBoost.Models;
using MoodBoost.Services;
using System.Collections.ObjectModel;

namespace MoodBoost.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    readonly IThemeService _themeService;

    public MainPageViewModel(IThemeService themeService)
    {
        _themeService = themeService;
        Moods = [.. MoodData.Moods];
        Themes = [];
        TodayDate = DateTimeOffset.Now;
        GenerateNewQuote();
    }

    public ObservableCollection<MoodEntry> Moods { get; }

    public ObservableCollection<Theme> Themes { get; }

    [ObservableProperty]
    public partial Theme? ActiveTheme { get; set; }

    [ObservableProperty]
    public partial string CurrentQuote { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool HasSelectedMood { get; set; }

    [ObservableProperty]
    public partial bool IsLoading { get; set; }

    [ObservableProperty]
    public partial MoodEntry? SelectedMood { get; set; }

    [ObservableProperty]
    public partial DateTimeOffset TodayDate { get; set; }

    public async Task InitializeAsync()
    {
        await LoadThemesAsync();
        await LoadActiveThemeAsync();
    }

    [RelayCommand]
    async Task CreateThemeAsync(string themeName)
    {
        if (string.IsNullOrWhiteSpace(themeName))
        {
            return;
        }

        IsLoading = true;

        try
        {
            // Generate random colors for the new theme
            var random = Random.Shared;
            var colors = new[]
            {
                "#FF6B35", "#F7931E", "#FFD700", "#4CAF50", "#8BC34A",
                "#2196F3", "#03DAC6", "#9C27B0", "#E91E63", "#FF5722"
            };

            var primaryColor = colors[random.Next(colors.Length)];
            var secondaryColor = colors[random.Next(colors.Length)];

            await _themeService.CreateThemeAsync(themeName, primaryColor, secondaryColor);
            await LoadThemesAsync();
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    async Task DeleteThemeAsync(Theme theme)
    {
        if (theme is null)
        {
            return;
        }

        IsLoading = true;

        try
        {
            var success = await _themeService.DeleteThemeAsync(theme.Id);
            if (success)
            {
                await LoadThemesAsync();
                await LoadActiveThemeAsync();
            }
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    void GenerateNewQuote()
    {
        var quotes = MoodData.MotivationalQuotes;
        var randomIndex = Random.Shared.Next(quotes.Count);
        CurrentQuote = quotes[randomIndex];
    }

    [RelayCommand]
    void ResetMood()
    {
        SelectedMood = null;
        HasSelectedMood = false;
        GenerateNewQuote();
    }

    [RelayCommand]
    void SelectMood(MoodEntry mood)
    {
        SelectedMood = mood;
        HasSelectedMood = true;
        GenerateNewQuote();
    }

    [RelayCommand]
    async Task SetActiveThemeAsync(Theme theme)
    {
        if (theme is null)
        {
            return;
        }

        IsLoading = true;

        try
        {
            var success = await _themeService.SetActiveThemeAsync(theme.Id);
            if (success)
            {
                await LoadActiveThemeAsync();
            }
        }
        finally
        {
            IsLoading = false;
        }
    }

    async Task LoadActiveThemeAsync()
    {
        try
        {
            ActiveTheme = await _themeService.GetActiveThemeAsync();
        }
        catch
        {
            // Handle error silently or log it
        }
    }

    async Task LoadThemesAsync()
    {
        try
        {
            var themes = await _themeService.GetAllThemesAsync();
            Themes.Clear();

            foreach (var theme in themes)
            {
                Themes.Add(theme);
            }
        }
        catch
        {
            // Handle error silently or log it
        }
    }
}
