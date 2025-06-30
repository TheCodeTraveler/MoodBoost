using MoodBoost.Views;

namespace MoodBoost;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new AppShell();
    }
}
