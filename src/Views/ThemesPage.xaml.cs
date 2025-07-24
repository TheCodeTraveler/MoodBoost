using MoodBoost.ViewModels;

namespace MoodBoost.Views;

public partial class ThemesPage : ContentPage
{
	readonly ThemesPageViewModel _viewModel;

	public ThemesPage(ThemesPageViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		BindingContext = _viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await _viewModel.InitializeAsync();
	}
}
