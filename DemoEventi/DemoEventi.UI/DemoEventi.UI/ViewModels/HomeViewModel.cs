using CommunityToolkit.Mvvm.Input;

namespace DemoEventi.UI.ViewModels;

public partial class HomeViewModel : BaseViewModel
{
    public HomeViewModel()
    {
        Title = "DemoEventi";
    }

    [RelayCommand]
    private async Task NavigateToUsersAsync()
    {
        await Shell.Current.GoToAsync("//UsersPage");
    }

    [RelayCommand]
    private async Task NavigateToEventsAsync()
    {
        await Shell.Current.GoToAsync("//EventsPage");
    }

    [RelayCommand]
    private async Task NavigateToInterestsAsync()
    {
        await Shell.Current.GoToAsync("//InterestsPage");
    }
}
