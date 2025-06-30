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

    public string SelectedMoodName => SelectedMood?.Name ?? string.Empty;
    public string SelectedMoodEmoji => SelectedMood?.Emoji ?? string.Empty;
    public string SelectedMoodDescription => SelectedMood?.Description ?? string.Empty;
    public bool HasSelectedMood => SelectedMood is not null;

    public ObservableCollection<MoodEntry> Moods { get; }

    [ObservableProperty]
    public partial string CurrentQuote { get; set; } = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SelectedMoodName))]
    [NotifyPropertyChangedFor(nameof(SelectedMoodEmoji))]
    [NotifyPropertyChangedFor(nameof(SelectedMoodDescription))]
    [NotifyPropertyChangedFor(nameof(HasSelectedMood))]
    public partial MoodEntry? SelectedMood { get; set; }

    [ObservableProperty]
    public partial DateTimeOffset TodayDate { get; set; }

    [RelayCommand]
    void SelectMood(MoodEntry mood)
    {
        SelectedMood = mood;
        GenerateNewQuote();
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
        GenerateNewQuote();
    }
}
