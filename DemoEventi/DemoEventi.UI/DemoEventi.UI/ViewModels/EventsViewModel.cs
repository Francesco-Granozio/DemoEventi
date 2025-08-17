using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DemoEventi.Application.DTOs;
using DemoEventi.UI.Services;
using System.Collections.ObjectModel;

namespace DemoEventi.UI.ViewModels;

public partial class EventsViewModel : BaseViewModel
{
    private readonly IApiService _apiService;

    [ObservableProperty]
    private ObservableCollection<EventDto> _events = new();

    public EventsViewModel(IApiService apiService)
    {
        _apiService = apiService;
        Title = "Events";
    }

    [RelayCommand]
    public async Task LoadEventsAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ClearError();

            var result = await _apiService.GetEventsAsync();
            if (result.IsSuccess)
            {
                Events.Clear();
                foreach (var eventDto in result.Value!)
                {
                    Events.Add(eventDto);
                }
            }
            else
            {
                SetError(result.Error);
            }
        }
        catch (Exception ex)
        {
            SetError($"Error loading events: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task CreateEventAsync()
    {
        await Shell.Current.GoToAsync("//EventFormPage");
    }

    [RelayCommand]
    private async Task EditEventAsync(EventDto eventDto)
    {
        var parameters = new Dictionary<string, object>
        {
            { "EventId", eventDto.Id }
        };
        await Shell.Current.GoToAsync("//EventFormPage", parameters);
    }

    [RelayCommand]
    private async Task DeleteEventAsync(EventDto eventDto)
    {
        if (IsBusy) return;

        var confirmed = await Microsoft.Maui.Controls.Application.Current!.MainPage!.DisplayAlert(
            "Delete Event",
            $"Are you sure you want to delete '{eventDto.Name}'?",
            "Delete",
            "Cancel");

        if (!confirmed) return;

        try
        {
            IsBusy = true;
            ClearError();

            var result = await _apiService.DeleteEventAsync(eventDto.Id);
            if (result.IsSuccess)
            {
                Events.Remove(eventDto);
            }
            else
            {
                SetError(result.Error);
            }
        }
        catch (Exception ex)
        {
            SetError($"Error deleting event: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task RefreshAsync()
    {
        await LoadEventsAsync();
    }
}
