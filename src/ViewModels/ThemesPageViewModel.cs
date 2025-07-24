using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoodBoost.Models;
using MoodBoost.Services;
using System.Collections.ObjectModel;

namespace MoodBoost.ViewModels;

public partial class ThemesPageViewModel : ObservableObject
{
	readonly IThemeService _themeService;

	public ThemesPageViewModel(IThemeService themeService)
	{
		_themeService = themeService;
		Themes = [];
	}

	public ObservableCollection<Theme> Themes { get; }

	[ObservableProperty]
	public partial Theme? ActiveTheme { get; set; }

	[ObservableProperty]
	public partial bool IsLoading { get; set; }

	[ObservableProperty]
	public partial string NewThemeName { get; set; } = string.Empty;

	public async Task InitializeAsync()
	{
		await LoadThemesAsync();
		await LoadActiveThemeAsync();
	}

	[RelayCommand]
async Task CreateThemeAsync()
	{
		if (string.IsNullOrWhiteSpace(NewThemeName))
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
				"#2196F3", "#03DAC6", "#9C27B0", "#E91E63", "#FF5722",
				"#795548", "#607D8B", "#FF9800", "#3F51B5", "#009688"
			};

			var primaryColor = colors[random.Next(colors.Length)];
			var secondaryColor = colors[random.Next(colors.Length)];
			var accentColor = colors[random.Next(colors.Length)];

			await _themeService.CreateThemeAsync(NewThemeName, primaryColor, secondaryColor, accentColor);
			NewThemeName = string.Empty;
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
				await LoadThemesAsync(); // Refresh to update IsActive status
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
