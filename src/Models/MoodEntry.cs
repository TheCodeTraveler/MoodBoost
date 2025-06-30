namespace MoodBoost.Models;

public record MoodEntry(
    string Emoji,
    string Name,
    string Description,
    Color Color
);

public static class MoodData
{
    public static IReadOnlyList<MoodEntry> Moods { get; } = 
    [
        new("😄", "Fantastic", "I'm feeling amazing today!", Color.FromArgb("#4CAF50")),
        new("😊", "Happy", "Things are going well!", Color.FromArgb("#8BC34A")),
        new("😐", "Neutral", "Just an ordinary day", Color.FromArgb("#FFC107")),
        new("😔", "Down", "Not my best day", Color.FromArgb("#FF9800")),
        new("😢", "Sad", "Feeling quite low today", Color.FromArgb("#F44336"))
    ];

    public static IReadOnlyList<string> MotivationalQuotes { get; } = 
    [
        "Every day is a new beginning. Take a deep breath and start again.",
        "You are stronger than you think and more capable than you imagine.",
        "Small steps every day lead to big changes over time.",
        "Your current situation is not your final destination.",
        "Believe in yourself and all that you are.",
        "The only way to do great work is to love what you do.",
        "Success is not final, failure is not fatal: it is the courage to continue that counts.",
        "You don't have to be perfect, just be yourself.",
        "Every moment is a fresh beginning.",
        "You are exactly where you need to be right now."
    ];
}
