using MoodBoost.ViewModels;

namespace MoodBoost.Views;

public partial class MainPage : ContentPage
{
	readonly MainPageViewModel _viewModel;

	public MainPage(MainPageViewModel viewModel)
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
