using DemoEventi.UI.ViewModels;

namespace DemoEventi.UI.Views;

public partial class ProfilePage : ContentPage
{
	public ProfilePage(ProfileViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		
		if (BindingContext is ProfileViewModel viewModel)
		{
			await viewModel.LoadProfileCommand.ExecuteAsync(null);
		}
	}
}
