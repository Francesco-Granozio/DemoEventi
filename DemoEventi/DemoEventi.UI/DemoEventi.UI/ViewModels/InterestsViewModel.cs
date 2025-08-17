using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DemoEventi.Application.DTOs;
using DemoEventi.UI.Services;
using System.Collections.ObjectModel;

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
            IsBusy = true;
            ClearError();

            var result = await _apiService.GetInterestsAsync();

            if (result.IsSuccess)
            {
                Interests.Clear();
                foreach (var interest in result.Value!)
                {
                    Interests.Add(interest);
                }
            }
            else
            {
                SetError(result.Error);
            }
        }
        catch (Exception ex)
        {
            SetError($"Error loading interests: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadInterestsAsync();
    }
}
