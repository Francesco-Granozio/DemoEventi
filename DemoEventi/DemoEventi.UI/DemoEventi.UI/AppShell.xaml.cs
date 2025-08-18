using DemoEventi.UI.ViewModels;
using DemoEventi.UI.Views;

namespace DemoEventi.UI;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        RegisterRoutes();
    }

    public void ShowMainApp()
    {
        LoginShell.IsVisible = false;
        MainTabBar.IsVisible = true;
        CurrentItem = MainTabBar;
    }

    public void ShowLogin()
    {
        MainTabBar.IsVisible = false;
        LoginShell.IsVisible = true;
        CurrentItem = LoginShell;
    }

    private void RegisterRoutes()
    {
        Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
        Routing.RegisterRoute(nameof(UsersPage), typeof(UsersPage));
        Routing.RegisterRoute(nameof(EventsPage), typeof(EventsPage));
        Routing.RegisterRoute(nameof(InterestsPage), typeof(InterestsPage));
        Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
        Routing.RegisterRoute(nameof(UserFormPage), typeof(UserFormPage));
        Routing.RegisterRoute(nameof(EventFormPage), typeof(EventFormPage));
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
    }

    protected override async void OnNavigating(ShellNavigatingEventArgs args)
    {
        base.OnNavigating(args);

        if (args.Target.Location.ToString().Contains("UserFormPage"))
        {
            if (args.Target.Location.ToString().Contains("UserId="))
            {
                // Extract UserId from query parameters
                var queryString = args.Target.Location.Query;
                var userIdParam = queryString.Split('&')
                    .FirstOrDefault(p => p.StartsWith("UserId="));

                if (userIdParam != null && Guid.TryParse(userIdParam.Split('=')[1], out var userId))
                {
                    // Set edit mode for UserFormViewModel
                    if (CurrentPage?.BindingContext is UserFormViewModel viewModel)
                    {
                        viewModel.SetEditMode(userId);
                    }
                }
            }
        }
        else if (args.Target.Location.ToString().Contains("EventFormPage"))
        {
            if (args.Target.Location.ToString().Contains("EventId="))
            {
                // Extract EventId from query parameters
                var queryString = args.Target.Location.Query;
                var eventIdParam = queryString.Split('&')
                    .FirstOrDefault(p => p.StartsWith("EventId="));

                if (eventIdParam != null && Guid.TryParse(eventIdParam.Split('=')[1], out var eventId))
                {
                    // Set edit mode for EventFormViewModel
                    if (CurrentPage?.BindingContext is EventFormViewModel viewModel)
                    {
                        viewModel.SetEditMode(eventId);
                    }
                }
            }
        }
    }
}
