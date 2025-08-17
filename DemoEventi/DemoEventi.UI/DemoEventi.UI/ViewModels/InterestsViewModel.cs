using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DemoEventi.Application.DTOs;
using DemoEventi.UI.Services;

namespace DemoEventi.UI.ViewModels;

public partial class InterestsViewModel : BaseViewModel
{
    private readonly IApiService _apiService;

    [ObservableProperty]
    private ObservableCollection<InterestDto> _interests = new();

    public InterestsViewModel(IApiService apiService)
    {
        _apiService = apiService;
        Title = "Interests";
    }

    [RelayCommand]
    public async Task LoadInterestsAsync()
    {
        if (IsBusy) return;

        try
        {
            System.Diagnostics.Debug.WriteLine("InterestsViewModel.LoadInterestsAsync: Starting...");
            IsBusy = true;
            ClearError();

            System.Diagnostics.Debug.WriteLine("InterestsViewModel.LoadInterestsAsync: Calling API...");
            var result = await _apiService.GetInterestsAsync();
            System.Diagnostics.Debug.WriteLine($"InterestsViewModel.LoadInterestsAsync: API result success: {result.IsSuccess}");
            
            if (result.IsSuccess)
            {
                System.Diagnostics.Debug.WriteLine($"InterestsViewModel.LoadInterestsAsync: Adding {result.Value?.Count() ?? 0} interests to collection");
                Interests.Clear();
                foreach (var interest in result.Value!)
                {
                    Interests.Add(interest);
                }
                System.Diagnostics.Debug.WriteLine("InterestsViewModel.LoadInterestsAsync: Successfully loaded interests");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"InterestsViewModel.LoadInterestsAsync: Error: {result.Error}");
                SetError(result.Error);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"InterestsViewModel.LoadInterestsAsync: Exception: {ex}");
            SetError($"Error loading interests: {ex.Message}");
        }
        finally
        {
            System.Diagnostics.Debug.WriteLine("InterestsViewModel.LoadInterestsAsync: Setting IsBusy to false");
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadInterestsAsync();
    }
}
