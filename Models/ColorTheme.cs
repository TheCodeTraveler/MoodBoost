using System;

namespace MoodMixer.Models
{
    public record ColorTheme
    {
        public string Name { get; init; }
        public Color PrimaryColor { get; init; }
        public Color SecondaryColor { get; init; }
        public Color AccentColor { get; init; }
        public DateTimeOffset CreatedAt { get; init; }

        public ColorTheme()
        {
            CreatedAt = DateTimeOffset.Now;
        }

        public ColorTheme(string name, Color primaryColor, Color secondaryColor, Color accentColor)
        {
            Name = name;
            PrimaryColor = primaryColor;
            SecondaryColor = secondaryColor;
            AccentColor = accentColor;
            CreatedAt = DateTimeOffset.Now;
        }
    }
}
