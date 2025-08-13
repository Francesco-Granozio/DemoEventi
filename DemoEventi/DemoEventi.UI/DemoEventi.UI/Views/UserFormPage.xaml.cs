using DemoEventi.UI.ViewModels;

namespace DemoEventi.UI.Views;

public partial class UserFormPage : ContentPage
{
    public UserFormPage(UserFormViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is UserFormViewModel viewModel)
        {
            await viewModel.InitializeAsync();
        }
    }
}
