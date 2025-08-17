using DemoEventi.UI.ViewModels;

namespace DemoEventi.UI.Views;

public partial class InterestsPage : ContentPage
{
    public InterestsPage(InterestsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is InterestsViewModel viewModel)
        {
            await viewModel.LoadInterestsAsync();
        }
    }
}
