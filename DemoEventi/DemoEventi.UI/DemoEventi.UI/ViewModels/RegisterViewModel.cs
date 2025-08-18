using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DemoEventi.Application.DTOs;
using DemoEventi.UI.Services;

namespace DemoEventi.UI.ViewModels;

public partial class RegisterViewModel : BaseViewModel
{
    private readonly IAuthService _authService;

    [ObservableProperty]
    private string _firstName = string.Empty;

    [ObservableProperty]
    private string _lastName = string.Empty;

    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private string _confirmPassword = string.Empty;

    public RegisterViewModel(IAuthService authService)
    {
        _authService = authService;
        Title = "Register";
    }

    [RelayCommand]
    public async Task RegisterAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ClearError();

            // Validation
            if (string.IsNullOrWhiteSpace(FirstName))
            {
                SetError("First name is required");
                return;
            }

            if (string.IsNullOrWhiteSpace(LastName))
            {
                SetError("Last name is required");
                return;
            }

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

            if (Password.Length < 6)
            {
                SetError("Password must be at least 6 characters");
                return;
            }

            if (Password != ConfirmPassword)
            {
                SetError("Passwords do not match");
                return;
            }

            var registerDto = new RegisterDto
            {
                FirstName = FirstName.Trim(),
                LastName = LastName.Trim(),
                Email = Email.Trim(),
                Password = Password,
                ConfirmPassword = ConfirmPassword
            };

            var result = await _authService.RegisterAsync(registerDto);
            if (result.IsSuccess)
            {
                await Microsoft.Maui.Controls.Application.Current!.MainPage!.DisplayAlert(
                    "Success", "Account created successfully! You can now login.", "OK");

                // Navigate back to login
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                SetError(result.Error);
            }
        }
        catch (Exception ex)
        {
            SetError($"Registration failed: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task NavigateToLoginAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
