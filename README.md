# MoodMixer

MoodMixer is a fun, interactive .NET MAUI application that allows users to create, save, and apply custom color themes based on their mood. The app features a clean, professional user interface and follows modern MVVM architecture principles.

![](./blog/App.gif)

## Features

- Create custom color themes with primary, secondary, and accent colors
- Randomize colors with a single tap
- Save themes with custom names
- View a list of saved themes
- Apply saved themes with one click
- Delete themes you no longer want

## Technical Details

- Built with .NET 9 and .NET MAUI
- Follows MVVM architecture using CommunityToolkit.Mvvm 8.4.0
- Uses CommunityToolkit.Maui 12.0.0 for enhanced UI components
- Implements dependency injection for better testability and maintainability
- Features responsive design that works across mobile platforms

## Project Structure

- **Models**: Contains the `ColorTheme` record for storing theme data
- **ViewModels**: Contains the `MainViewModel` with commands and properties
- **Views**: Contains the `MainPage` XAML and code-behind
- **Platforms**: Platform-specific implementation files
- **Resources**: App resources including styles, images, and fonts

## Best Practices Used

- Source generators for MVVM with `[ObservableProperty]` and `[RelayCommand]`
- Dependency injection for services and view models
- Clean separation of concerns with MVVM pattern
- Use of records for immutable data models
- DateTimeOffset for proper date/time handling
- Responsive UI design with proper layout techniques
- Consistent styling with ResourceDictionary

## Getting Started

1. Clone the repository
2. Open the solution in Visual Studio 2022 or later
3. Build and run the application on your preferred device or emulator

## License

MIT
