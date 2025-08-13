using DemoEventi.UI.ViewModels;

namespace DemoEventi.UI;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
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
