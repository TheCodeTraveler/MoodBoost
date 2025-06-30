using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using MoodBoost.ViewModels;
using MoodBoost.Views;

namespace MoodBoost;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Register ViewModels and Views
        builder.Services.AddSingleton<MainPageViewModel>();
        builder.Services.AddSingleton<MainPage>();

#if DEBUG
        builder.Services.AddLogging(configure => configure.AddDebug());
#endif

        return builder.Build();
    }
}
