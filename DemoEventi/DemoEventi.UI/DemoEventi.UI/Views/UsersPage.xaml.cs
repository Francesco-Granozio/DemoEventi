using DemoEventi.UI.ViewModels;

namespace DemoEventi.UI.Views;

public partial class UsersPage : ContentPage
{
    public UsersPage(UsersViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is UsersViewModel viewModel)
        {
            await viewModel.LoadUsersAsync();
        }
    }
}
