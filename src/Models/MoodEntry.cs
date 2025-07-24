using SQLite;

namespace MoodBoost.Models;

public record MoodEntry(
    string Emoji,
    string Name,
    string Description,
    Color Color
);

[Table("Themes")]
public class Theme
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [NotNull]
    public string Name { get; set; } = string.Empty;

    [NotNull]
    public string PrimaryColor { get; set; } = string.Empty;

    [NotNull]
    public string SecondaryColor { get; set; } = string.Empty;

    public string? AccentColor { get; set; }

    public string? BackgroundColor { get; set; }

    public string? TextColor { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public bool IsActive { get; set; } = false;

    // Helper properties for Color objects (not stored in DB)
    [Ignore]
    public Color PrimaryColorValue => Color.FromArgb(PrimaryColor);

    [Ignore]
    public Color SecondaryColorValue => Color.FromArgb(SecondaryColor);

    [Ignore]
    public Color? AccentColorValue => !string.IsNullOrEmpty(AccentColor) ? Color.FromArgb(AccentColor) : null;

    [Ignore]
    public Color? BackgroundColorValue => !string.IsNullOrEmpty(BackgroundColor) ? Color.FromArgb(BackgroundColor) : null;

    [Ignore]
    public Color? TextColorValue => !string.IsNullOrEmpty(TextColor) ? Color.FromArgb(TextColor) : null;
}

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

    // Default themes that will be seeded into the database
    public static IReadOnlyList<Theme> DefaultThemes { get; } =
    [
        new Theme
        {
            Name = "Ocean Breeze",
            PrimaryColor = "#0077BE",
            SecondaryColor = "#00A8CC",
            AccentColor = "#87CEEB",
            BackgroundColor = "#F0F8FF",
            TextColor = "#2C3E50"
        },
        new Theme
        {
            Name = "Sunset Glow",
            PrimaryColor = "#FF6B35",
            SecondaryColor = "#F7931E",
            AccentColor = "#FFD700",
            BackgroundColor = "#FFF8DC",
            TextColor = "#8B4513"
        },
        new Theme
        {
            Name = "Forest Green",
            PrimaryColor = "#228B22",
            SecondaryColor = "#32CD32",
            AccentColor = "#90EE90",
            BackgroundColor = "#F0FFF0",
            TextColor = "#006400"
        },
        new Theme
        {
            Name = "Purple Dreams",
            PrimaryColor = "#8A2BE2",
            SecondaryColor = "#9370DB",
            AccentColor = "#DDA0DD",
            BackgroundColor = "#F8F0FF",
            TextColor = "#4B0082"
        },
        new Theme
        {
            Name = "Rose Gold",
            PrimaryColor = "#E91E63",
            SecondaryColor = "#F06292",
            AccentColor = "#FCE4EC",
            BackgroundColor = "#FFF0F5",
            TextColor = "#880E4F"
        }
    ];
}
