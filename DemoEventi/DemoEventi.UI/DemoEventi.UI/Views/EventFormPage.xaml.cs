using DemoEventi.UI.ViewModels;

namespace DemoEventi.UI.Views;

public partial class EventFormPage : ContentPage
{
    public EventFormPage(EventFormViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is EventFormViewModel viewModel)
        {
            await viewModel.InitializeAsync();
        }
    }
}
