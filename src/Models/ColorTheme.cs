namespace MoodBoost.Models;

public record ColorTheme
{
	public string Name { get; init; } = string.Empty;
	public Color PrimaryColor { get; init; } = Colors.Transparent;
	public Color SecondaryColor { get; init; } = Colors.Transparent;
	public Color AccentColor { get; init; } = Colors.Transparent;
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
