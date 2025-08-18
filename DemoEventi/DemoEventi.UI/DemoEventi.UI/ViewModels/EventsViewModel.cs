using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DemoEventi.Application.DTOs;
using DemoEventi.UI.Services;
using DemoEventi.UI.Views;
using System.Collections.ObjectModel;
using System.Timers;

namespace DemoEventi.UI.ViewModels;

public partial class EventsViewModel : BaseViewModel
{
    private readonly IApiService _apiService;
    private readonly IAuthService _authService;
    private readonly System.Timers.Timer _searchTimer;
    private int _currentPage = 1;
    private const int PageSize = 10;

    [ObservableProperty]
    private ObservableCollection<EventDto> _events = new();

    [ObservableProperty]
    private string _searchText = string.Empty;

    [ObservableProperty]
    private bool _isLoadingMore;

    [ObservableProperty]
    private bool _canLoadMore = true;

    [ObservableProperty]
    private Guid _currentUserId;

    public EventsViewModel(IApiService apiService, IAuthService authService)
    {
        _apiService = apiService;
        _authService = authService;
        Title = "Events";
        
        // Setup search debouncing
        _searchTimer = new System.Timers.Timer(500); // 500ms delay
        _searchTimer.Elapsed += OnSearchTimerElapsed;
        _searchTimer.AutoReset = false;

        // Get current user ID from authentication service
        Task.Run(async () => await SetCurrentUserAsync());
    }

    partial void OnSearchTextChanged(string value)
    {
        _searchTimer.Stop();
        _searchTimer.Start();
    }

    [RelayCommand]
    public async Task LoadEventsAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ClearError();
            _currentPage = 1;
            CanLoadMore = true;

            var result = await _apiService.GetEventsAsync();
            if (result.IsSuccess)
            {
                Events.Clear();
                foreach (var eventDto in result.Value!)
                {
                    Events.Add(eventDto);
                }
                // For now, simulate pagination by limiting results
                CanLoadMore = result.Value!.Count() >= PageSize;
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
    public async Task LoadMoreEventsAsync()
    {
        if (IsLoadingMore || !CanLoadMore) return;

        try
        {
            IsLoadingMore = true;
            _currentPage++;

            var searchDto = new EventSearchDto
            {
                SearchTerm = SearchText,
                PageNumber = _currentPage,
                PageSize = PageSize
            };

            var result = await _apiService.SearchEventsAsync(searchDto);
            if (result.IsSuccess)
            {
                var pagedResult = result.Value!;
                foreach (var eventDto in pagedResult.Items)
                {
                    Events.Add(eventDto);
                }

                CanLoadMore = pagedResult.HasNextPage;
            }
            else
            {
                SetError(result.Error);
                _currentPage--; // Revert page increment on error
            }
        }
        catch (Exception ex)
        {
            SetError($"Error loading more events: {ex.Message}");
            _currentPage--; // Revert page increment on error
        }
        finally
        {
            IsLoadingMore = false;
        }
    }

    [RelayCommand]
    public async Task SearchEventsAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ClearError();
            _currentPage = 1;
            _canLoadMore = true;

            var searchDto = new EventSearchDto
            {
                SearchTerm = SearchText,
                PageNumber = _currentPage,
                PageSize = PageSize
            };

            var result = await _apiService.SearchEventsAsync(searchDto);
            if (result.IsSuccess)
            {
                var pagedResult = result.Value!;
                Events.Clear();
                foreach (var eventDto in pagedResult.Items)
                {
                    Events.Add(eventDto);
                }

                _canLoadMore = pagedResult.HasNextPage;
            }
            else
            {
                SetError(result.Error);
            }
        }
        catch (Exception ex)
        {
            SetError($"Error searching events: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task CreateEventAsync()
    {
        await Shell.Current.GoToAsync(nameof(EventFormPage));
    }

    [RelayCommand]
    private async Task EditEventAsync(EventDto eventDto)
    {
        var parameters = new Dictionary<string, object>
        {
            { "EventId", eventDto.Id }
        };
        await Shell.Current.GoToAsync(nameof(EventFormPage), parameters);
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

    [RelayCommand]
    public async Task JoinEventAsync(EventDto eventDto)
    {
        if (CurrentUserId == Guid.Empty) return;

        try
        {
            IsBusy = true;
            var result = await _apiService.JoinEventAsync(eventDto.Id, CurrentUserId);
            if (result.IsSuccess)
            {
                // Update local model
                var participantIds = eventDto.ParticipantIds?.ToList() ?? new List<Guid>();
                if (!participantIds.Contains(CurrentUserId))
                {
                    participantIds.Add(CurrentUserId);
                    eventDto.ParticipantIds = participantIds;
                }
                
                await Microsoft.Maui.Controls.Application.Current!.MainPage!.DisplayAlert(
                    "Success", $"You've joined '{eventDto.Name}'!", "OK");
            }
            else
            {
                SetError(result.Error);
            }
        }
        catch (Exception ex)
        {
            SetError($"Error joining event: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task LeaveEventAsync(EventDto eventDto)
    {
        if (CurrentUserId == Guid.Empty) return;

        try
        {
            IsBusy = true;
            var result = await _apiService.LeaveEventAsync(eventDto.Id, CurrentUserId);
            if (result.IsSuccess)
            {
                // Update local model
                var participantIds = eventDto.ParticipantIds?.ToList() ?? new List<Guid>();
                if (participantIds.Contains(CurrentUserId))
                {
                    participantIds.Remove(CurrentUserId);
                    eventDto.ParticipantIds = participantIds;
                }
                
                await Microsoft.Maui.Controls.Application.Current!.MainPage!.DisplayAlert(
                    "Success", $"You've left '{eventDto.Name}'!", "OK");
            }
            else
            {
                SetError(result.Error);
            }
        }
        catch (Exception ex)
        {
            SetError($"Error leaving event: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task ViewParticipantsAsync(EventDto eventDto)
    {
        try
        {
            if (eventDto.ParticipantCount == 0)
            {
                await Microsoft.Maui.Controls.Application.Current!.MainPage!.DisplayAlert(
                    "No Participants", "No one has joined this event yet.", "OK");
                return;
            }

            // TODO: Navigate to participants page or show participant list
            var message = $"This event has {eventDto.ParticipantCount} participant(s).";
            await Microsoft.Maui.Controls.Application.Current!.MainPage!.DisplayAlert(
                "Participants", message, "OK");
        }
        catch (Exception ex)
        {
            SetError($"Error viewing participants: {ex.Message}");
        }
    }

    public bool IsUserParticipating(EventDto eventDto)
    {
        return eventDto.ParticipantIds?.Contains(CurrentUserId) == true;
    }



    private async void OnSearchTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await SearchEventsAsync();
        });
    }

    private async Task SetCurrentUserAsync()
    {
        try
        {
            CurrentUserId = await _authService.GetCurrentUserIdAsync();
        }
        catch
        {
            // Ignore for now
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _searchTimer?.Dispose();
        }
        base.Dispose(disposing);
    }
}
