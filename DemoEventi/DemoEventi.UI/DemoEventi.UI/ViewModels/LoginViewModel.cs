using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DemoEventi.Application.DTOs;
using DemoEventi.UI.Services;
using DemoEventi.UI.Views;

namespace DemoEventi.UI.ViewModels;

public partial class LoginViewModel : BaseViewModel
{
    private readonly IAuthService _authService;

    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    public LoginViewModel(IAuthService authService)
    {
        _authService = authService;
        Title = "Login";
    }

    [RelayCommand]
    public async Task LoginAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ClearError();

            // Basic validation
            if (string.IsNullOrWhiteSpace(Email))
            {
                SetError("Email is required");
                return;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                SetError("Password is required");
                return;
            }

            var loginDto = new LoginDto
            {
                Email = Email.Trim(),
                Password = Password
            };

            var result = await _authService.LoginAsync(loginDto);
            if (result.IsSuccess)
            {
                if (Shell.Current is AppShell appShell)
                {
                    appShell.ShowMainApp();
                }
            }
            else
            {
                SetError(result.Error);
            }
        }
        catch (Exception ex)
        {
            SetError($"Login failed: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task DemoLoginAsync()
    {
        try
        {
            IsBusy = true;
            ClearError();

            // For demo purposes, create a demo user
            var demoUser = new UserDto
            {
                Id = Guid.NewGuid(),
                FirstName = "Demo",
                LastName = "User",
                Email = "demo@demo.com",
                InterestIds = new List<Guid>()
            };
            
            await _authService.SetCurrentUserAsync(demoUser);

            if (Shell.Current is AppShell appShell)
            {
                appShell.ShowMainApp();
            }
        }
        catch (Exception ex)
        {
            SetError($"Demo login failed: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task NavigateToRegisterAsync()
    {
        await Shell.Current.GoToAsync(nameof(RegisterPage));
    }
}
