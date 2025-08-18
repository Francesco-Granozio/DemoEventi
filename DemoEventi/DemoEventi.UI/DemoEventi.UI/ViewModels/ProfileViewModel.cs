using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DemoEventi.Application.DTOs;
using DemoEventi.UI.Services;
using System.Collections.ObjectModel;

namespace DemoEventi.UI.ViewModels;

public partial class ProfileViewModel : BaseViewModel
{
    private readonly IApiService _apiService;
    private readonly IAuthService _authService;

    [ObservableProperty]
    private string? _firstName;

    [ObservableProperty]
    private string? _lastName;

    [ObservableProperty]
    private string? _email;

    [ObservableProperty]
    private string? _profileImageUrl;

    [ObservableProperty]
    private ObservableCollection<InterestDto> _userInterests = new();

    [ObservableProperty]
    private Guid _currentUserId;

    public ProfileViewModel(IApiService apiService, IAuthService authService)
    {
        _apiService = apiService;
        _authService = authService;
        Title = "Profile";
    }

    [RelayCommand]
    public async Task LoadProfileAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ClearError();

            var currentUser = await _authService.GetCurrentUserAsync();
            if (currentUser != null)
            {
                CurrentUserId = currentUser.Id;
                FirstName = currentUser.FirstName;
                LastName = currentUser.LastName;
                Email = currentUser.Email;
                ProfileImageUrl = currentUser.ProfileImageUrl;

                // Load user interests
                await LoadUserInterestsAsync();
            }
            else
            {
                SetError("No user logged in");
            }
        }
        catch (Exception ex)
        {
            SetError($"Error loading profile: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task SaveAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ClearError();

            var updateDto = new UpdateUserDto
            {
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                ProfileImageUrl = ProfileImageUrl,
                InterestIds = UserInterests.Select(i => i.Id).ToList()
            };

            var result = await _apiService.UpdateUserAsync(CurrentUserId, updateDto);
            if (result.IsSuccess)
            {
                // Update the current user in auth service
                var updatedUser = result.Value!;
                await _authService.SetCurrentUserAsync(updatedUser);
                
                await Microsoft.Maui.Controls.Application.Current!.MainPage!.DisplayAlert(
                    "Success", "Profile updated successfully!", "OK");
            }
            else
            {
                SetError(result.Error);
            }
        }
        catch (Exception ex)
        {
            SetError($"Error saving profile: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task ChangeProfileImageAsync()
    {
        try
        {
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Select a profile photo"
            });

            if (result != null)
            {
                // For demo purposes, just set a placeholder URL
                // In a real app, you would upload the image to a server
                ProfileImageUrl = $"profile_image_{CurrentUserId}.jpg";
                
                await Microsoft.Maui.Controls.Application.Current!.MainPage!.DisplayAlert(
                    "Success", "Profile image updated! (Demo mode - image not actually uploaded)", "OK");
            }
        }
        catch (Exception ex)
        {
            SetError($"Error changing profile image: {ex.Message}");
        }
    }

    [RelayCommand]
    public async Task ManageInterestsAsync()
    {
        try
        {
            // Navigate to interests page where user can select their interests
            await Shell.Current.GoToAsync("//InterestsPage");
        }
        catch (Exception ex)
        {
            SetError($"Error navigating to interests: {ex.Message}");
        }
    }

    [RelayCommand]
    public async Task LogoutAsync()
    {
        try
        {
            var result = await Microsoft.Maui.Controls.Application.Current!.MainPage!.DisplayAlert(
                "Logout", "Are you sure you want to logout?", "Yes", "No");
            
            if (result)
            {
                await _authService.LogoutAsync();
                if (Shell.Current is AppShell appShell)
                {
                    appShell.ShowLogin();
                }
            }
        }
        catch (Exception ex)
        {
            SetError($"Error during logout: {ex.Message}");
        }
    }

    private async Task LoadUserInterestsAsync()
    {
        try
        {
            var allInterestsResult = await _apiService.GetInterestsAsync();
            if (allInterestsResult.IsSuccess)
            {
                var userResult = await _apiService.GetUserAsync(CurrentUserId);
                if (userResult.IsSuccess && userResult.Value!.InterestIds != null)
                {
                    var userInterestIds = userResult.Value.InterestIds.ToHashSet();
                    var userInterests = allInterestsResult.Value!
                        .Where(i => userInterestIds.Contains(i.Id))
                        .ToList();

                    UserInterests.Clear();
                    foreach (var interest in userInterests)
                    {
                        UserInterests.Add(interest);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            SetError($"Error loading user interests: {ex.Message}");
        }
    }
}
