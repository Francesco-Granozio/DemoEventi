using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DemoEventi.Application.DTOs;
using DemoEventi.UI.Services;

namespace DemoEventi.UI.ViewModels;

public partial class UsersViewModel : BaseViewModel
{
    private readonly IApiService _apiService;

    [ObservableProperty]
    private ObservableCollection<UserDto> _users = new();

    public UsersViewModel(IApiService apiService)
    {
        _apiService = apiService;
        Title = "Users";
    }

    [RelayCommand]
    public async Task LoadUsersAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ClearError();

            var result = await _apiService.GetUsersAsync();
            if (result.IsSuccess)
            {
                Users.Clear();
                foreach (var user in result.Value!)
                {
                    Users.Add(user);
                }
            }
            else
            {
                SetError(result.Error);
            }
        }
        catch (Exception ex)
        {
            SetError($"Error loading users: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task CreateUserAsync()
    {
        await Shell.Current.GoToAsync("//UserFormPage");
    }

    [RelayCommand]
    private async Task EditUserAsync(UserDto user)
    {
        var parameters = new Dictionary<string, object>
        {
            { "UserId", user.Id }
        };
        await Shell.Current.GoToAsync("//UserFormPage", parameters);
    }

    [RelayCommand]
    private async Task DeleteUserAsync(UserDto user)
    {
        if (IsBusy) return;

        var confirmed = await Microsoft.Maui.Controls.Application.Current!.MainPage!.DisplayAlert(
            "Delete User",
            $"Are you sure you want to delete {user.FullName}?",
            "Delete",
            "Cancel");

        if (!confirmed) return;

        try
        {
            IsBusy = true;
            ClearError();

            var result = await _apiService.DeleteUserAsync(user.Id);
            if (result.IsSuccess)
            {
                Users.Remove(user);
            }
            else
            {
                SetError(result.Error);
            }
        }
        catch (Exception ex)
        {
            SetError($"Error deleting user: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadUsersAsync();
    }
}
