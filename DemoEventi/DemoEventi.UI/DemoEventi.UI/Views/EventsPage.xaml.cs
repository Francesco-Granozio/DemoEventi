using DemoEventi.UI.ViewModels;

namespace DemoEventi.UI.Views;

public partial class EventsPage : ContentPage
{
    public EventsPage(EventsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is EventsViewModel viewModel)
        {
            await viewModel.LoadEventsAsync();
        }
    }
}
