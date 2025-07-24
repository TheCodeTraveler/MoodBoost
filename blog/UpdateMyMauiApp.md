## Hey Q! Add A New Feature to My Mobile App

Welcome to the **Hey Q!** blog series where we build fun apps using [Amazon Q Developer](https://docs.aws.amazon.com/amazonq/latest/qdeveloper-ug/command-line.html?trk=26a307dd-f6c6-4133-a99f-0388d1304aef&sc_channel=el) and learn about the code along the way! The scope of the sample apps created in this series are small 1-2 page applications that are fun, well designed and properly architected following best practices. And, the best part, the code is all open-source allowing you to grab it and use it for your apps! Here's the link to the completed open-source project: https://github.com/TheCodeTraveler/MoodBoost

This week we are updating an existing mobile app built in .NET MAUI. For this app, I have a few goals:
1. The app must compile and run on Windows Desktop
2. The app must have a nice, highly polished, professional User Interface (UI)
3. The app must have a fun name that relates to its use
4. The code must be created by first using the latest version of the **WPF** template in the latest version of Visual Studio
5. The code must use the MVVM Architecture
6. The code must follow best practices
7. The code must use the most-recent versions for all dependencies

## The Completed App

**Note:** If you're interested in learning more about prompt engineering and want to see the steps I used to guide Q Developer CLI, scroll down to the Appendix.

### Things to watch out for

1. Used Null Forgiving Operator
2. Did not `await` Tasks
3. Used the `_ = Task.Run()` pattern 
4. Nested layouts (Grid inside of a Grid)

## Appendix

For anyone out there who is interested in learning more about prompt engineering, this section contains the steps I followed to create this app.

**Note:** Responses from LLMs are non-deterministic which can result in different outputs despite asking the same question. You may receive slightly different responses from Q Developer when trying to replicate the same steps below.

I've [already installed Q Developer CLI](https://docs.aws.amazon.com/amazonq/latest/qdeveloper-ug/command-line-installing.html?trk=26a307dd-f6c6-4133-a99f-0388d1304aef&sc_channel=el) on my Mac, so let's begin by opening the [macOS Terminal](https://support.apple.com/guide/terminal/welcome/mac) and begin chatting with Q using the following command:

```
> q chat
```

Next, I will help Q out a bit by giving Q a profile. This reduces the scope of the datasets that Q will reference helps Q better understand the problem we are trying to solve together:

**Prompt**
> Hey Q! You are a senior. NET MAUI Developer who is an expert in C# and always follows C#, Mobile and .NET MAUI best practices. You always reference the official C# documentation on https://learn.microsoft.com/dotnet/csharp/ when writing your code and you always reference the official .NET MAUI documentation when writing your code https://learn.microsoft.com/dotnet/maui/. You always write code the uses the recommended libraries `CommunityToolkit.MVVM` and `CommunityToolkit.Maui`. The User Interface for your apps are always highly polished and professional, giving the appearance that it was created by a Senior Designer. In addition to the reference domains above, you are also allowed to reference the open-source NET runtime code on GitHub, https://github.com/dotnet/runtime, and the open-source .NET MAUI code on GitHub https://github.com/dotnet/maui.

**Prompt**
> In the current folder is the source code for a .NET MAUI mobile app called MoodBoost. I'd like to update this app to add local storage using the sqlite-net-pcl NuGet Pacakge: https://www.nuget.org/packages/sqlite-net-pcl/. This new database will save every theme created locally to the device and will load all available themes when the app launches

**Respose**
> I'll help you add SQLite local storage to your MoodBoost app using sqlite-net-pcl. Let me first examine the current project 
structure to understand how it's organized and then implement the database functionality. Perfect! Now I understand the current MoodBoost app structure. I can see it's a mood tracking app that allows users to select their current mood and displays motivational quotes. Now I'll implement SQLite local storage to save and load themes. Let me start by updating the project file to include the sqlite-net-pcl package:


```xml
  42, 42: 	<ItemGroup>
  43, 43: 		<PackageReference Include="Microsoft.Extensions.Logging.Debug" Version="9.0.6" />
  44, 44: 		<PackageReference Include="CommunityToolkit.Mvvm" Version="8.4.0" />
  45, 45: 		<PackageReference Include="CommunityToolkit.Maui" Version="12.0.0" />
+     46: 		<PackageReference Include="sqlite-net-pcl" Version="1.9.172" />
+     47: 		<PackageReference Include="SQLitePCLRaw.bundle_green" Version="2.1.10" />
  46, 48: 	</ItemGroup>
```

Great! It looks like it has added the latest version of the correct NuGet Packages. Approved ✅

**Response**
> Now I'll create a Theme model that can be stored in SQLite. I'll update the existing MoodEntry.cs file to include theme 
functionality:

```cs
+       1: using SQLite;
+       2: 
   1,   3: namespace MoodBoost.Models;
   2,   4: 
   3,   5: public record MoodEntry(
   4,   6:     string Emoji,
   5,   7:     string Name,
   6,   8:     string Description,
   7,   9:     Color Color
   8,  10: );
   9,  11: 
+      12: [Table("Themes")]
+      13: public class Theme
+      14: {
+      15:     [PrimaryKey, AutoIncrement]
+      16:     public int Id { get; set; }
+      17: 
+      18:     [NotNull]
+      19:     public string Name { get; set; } = string.Empty;
+      20: 
+      21:     [NotNull]
+      22:     public string PrimaryColor { get; set; } = string.Empty;
+      23: 
+      24:     [NotNull]
+      25:     public string SecondaryColor { get; set; } = string.Empty;
+      26: 
+      27:     public string? AccentColor { get; set; }
+      28: 
+      29:     public string? BackgroundColor { get; set; }
+      30: 
+      31:     public string? TextColor { get; set; }
+      32: 
+      33:     public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
+      34: 
+      35:     public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
+      36: 
+      37:     public bool IsActive { get; set; } = false;
+      38: 
+      39:     // Helper properties for Color objects (not stored in DB)
+      40:     [Ignore]
+      41:     public Color PrimaryColorValue => Color.FromArgb(PrimaryColor);
+      42: 
+      43:     [Ignore]
+      44:     public Color SecondaryColorValue => Color.FromArgb(SecondaryColor);
+      45: 
+      46:     [Ignore]
+      47:     public Color? AccentColorValue => !string.IsNullOrEmpty(AccentColor) ? Color.FromArgb(AccentColor) : null;
+      48: 
+      49:     [Ignore]
+      50:     public Color? BackgroundColorValue => !string.IsNullOrEmpty(BackgroundColor) ? Color.FromArgb(BackgroundColor) : null;
+      51: 
+      52:     [Ignore]
+      53:     public Color? TextColorValue => !string.IsNullOrEmpty(TextColor) ? Color.FromArgb(TextColor) : null;
+      54: }
+      55: 
  10,  56: public static class MoodData
  11,  57: {
  12,  58:     public static IReadOnlyList<MoodEntry> Moods { get; } =
  13,  59:     [
  14,  60:         new("😄", "Fantastic", "I'm feeling amazing today!", Color.FromArgb("#4CAF50")),
  15,  61:         new("😊", "Happy", "Things are going well!", Color.FromArgb("#8BC34A")),
  16,  62:         new("😐", "Neutral", "Just an ordinary day", Color.FromArgb("#FFC107")),
  17,  63:         new("😔", "Down", "Not my best day", Color.FromArgb("#FF9800")),
  18,  64:         new("😢", "Sad", "Feeling quite low today", Color.FromArgb("#F44336"))
  19,  65:     ];
  20,  66: 
  21,  67:     public static IReadOnlyList<string> MotivationalQuotes { get; } =
  22,  68:     [
  23,  69:         "Every day is a new beginning. Take a deep breath and start again.",
  24,  70:         "You are stronger than you think and more capable than you imagine.",
  25,  71:         "Small steps every day lead to big changes over time.",
  26,  72:         "Your current situation is not your final destination.",
  27,  73:         "Believe in yourself and all that you are.",
  28,  74:         "The only way to do great work is to love what you do.",
  29,  75:         "Success is not final, failure is not fatal: it is the courage to continue that counts.",
  30,  76:         "You don't have to be perfect, just be yourself.",
  31,  77:         "Every moment is a fresh beginning.",
  32,  78:         "You are exactly where you need to be right now."
  33,  79:     ];
+      80: 
+      81:     // Default themes that will be seeded into the database
+      82:     public static IReadOnlyList<Theme> DefaultThemes { get; } =
+      83:     [
+      84:         new Theme
+      85:         {
+      86:             Name = "Ocean Breeze",
+      87:             PrimaryColor = "#0077BE",
+      88:             SecondaryColor = "#00A8CC",
+      89:             AccentColor = "#87CEEB",
+      90:             BackgroundColor = "#F0F8FF",
+      91:             TextColor = "#2C3E50"
+      92:         },
+      93:         new Theme
+      94:         {
+      95:             Name = "Sunset Glow",
+      96:             PrimaryColor = "#FF6B35",
+      97:             SecondaryColor = "#F7931E",
+      98:             AccentColor = "#FFD700",
+      99:             BackgroundColor = "#FFF8DC",
+     100:             TextColor = "#8B4513"
+     101:         },
+     102:         new Theme
+     103:         {
+     104:             Name = "Forest Green",
+     105:             PrimaryColor = "#228B22",
+     106:             SecondaryColor = "#32CD32",
+     107:             AccentColor = "#90EE90",
+     108:             BackgroundColor = "#F0FFF0",
+     109:             TextColor = "#006400"
+     110:         },
+     111:         new Theme
+     112:         {
+     113:             Name = "Purple Dreams",
+     114:             PrimaryColor = "#8A2BE2",
+     115:             SecondaryColor = "#9370DB",
+     116:             AccentColor = "#DDA0DD",
+     117:             BackgroundColor = "#F8F0FF",
+     118:             TextColor = "#4B0082"
+     119:         },
+     120:         new Theme
+     121:         {
+     122:             Name = "Rose Gold",
+     123:             PrimaryColor = "#E91E63",
+     124:             SecondaryColor = "#F06292",
+     125:             AccentColor = "#FCE4EC",
+     126:             BackgroundColor = "#FFF0F5",
+     127:             TextColor = "#880E4F"
+     128:         }
+     129:     ];
  34, 130: }
```

Approved ✅

Next, it created our new Database Service:


```cs
+      1: using MoodBoost.Models;
+      2: using SQLite;
+      3: 
+      4: namespace MoodBoost.Services;
+      5: 
+      6: public interface IDatabaseService
+      7: {
+      8:     Task InitializeAsync();
+      9:     Task<List<Theme>> GetAllThemesAsync();
+     10:     Task<Theme?> GetActiveThemeAsync();
+     11:     Task<int> SaveThemeAsync(Theme theme);
+     12:     Task<int> DeleteThemeAsync(Theme theme);
+     13:     Task<int> SetActiveThemeAsync(int themeId);
+     14: }
+     15: 
+     16: public class DatabaseService : IDatabaseService
+     17: {
+     18:     private readonly string _databasePath;
+     19:     private SQLiteAsyncConnection? _database;
+     20: 
+     21:     public DatabaseService()
+     22:     {
+     23:         _databasePath = Path.Combine(FileSystem.AppDataDirectory, "MoodBoost.db3");
+     24:     }
+     25: 
+     26:     public async Task InitializeAsync()
+     27:     {
+     28:         if (_database is not null)
+     29:         {
+     30:             return;
+     31:         }
+     32: 
+     33:         _database = new SQLiteAsyncConnection(_databasePath);
+     34:         await _database.CreateTableAsync<Theme>();
+     35:         await SeedDefaultThemesAsync();
+     36:     }
+     37: 
+     38:     public async Task<List<Theme>> GetAllThemesAsync()
+     39:     {
+     40:         await InitializeAsync();
+     41:         return await _database!.Table<Theme>().OrderBy(t => t.Name).ToListAsync();
+     42:     }
+     43: 
+     44:     public async Task<Theme?> GetActiveThemeAsync()
+     45:     {
+     46:         await InitializeAsync();
+     47:         return await _database!.Table<Theme>().Where(t => t.IsActive).FirstOrDefaultAsync();
+     48:     }
+     49: 
+     50:     public async Task<int> SaveThemeAsync(Theme theme)
+     51:     {
+     52:         await InitializeAsync();
+     53:         
+     54:         theme.UpdatedAt = DateTime.UtcNow;
+     55:         
+     56:         if (theme.Id != 0)
+     57:         {
+     58:             return await _database!.UpdateAsync(theme);
+     59:         }
+     60:         else
+     61:         {
+     62:             theme.CreatedAt = DateTime.UtcNow;
+     63:             return await _database!.InsertAsync(theme);
+     64:         }
+     65:     }
+     66: 
+     67:     public async Task<int> DeleteThemeAsync(Theme theme)
+     68:     {
+     69:         await InitializeAsync();
+     70:         return await _database!.DeleteAsync(theme);
+     71:     }
+     72: 
+     73:     public async Task<int> SetActiveThemeAsync(int themeId)
+     74:     {
+     75:         await InitializeAsync();
+     76:         
+     77:         // First, deactivate all themes
+     78:         await _database!.ExecuteAsync("UPDATE Themes SET IsActive = 0");
+     79:         
+     80:         // Then activate the selected theme
+     81:         return await _database!.ExecuteAsync("UPDATE Themes SET IsActive = 1 WHERE Id = ?", themeId);
+     82:     }
+     83: 
+     84:     private async Task SeedDefaultThemesAsync()
+     85:     {
+     86:         var existingThemes = await _database!.Table<Theme>().CountAsync();
+     87:         
+     88:         if (existingThemes == 0)
+     89:         {
+     90:             var defaultThemes = MoodData.DefaultThemes.ToList();
+     91:             
+     92:             // Set the first theme as active
+     93:             if (defaultThemes.Count > 0)
+     94:             {
+     95:                 defaultThemes[0].IsActive = true;
+     96:             }
+     97:             
+     98:             await _database.InsertAllAsync(defaultThemes);
+     99:         }
+    100:     }
+    101: }
```

Approved ✅

Next it created a ThemeService:

```cs
+      1: using MoodBoost.Models;
+      2: 
+      3: namespace MoodBoost.Services;
+      4: 
+      5: public interface IThemeService
+      6: {
+      7:     Task<List<Theme>> GetAllThemesAsync();
+      8:     Task<Theme?> GetActiveThemeAsync();
+      9:     Task<Theme> CreateThemeAsync(string name, string primaryColor, string secondaryColor, string? accentColor = null, string? backgroundColor = null, string? textColor = null);
+     10:     Task<bool> SetActiveThemeAsync(int themeId);
+     11:     Task<bool> DeleteThemeAsync(int themeId);
+     12:     Task<bool> UpdateThemeAsync(Theme theme);
+     13: }
+     14: 
+     15: public class ThemeService : IThemeService
+     16: {
+     17:     private readonly IDatabaseService _databaseService;
+     18: 
+     19:     public ThemeService(IDatabaseService databaseService)
+     20:     {
+     21:         _databaseService = databaseService;
+     22:     }
+     23: 
+     24:     public async Task<List<Theme>> GetAllThemesAsync()
+     25:     {
+     26:         return await _databaseService.GetAllThemesAsync();
+     27:     }
+     28: 
+     29:     public async Task<Theme?> GetActiveThemeAsync()
+     30:     {
+     31:         return await _databaseService.GetActiveThemeAsync();
+     32:     }
+     33: 
+     34:     public async Task<Theme> CreateThemeAsync(string name, string primaryColor, string secondaryColor, string? accentColor = null, string? backgroundColor = null, string? textColor = null)
+     35:     {
+     36:         var theme = new Theme
+     37:         {
+     38:             Name = name,
+     39:             PrimaryColor = primaryColor,
+     40:             SecondaryColor = secondaryColor,
+     41:             AccentColor = accentColor,
+     42:             BackgroundColor = backgroundColor,
+     43:             TextColor = textColor,
+     44:             CreatedAt = DateTime.UtcNow,
+     45:             UpdatedAt = DateTime.UtcNow,
+     46:             IsActive = false
+     47:         };
+     48: 
+     49:         await _databaseService.SaveThemeAsync(theme);
+     50:         return theme;
+     51:     }
+     52: 
+     53:     public async Task<bool> SetActiveThemeAsync(int themeId)
+     54:     {
+     55:         try
+     56:         {
+     57:             await _databaseService.SetActiveThemeAsync(themeId);
+     58:             return true;
+     59:         }
+     60:         catch
+     61:         {
+     62:             return false;
+     63:         }
+     64:     }
+     65: 
+     66:     public async Task<bool> DeleteThemeAsync(int themeId)
+     67:     {
+     68:         try
+     69:         {
+     70:             var themes = await _databaseService.GetAllThemesAsync();
+     71:             var themeToDelete = themes.FirstOrDefault(t => t.Id == themeId);
+     72:             
+     73:             if (themeToDelete is null)
+     74:             {
+     75:                 return false;
+     76:             }
+     77: 
+     78:             // Don't allow deletion if it's the only theme or if it's active
+     79:             if (themes.Count <= 1 || themeToDelete.IsActive)
+     80:             {
+     81:                 return false;
+     82:             }
+     83: 
+     84:             await _databaseService.DeleteThemeAsync(themeToDelete);
+     85:             return true;
+     86:         }
+     87:         catch
+     88:         {
+     89:             return false;
+     90:         }
+     91:     }
+     92: 
+     93:     public async Task<bool> UpdateThemeAsync(Theme theme)
+     94:     {
+     95:         try
+     96:         {
+     97:             await _databaseService.SaveThemeAsync(theme);
+     98:             return true;
+     99:         }
+    100:         catch
+    101:         {
+    102:             return false;
+    103:         }
+    104:     }
+    105: }
```

Next, it updated `MauiProgram.cs` to register the new services:

```cs
   1,  1: using CommunityToolkit.Maui;
   2,  2: using Microsoft.Extensions.Logging;
+      3: using MoodBoost.Services;
   3,  4: using MoodBoost.ViewModels;
   4,  5: using MoodBoost.Views;
   5,  6: 
   6,  7: namespace MoodBoost;
   7,  8: 
   8,  9: public static class MauiProgram
   9, 10: {
  10, 11:     public static MauiApp CreateMauiApp()
  11, 12:     {
  12, 13:         var builder = MauiApp.CreateBuilder();
  13, 14:         builder
  14, 15:             .UseMauiApp<App>()
  15, 16:             .UseMauiCommunityToolkit()
  16, 17:             .ConfigureFonts(fonts =>
  17, 18:             {
  18, 19:                 fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
  19, 20:                 fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
  20, 21:             });
  21, 22: 
+     23:         // Register Services
+     24:         builder.Services.AddSingleton<IDatabaseService, DatabaseService>();
+     25:         builder.Services.AddSingleton<IThemeService, ThemeService>();
+     26: 
  22, 27:         // Register ViewModels and Views
  23, 28:         builder.Services.AddSingleton<MainPageViewModel>();
  24, 29:         builder.Services.AddSingleton<MainPage>();
  25, 30: 
  26, 31: #if DEBUG
  27, 32:         builder.Services.AddLogging(configure => configure.AddDebug());
  28, 33: #endif
  29, 34: 
  30, 35:         return builder.Build();
  31, 36:     }
  32, 37: }
```

Next, it updated MainViewModel to add the new Theme functionality:
```cs
   1,   1: using CommunityToolkit.Mvvm.ComponentModel;
   2,   2: using CommunityToolkit.Mvvm.Input;
   3,   3: using MoodBoost.Models;
+       4: using MoodBoost.Services;
   4,   5: using System.Collections.ObjectModel;
   5,   6: 
   6,   7: namespace MoodBoost.ViewModels;
   7,   8: 
   8,   9: public partial class MainPageViewModel : ObservableObject
   9,  10: {
- 10     :     public MainPageViewModel()
+      11:     private readonly IThemeService _themeService;
+      12: 
+      13:     public MainPageViewModel(IThemeService themeService)
  11,  14:     {
+      15:         _themeService = themeService;
  12,  16:         Moods = [.. MoodData.Moods];
+      17:         Themes = [];
  13,  18:         TodayDate = DateTimeOffset.Now;
  14,  19:         GenerateNewQuote();
  15,  20:     }
  16,  21: 
  17,  22:     public ObservableCollection<MoodEntry> Moods { get; }
  18,  23: 
+      24:     public ObservableCollection<Theme> Themes { get; }
+      25: 
  19,  26:     [ObservableProperty]
+      27:     public partial Theme? ActiveTheme { get; set; }
+      28: 
+      29:     [ObservableProperty]
  20,  30:     public partial string CurrentQuote { get; set; } = string.Empty;
  21,  31: 
  22,  32:     [ObservableProperty]
  23,  33:     public partial bool HasSelectedMood { get; set; }
  24,  34: 
  25,  35:     [ObservableProperty]
+      36:     public partial bool IsLoading { get; set; }
+      37: 
+      38:     [ObservableProperty]
  26,  39:     public partial MoodEntry? SelectedMood { get; set; }
  27,  40: 
  28,  41:     [ObservableProperty]
  29,  42:     public partial DateTimeOffset TodayDate { get; set; }
  30,  43: 
+      44:     public async Task InitializeAsync()
+      45:     {
+      46:         await LoadThemesAsync();
+      47:         await LoadActiveThemeAsync();
+      48:     }
+      49: 
+      50:     [RelayCommand]
+      51:     private async Task CreateThemeAsync(string themeName)
+      52:     {
+      53:         if (string.IsNullOrWhiteSpace(themeName))
+      54:         {
+      55:             return;
+      56:         }
+      57: 
+      58:         IsLoading = true;
+      59:         
+      60:         try
+      61:         {
+      62:             // Generate random colors for the new theme
+      63:             var random = Random.Shared;
+      64:             var colors = new[]
+      65:             {
+      66:                 "#FF6B35", "#F7931E", "#FFD700", "#4CAF50", "#8BC34A",
+      67:                 "#2196F3", "#03DAC6", "#9C27B0", "#E91E63", "#FF5722"
+      68:             };
+      69:             
+      70:             var primaryColor = colors[random.Next(colors.Length)];
+      71:             var secondaryColor = colors[random.Next(colors.Length)];
+      72:             
+      73:             await _themeService.CreateThemeAsync(themeName, primaryColor, secondaryColor);
+      74:             await LoadThemesAsync();
+      75:         }
+      76:         finally
+      77:         {
+      78:             IsLoading = false;
+      79:         }
+      80:     }
+      81: 
+      82:     [RelayCommand]
+      83:     private async Task DeleteThemeAsync(Theme theme)
+      84:     {
+      85:         if (theme is null)
+      86:         {
+      87:             return;
+      88:         }
+      89: 
+      90:         IsLoading = true;
+      91:         
+      92:         try
+      93:         {
+      94:             var success = await _themeService.DeleteThemeAsync(theme.Id);
+      95:             if (success)
+      96:             {
+      97:                 await LoadThemesAsync();
+      98:                 await LoadActiveThemeAsync();
+      99:             }
+     100:         }
+     101:         finally
+     102:         {
+     103:             IsLoading = false;
+     104:         }
+     105:     }
+     106: 
  31, 107:     [RelayCommand]
  32, 108:     private void GenerateNewQuote()
  33, 109:     {
  34, 110:         var quotes = MoodData.MotivationalQuotes;
  35, 111:         var randomIndex = Random.Shared.Next(quotes.Count);
  36, 112:         CurrentQuote = quotes[randomIndex];
  37, 113:     }
  38, 114: 
  39, 115:     [RelayCommand]
  40, 116:     private void ResetMood()
  41, 117:     {
  42, 118:         SelectedMood = null;
  43, 119:         HasSelectedMood = false;
  44, 120:         GenerateNewQuote();
  45, 121:     }
  46, 122: 
  47, 123:     [RelayCommand]
  48, 124:     private void SelectMood(MoodEntry mood)
  49, 125:     {
  50, 126:         SelectedMood = mood;
  51, 127:         HasSelectedMood = true;
  52, 128:         GenerateNewQuote();
  53, 129:     }
+     130: 
+     131:     [RelayCommand]
+     132:     private async Task SetActiveThemeAsync(Theme theme)
+     133:     {
+     134:         if (theme is null)
+     135:         {
+     136:             return;
+     137:         }
+     138: 
+     139:         IsLoading = true;
+     140:         
+     141:         try
+     142:         {
+     143:             var success = await _themeService.SetActiveThemeAsync(theme.Id);
+     144:             if (success)
+     145:             {
+     146:                 await LoadActiveThemeAsync();
+     147:             }
+     148:         }
+     149:         finally
+     150:         {
+     151:             IsLoading = false;
+     152:         }
+     153:     }
+     154: 
+     155:     private async Task LoadActiveThemeAsync()
+     156:     {
+     157:         try
+     158:         {
+     159:             ActiveTheme = await _themeService.GetActiveThemeAsync();
+     160:         }
+     161:         catch
+     162:         {
+     163:             // Handle error silently or log it
+     164:         }
+     165:     }
+     166: 
+     167:     private async Task LoadThemesAsync()
+     168:     {
+     169:         try
+     170:         {
+     171:             var themes = await _themeService.GetAllThemesAsync();
+     172:             Themes.Clear();
+     173:             
+     174:             foreach (var theme in themes)
+     175:             {
+     176:                 Themes.Add(theme);
+     177:             }
+     178:         }
+     179:         catch
+     180:         {
+     181:             // Handle error silently or log it
+     182:         }
+     183:     }
  54, 184: }
```

Next, it updated our MainPage.xml.cs

```cs
   1,  1: using MoodBoost.ViewModels;
   2,  2: 
   3,  3: namespace MoodBoost.Views;
   4,  4: 
   5,  5: public partial class MainPage : ContentPage
   6,  6: {
+      7:     private readonly MainPageViewModel _viewModel;
+      8: 
   7,  9:     public MainPage(MainPageViewModel viewModel)
   8, 10:     {
   9, 11:         InitializeComponent();
- 10    :         BindingContext = viewModel;
+     12:         _viewModel = viewModel;
+     13:         BindingContext = _viewModel;
+     14:     }
+     15: 
+     16:     protected override async void OnAppearing()
+     17:     {
+     18:         base.OnAppearing();
+     19:         await _viewModel.InitializeAsync();
  11, 20:     }
  12, 21: }
```

Next, it created a ThemesPageViewModel:
```cs
+      1: using CommunityToolkit.Mvvm.ComponentModel;
+      2: using CommunityToolkit.Mvvm.Input;
+      3: using MoodBoost.Models;
+      4: using MoodBoost.Services;
+      5: using System.Collections.ObjectModel;
+      6: 
+      7: namespace MoodBoost.ViewModels;
+      8: 
+      9: public partial class ThemesPageViewModel : ObservableObject
+     10: {
+     11:     private readonly IThemeService _themeService;
+     12: 
+     13:     public ThemesPageViewModel(IThemeService themeService)
+     14:     {
+     15:         _themeService = themeService;
+     16:         Themes = [];
+     17:     }
+     18: 
+     19:     public ObservableCollection<Theme> Themes { get; }
+     20: 
+     21:     [ObservableProperty]
+     22:     public partial Theme? ActiveTheme { get; set; }
+     23: 
+     24:     [ObservableProperty]
+     25:     public partial bool IsLoading { get; set; }
+     26: 
+     27:     [ObservableProperty]
+     28:     public partial string NewThemeName { get; set; } = string.Empty;
+     29: 
+     30:     public async Task InitializeAsync()
+     31:     {
+     32:         await LoadThemesAsync();
+     33:         await LoadActiveThemeAsync();
+     34:     }
+     35: 
+     36:     [RelayCommand]
+     37:     private async Task CreateThemeAsync()
+     38:     {
+     39:         if (string.IsNullOrWhiteSpace(NewThemeName))
+     40:         {
+     41:             return;
+     42:         }
+     43: 
+     44:         IsLoading = true;
+     45:         
+     46:         try
+     47:         {
+     48:             // Generate random colors for the new theme
+     49:             var random = Random.Shared;
+     50:             var colors = new[]
+     51:             {
+     52:                 "#FF6B35", "#F7931E", "#FFD700", "#4CAF50", "#8BC34A",
+     53:                 "#2196F3", "#03DAC6", "#9C27B0", "#E91E63", "#FF5722",
+     54:                 "#795548", "#607D8B", "#FF9800", "#3F51B5", "#009688"
+     55:             };
+     56:             
+     57:             var primaryColor = colors[random.Next(colors.Length)];
+     58:             var secondaryColor = colors[random.Next(colors.Length)];
+     59:             var accentColor = colors[random.Next(colors.Length)];
+     60:             
+     61:             await _themeService.CreateThemeAsync(NewThemeName, primaryColor, secondaryColor, accentColor);
+     62:             NewThemeName = string.Empty;
+     63:             await LoadThemesAsync();
+     64:         }
+     65:         finally
+     66:         {
+     67:             IsLoading = false;
+     68:         }
+     69:     }
+     70: 
+     71:     [RelayCommand]
+     72:     private async Task DeleteThemeAsync(Theme theme)
+     73:     {
+     74:         if (theme is null)
+     75:         {
+     76:             return;
+     77:         }
+     78: 
+     79:         IsLoading = true;
+     80:         
+     81:         try
+     82:         {
+     83:             var success = await _themeService.DeleteThemeAsync(theme.Id);
+     84:             if (success)
+     85:             {
+     86:                 await LoadThemesAsync();
+     87:                 await LoadActiveThemeAsync();
+     88:             }
+     89:         }
+     90:         finally
+     91:         {
+     92:             IsLoading = false;
+     93:         }
+     94:     }
+     95: 
+     96:     [RelayCommand]
+     97:     private async Task SetActiveThemeAsync(Theme theme)
+     98:     {
+     99:         if (theme is null)
+    100:         {
+    101:             return;
+    102:         }
+    103: 
+    104:         IsLoading = true;
+    105:         
+    106:         try
+    107:         {
+    108:             var success = await _themeService.SetActiveThemeAsync(theme.Id);
+    109:             if (success)
+    110:             {
+    111:                 await LoadActiveThemeAsync();
+    112:                 await LoadThemesAsync(); // Refresh to update IsActive status
+    113:             }
+    114:         }
+    115:         finally
+    116:         {
+    117:             IsLoading = false;
+    118:         }
+    119:     }
+    120: 
+    121:     private async Task LoadActiveThemeAsync()
+    122:     {
+    123:         try
+    124:         {
+    125:             ActiveTheme = await _themeService.GetActiveThemeAsync();
+    126:         }
+    127:         catch
+    128:         {
+    129:             // Handle error silently or log it
+    130:         }
+    131:     }
+    132: 
+    133:     private async Task LoadThemesAsync()
+    134:     {
+    135:         try
+    136:         {
+    137:             var themes = await _themeService.GetAllThemesAsync();
+    138:             Themes.Clear();
+    139:             
+    140:             foreach (var theme in themes)
+    141:             {
+    142:                 Themes.Add(theme);
+    143:             }
+    144:         }
+    145:         catch
+    146:         {
+    147:             // Handle error silently or log it
+    148:         }
+    149:     }
+    150: }
```

Next it created a new Themes Page:

```cs
+     1: using MoodBoost.ViewModels;
+     2: 
+     3: namespace MoodBoost.Views;
+     4: 
+     5: public partial class ThemesPage : ContentPage
+     6: {
+     7:     private readonly ThemesPageViewModel _viewModel;
+     8: 
+     9:     public ThemesPage(ThemesPageViewModel viewModel)
+    10:     {
+    11:         InitializeComponent();
+    12:         _viewModel = viewModel;
+    13:         BindingContext = _viewModel;
+    14:     }
+    15: 
+    16:     protected override async void OnAppearing()
+    17:     {
+    18:         base.OnAppearing();
+    19:         await _viewModel.InitializeAsync();
+    20:     }
+    21: }
```

```xml
+      1: <?xml version="1.0" encoding="utf-8" ?>
+      2: <ContentPage x:Class="MoodBoost.Views.ThemesPage"
+      3:              xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
+      4:              xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
+      5:              xmlns:toolkit="http://schemas.microsoft.com/dotnet/2022/maui/toolkit"
+      6:              xmlns:vm="clr-namespace:MoodBoost.ViewModels"
+      7:              x:DataType="vm:ThemesPageViewModel"
+      8:              Title="Themes"
+      9:              BackgroundColor="{StaticResource Primary}">
+     10: 
+     11:     <ScrollView>
+     12:         <Grid RowDefinitions="Auto,Auto,Auto,Auto,Auto,Auto,Auto,*" 
+     13:               ColumnDefinitions="*,*,*"
+     14:               Padding="20"
+     15:               RowSpacing="20"
+     16:               ColumnSpacing="10">
+     17: 
+     18:             <!-- Page Title -->
+     19:             <Label Grid.Row="0"
+     20:                    Grid.ColumnSpan="3"
+     21:                    Text="Customize Your Theme" 
+     22:                    FontSize="28" 
+     23:                    FontAttributes="Bold"
+     24:                    TextColor="White"
+     25:                    HorizontalOptions="Center" />
+     26: 
+     27:             <!-- Create New Theme Section Background -->
+     28:             <Border Grid.Row="1"
+     29:                     Grid.ColumnSpan="3"
+     30:                     BackgroundColor="White" 
+     31:                     StrokeThickness="0"
+     32:                     Stroke="Transparent">
+     33:                 <Border.StrokeShape>
+     34:                     <RoundRectangle CornerRadius="20" />
+     35:                 </Border.StrokeShape>
+     36:             </Border>
+     37: 
+     38:             <!-- Create New Theme Title -->
+     39:             <Label Grid.Row="1"
+     40:                    Grid.ColumnSpan="3"
+     41:                    Text="Create New Theme" 
+     42:                    FontSize="18" 
+     43:                    FontAttributes="Bold"
+     44:                    TextColor="{StaticResource Gray900}"
+     45:                    HorizontalOptions="Center"
+     46:                    VerticalOptions="Start"
+     47:                    Margin="20,20,20,0" />
+     48: 
+     49:             <!-- Theme Name Entry -->
+     50:             <Entry Grid.Row="2"
+     51:                    Grid.ColumnSpan="3"
+     52:                    Text="{Binding NewThemeName}"
+     53:                    Placeholder="Enter theme name..."
+     54:                    BackgroundColor="{StaticResource Gray100}"
+     55:                    TextColor="{StaticResource Gray900}"
+     56:                    PlaceholderColor="{StaticResource Gray600}"
+     57:                    Margin="20,0,20,0" />
+     58: 
+     59:             <!-- Create Theme Button -->
+     60:             <Button Grid.Row="3"
+     61:                     Grid.ColumnSpan="3"
+     62:                     Text="Create Theme"
+     63:                     Command="{Binding CreateThemeCommand}"
+     64:                     BackgroundColor="{StaticResource Secondary}"
+     65:                     TextColor="White"
+     66:                     CornerRadius="15"
+     67:                     FontAttributes="Bold"
+     68:                     HeightRequest="45"
+     69:                     IsEnabled="{Binding IsLoading, Converter={toolkit:InvertedBoolConverter}}"
+     70:                     Margin="20,0,20,20" />
+     71: 
+     72:             <!-- Active Theme Section Background -->
+     73:             <Border Grid.Row="4"
+     74:                     Grid.ColumnSpan="3"
+     75:                     BackgroundColor="White" 
+     76:                     StrokeThickness="0"
+     77:                     Stroke="Transparent"
+     78:                     IsVisible="{Binding ActiveTheme, Converter={toolkit:IsNotNullConverter}}">
+     79:                 <Border.StrokeShape>
+     80:                     <RoundRectangle CornerRadius="20" />
+     81:                 </Border.StrokeShape>
+     82:             </Border>
+     83: 
+     84:             <!-- Active Theme Title -->
+     85:             <Label Grid.Row="4"
+     86:                    Grid.ColumnSpan="3"
+     87:                    Text="Active Theme" 
+     88:                    FontSize="16" 
+     89:                    FontAttributes="Bold"
+     90:                    TextColor="{StaticResource Gray900}"
+     91:                    HorizontalOptions="Center"
+     92:                    VerticalOptions="Start"
+     93:                    IsVisible="{Binding ActiveTheme, Converter={toolkit:IsNotNullConverter}}"
+     94:                    Margin="20,20,20,0" />
+     95: 
+     96:             <!-- Active Theme Color -->
+     97:             <Border Grid.Row="5"
+     98:                     Grid.Column="0"
+     99:                     BackgroundColor="{Binding ActiveTheme.PrimaryColorValue}"
+    100:                     StrokeThickness="0"
+    101:                     HeightRequest="40"
+    102:                     WidthRequest="40"
+    103:                     HorizontalOptions="End"
+    104:                     VerticalOptions="Center"
+    105:                     IsVisible="{Binding ActiveTheme, Converter={toolkit:IsNotNullConverter}}"
+    106:                     Margin="0,0,10,20">
+    107:                 <Border.StrokeShape>
+    108:                     <RoundRectangle CornerRadius="20" />
+    109:                 </Border.StrokeShape>
+    110:             </Border>
+    111: 
+    112:             <!-- Active Theme Name -->
+    113:             <Label Grid.Row="5"
+    114:                    Grid.Column="1"
+    115:                    Text="{Binding ActiveTheme.Name}" 
+    116:                    FontSize="16"
+    117:                    FontAttributes="Bold"
+    118:                    TextColor="{StaticResource Gray900}"
+    119:                    VerticalOptions="Center"
+    120:                    HorizontalOptions="Center"
+    121:                    IsVisible="{Binding ActiveTheme, Converter={toolkit:IsNotNullConverter}}"
+    122:                    Margin="0,0,0,20" />
+    123: 
+    124:             <!-- Active Theme Status -->
+    125:             <Label Grid.Row="5"
+    126:                    Grid.Column="2"
+    127:                    Text="✓ Active"
+    128:                    FontSize="14"
+    129:                    TextColor="{StaticResource Secondary}"
+    130:                    FontAttributes="Bold"
+    131:                    VerticalOptions="Center"
+    132:                    HorizontalOptions="Start"
+    133:                    IsVisible="{Binding ActiveTheme, Converter={toolkit:IsNotNullConverter}}"
+    134:                    Margin="10,0,0,20" />
+    135: 
+    136:             <!-- Available Themes Title Background -->
+    137:             <Border Grid.Row="6"
+    138:                     Grid.ColumnSpan="3"
+    139:                     BackgroundColor="White" 
+    140:                     StrokeThickness="0"
+    141:                     Stroke="Transparent">
+    142:                 <Border.StrokeShape>
+    143:                     <RoundRectangle CornerRadius="20" />
+    144:                 </Border.StrokeShape>
+    145:             </Border>
+    146: 
+    147:             <!-- Available Themes Title -->
+    148:             <Label Grid.Row="6"
+    149:                    Grid.ColumnSpan="3"
+    150:                    Text="Available Themes" 
+    151:                    FontSize="16" 
+    152:                    FontAttributes="Bold"
+    153:                    TextColor="{StaticResource Gray900}"
+    154:                    HorizontalOptions="Center"
+    155:                    VerticalOptions="Center"
+    156:                    Padding="20,15" />
+    157: 
+    158:             <!-- Themes Collection -->
+    159:             <CollectionView Grid.Row="7"
+    160:                             Grid.ColumnSpan="3"
+    161:                             ItemsSource="{Binding Themes}"
+    162:                             SelectionMode="None"
+    163:                             ItemSpacing="10">
+    164:                 
+    165:                 <CollectionView.ItemTemplate>
+    166:                     <DataTemplate>
+    167:                         <Border BackgroundColor="White" 
+    168:                                 StrokeThickness="0"
+    169:                                 Stroke="Transparent"
+    170:                                 Margin="0,5">
+    171:                             <Border.StrokeShape>
+    172:                                 <RoundRectangle CornerRadius="15" />
+    173:                             </Border.StrokeShape>
+    174:                             
+    175:                             <Grid RowDefinitions="Auto,Auto" 
+    176:                                   ColumnDefinitions="Auto,Auto,Auto,*,Auto,Auto"
+    177:                                   Padding="15"
+    178:                                   RowSpacing="10"
+    179:                                   ColumnSpacing="10">
+    180:                                 
+    181:                                 <!-- Primary Color -->
+    182:                                 <Border Grid.Row="0"
+    183:                                         Grid.Column="0"
+    184:                                         BackgroundColor="{Binding PrimaryColorValue}"
+    185:                                         StrokeThickness="0"
+    186:                                         HeightRequest="30"
+    187:                                         WidthRequest="30">
+    188:                                     <Border.StrokeShape>
+    189:                                         <RoundRectangle CornerRadius="15" />
+    190:                                     </Border.StrokeShape>
+    191:                                 </Border>
+    192:                                 
+    193:                                 <!-- Secondary Color -->
+    194:                                 <Border Grid.Row="0"
+    195:                                         Grid.Column="1"
+    196:                                         BackgroundColor="{Binding SecondaryColorValue}"
+    197:                                         StrokeThickness="0"
+    198:                                         HeightRequest="30"
+    199:                                         WidthRequest="30">
+    200:                                     <Border.StrokeShape>
+    201:                                         <RoundRectangle CornerRadius="15" />
+    202:                                     </Border.StrokeShape>
+    203:                                 </Border>
+    204:                                 
+    205:                                 <!-- Accent Color -->
+    206:                                 <Border Grid.Row="0"
+    207:                                         Grid.Column="2"
+    208:                                         BackgroundColor="{Binding AccentColorValue}"
+    209:                                         StrokeThickness="0"
+    210:                                         HeightRequest="30"
+    211:                                         WidthRequest="30"
+    212:                                         IsVisible="{Binding AccentColorValue, Converter={toolkit:IsNotNullConverter}}">
+    213:                                     <Border.StrokeShape>
+    214:                                         <RoundRectangle CornerRadius="15" />
+    215:                                     </Border.StrokeShape>
+    216:                                 </Border>
+    217:                                 
+    218:                                 <!-- Theme Name -->
+    219:                                 <Label Grid.Row="0"
+    220:                                        Grid.Column="3"
+    221:                                        Text="{Binding Name}" 
+    222:                                        FontSize="16"
+    223:                                        FontAttributes="Bold"
+    224:                                        TextColor="{StaticResource Gray900}"
+    225:                                        VerticalOptions="Center" />
+    226:                                 
+    227:                                 <!-- Active Indicator -->
+    228:                                 <Label Grid.Row="0"
+    229:                                        Grid.Column="4"
+    230:                                        Text="✓"
+    231:                                        FontSize="16"
+    232:                                        TextColor="{StaticResource Secondary}"
+    233:                                        FontAttributes="Bold"
+    234:                                        VerticalOptions="Center"
+    235:                                        IsVisible="{Binding IsActive}" />
+    236:                                 
+    237:                                 <!-- Set Active Button -->
+    238:                                 <Button Grid.Row="1"
+    239:                                         Grid.Column="4"
+    240:                                         Text="Set Active"
+    241:                                         Command="{Binding Source={RelativeSource AncestorType={x:Type vm:ThemesPageViewModel}}, Path=SetActiveThemeCommand}"
+    242:                                         CommandParameter="{Binding .}"
+    243:                                         BackgroundColor="{StaticResource Secondary}"
+    244:                                         TextColor="White"
+    245:                                         CornerRadius="10"
+    246:                                         FontSize="12"
+    247:                                         Padding="15,8"
+    248:                                         IsVisible="{Binding IsActive, Converter={toolkit:InvertedBoolConverter}}" />
+    249:                                 
+    250:                                 <!-- Delete Button -->
+    251:                                 <Button Grid.Row="1"
+    252:                                         Grid.Column="5"
+    253:                                         Text="Delete"
+    254:                                         Command="{Binding Source={RelativeSource AncestorType={x:Type vm:ThemesPageViewModel}}, Path=DeleteThemeCommand}"
+    255:                                         CommandParameter="{Binding .}"
+    256:                                         BackgroundColor="{StaticResource Gray300}"
+    257:                                         TextColor="{StaticResource Gray700}"
+    258:                                         CornerRadius="10"
+    259:                                         FontSize="12"
+    260:                                         Padding="15,8"
+    261:                                         IsVisible="{Binding IsActive, Converter={toolkit:InvertedBoolConverter}}" />
+    262:                             </Grid>
+    263:                         </Border>
+    264:                     </DataTemplate>
+    265:                 </CollectionView.ItemTemplate>
+    266:             </CollectionView>
+    267: 
+    268:             <!-- Loading Indicator -->
+    269:             <ActivityIndicator Grid.Row="7"
+    270:                                Grid.ColumnSpan="3"
+    271:                                IsVisible="{Binding IsLoading}"
+    272:                                IsRunning="{Binding IsLoading}"
+    273:                                Color="{StaticResource Secondary}"
+    274:                                HorizontalOptions="Center"
+    275:                                VerticalOptions="Center" />
+    276:         </Grid>
+    277:     </ScrollView>
+    278: </ContentPage>
```

Next, it updated `MauiProgram.cs` to register ThemesPage and ThemesPageViewModel:

```cs
 1,  1: using CommunityToolkit.Maui;
   2,  2: using Microsoft.Extensions.Logging;
   3,  3: using MoodBoost.Services;
   4,  4: using MoodBoost.ViewModels;
   5,  5: using MoodBoost.Views;
   6,  6: 
   7,  7: namespace MoodBoost;
   8,  8: 
   9,  9: public static class MauiProgram
  10, 10: {
  11, 11:     public static MauiApp CreateMauiApp()
  12, 12:     {
  13, 13:         var builder = MauiApp.CreateBuilder();
  14, 14:         builder
  15, 15:             .UseMauiApp<App>()
  16, 16:             .UseMauiCommunityToolkit()
  17, 17:             .ConfigureFonts(fonts =>
  18, 18:             {
  19, 19:                 fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
  20, 20:                 fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
  21, 21:             });
  22, 22: 
  23, 23:         // Register Services
  24, 24:         builder.Services.AddSingleton<IDatabaseService, DatabaseService>();
  25, 25:         builder.Services.AddSingleton<IThemeService, ThemeService>();
  26, 26: 
  27, 27:         // Register ViewModels and Views
  28, 28:         builder.Services.AddSingleton<MainPageViewModel>();
  29, 29:         builder.Services.AddSingleton<MainPage>();
+     30:         builder.Services.AddTransient<ThemesPageViewModel>();
+     31:         builder.Services.AddTransient<ThemesPage>();
  30, 32: 
  31, 33: #if DEBUG
  32, 34:         builder.Services.AddLogging(configure => configure.AddDebug());
  33, 35: #endif
  34, 36: 
  35, 37:         return builder.Build();
  36, 38:     }
  37, 39: }
```

  Next, it updated AppShell to use a TabBar:

```xml
    1,  1: <?xml version="1.0" encoding="UTF-8" ?>
   2,  2: <Shell x:Class="MoodBoost.AppShell"
   3,  3:        xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
   4,  4:        xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
   5,  5:        xmlns:local="clr-namespace:MoodBoost.Views"
   6,  6:        Shell.FlyoutBehavior="Disabled"
   7,  7:        Title="MoodBoost">
   8,  8: 
-  9    :     <ShellContent Title="Home"
- 10    :                   ContentTemplate="{DataTemplate local:MainPage}"
- 11    :                   Route="MainPage" />
+      9:     <TabBar>
+     10:         <ShellContent Title="Home"
+     11:                       Icon="home"
+     12:                       ContentTemplate="{DataTemplate local:MainPage}"
+     13:                       Route="MainPage" />
+     14: 
+     15:         <ShellContent Title="Themes"
+     16:                       Icon="palette"
+     17:                       ContentTemplate="{DataTemplate local:ThemesPage}"
+     18:                       Route="ThemesPage" />
+     19:     </TabBar>
  12, 20: 
  13, 21: </Shell>
```