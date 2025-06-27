using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoodMixer.Models;

namespace MoodMixer.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        public partial string ThemeName { get; set; } = string.Empty;

        [ObservableProperty]
        public partial Color PrimaryColor { get; set; } = Colors.Blue;

        [ObservableProperty]
        public partial Color SecondaryColor { get; set; } = Colors.LightBlue;

        [ObservableProperty]
        public partial Color AccentColor { get; set; } = Colors.Purple;

        [ObservableProperty]
        public partial ObservableCollection<ColorTheme> SavedThemes { get; set; } = new();

        [ObservableProperty]
        public partial bool IsSaving { get; set; }

        [ObservableProperty]
        public partial string StatusMessage { get; set; } = string.Empty;

        [RelayCommand]
        private void RandomizeColors()
        {
            var random = new Random();
            PrimaryColor = Color.FromRgb(
                random.Next(256),
                random.Next(256),
                random.Next(256));

            SecondaryColor = Color.FromRgb(
                random.Next(256),
                random.Next(256),
                random.Next(256));

            AccentColor = Color.FromRgb(
                random.Next(256),
                random.Next(256),
                random.Next(256));

            StatusMessage = "Colors randomized! How do you feel about this combination?";
        }

        [RelayCommand]
        private void SaveTheme()
        {
            if (string.IsNullOrWhiteSpace(ThemeName))
            {
                StatusMessage = "Please enter a name for your theme";
                return;
            }

            IsSaving = true;

            try
            {
                var newTheme = new ColorTheme(ThemeName, PrimaryColor, SecondaryColor, AccentColor);
                SavedThemes.Add(newTheme);
                ThemeName = string.Empty;
                StatusMessage = $"Theme '{newTheme.Name}' saved successfully!";
            }
            finally
            {
                IsSaving = false;
            }
        }

        [RelayCommand]
        private void ApplyTheme(ColorTheme theme)
        {
            if (theme == null)
                return;

            PrimaryColor = theme.PrimaryColor;
            SecondaryColor = theme.SecondaryColor;
            AccentColor = theme.AccentColor;
            StatusMessage = $"Applied theme: {theme.Name}";
        }

        [RelayCommand]
        private void DeleteTheme(ColorTheme theme)
        {
            if (theme == null)
                return;

            SavedThemes.Remove(theme);
            StatusMessage = $"Deleted theme: {theme.Name}";
        }

        public MainViewModel()
        {
            // Add some sample themes
            SavedThemes.Add(new ColorTheme("Ocean Breeze", Colors.DodgerBlue, Colors.SkyBlue, Colors.Navy));
            SavedThemes.Add(new ColorTheme("Sunset Glow", Colors.Orange, Colors.Coral, Colors.DarkRed));
            SavedThemes.Add(new ColorTheme("Forest Fresh", Colors.ForestGreen, Colors.LightGreen, Colors.DarkGreen));
        }
    }
}
