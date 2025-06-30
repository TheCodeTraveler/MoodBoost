using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoodBoost.Models;
using System.Collections.ObjectModel;

namespace MoodBoost.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    public MainPageViewModel()
    {
        Moods = [.. MoodData.Moods];
        TodayDate = DateTimeOffset.Now;
        GenerateNewQuote();
    }

    public ObservableCollection<MoodEntry> Moods { get; }

    [ObservableProperty]
    public partial string CurrentQuote { get; set; } = string.Empty;

    [ObservableProperty]
    public partial MoodEntry? SelectedMood { get; set; }

    [ObservableProperty]
    public partial DateTimeOffset TodayDate { get; set; }

    [ObservableProperty]
    public partial bool HasSelectedMood { get; set; }

    [RelayCommand]
    private void SelectMood(MoodEntry mood)
    {
        SelectedMood = mood;
        HasSelectedMood = true;
        GenerateNewQuote();
    }

    [RelayCommand]
    private void GenerateNewQuote()
    {
        var quotes = MoodData.MotivationalQuotes;
        var randomIndex = Random.Shared.Next(quotes.Count);
        CurrentQuote = quotes[randomIndex];
    }

    [RelayCommand]
    private void ResetMood()
    {
        SelectedMood = null;
        HasSelectedMood = false;
        GenerateNewQuote();
    }
}
