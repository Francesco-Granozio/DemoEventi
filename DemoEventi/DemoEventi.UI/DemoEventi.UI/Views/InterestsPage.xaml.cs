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
        System.Diagnostics.Debug.WriteLine("InterestsPage.OnAppearing: Starting...");
        if (BindingContext is InterestsViewModel viewModel)
        {
            System.Diagnostics.Debug.WriteLine("InterestsPage.OnAppearing: Calling LoadInterestsAsync...");
            await viewModel.LoadInterestsAsync();
            System.Diagnostics.Debug.WriteLine("InterestsPage.OnAppearing: LoadInterestsAsync completed");
        }
        else
        {
            System.Diagnostics.Debug.WriteLine("InterestsPage.OnAppearing: BindingContext is not InterestsViewModel");
        }
    }
}
