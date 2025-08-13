using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DemoEventi.Application.DTOs;
using DemoEventi.UI.Services;

namespace DemoEventi.UI.ViewModels;

public partial class EventFormViewModel : BaseViewModel
{
    private readonly IApiService _apiService;

    [ObservableProperty]
    private string? _name;

    [ObservableProperty]
    private string? _location;

    [ObservableProperty]
    private DateTime _startDate = DateTime.Now;

    [ObservableProperty]
    private TimeSpan _startTime = DateTime.Now.TimeOfDay;

    [ObservableProperty]
    private ObservableCollection<UserSelectionItem> _availableUsers = new();

    [ObservableProperty]
    private bool _isEdit;

    [ObservableProperty]
    private Guid _eventId;

    [ObservableProperty]
    private string _pageTitle = "Create Event";

    [ObservableProperty]
    private string _submitButtonText = "Create";

    public EventFormViewModel(IApiService apiService)
    {
        _apiService = apiService;
    }

    [RelayCommand]
    public async Task InitializeAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ClearError();

            // Load users
            var usersResult = await _apiService.GetUsersAsync();
            if (usersResult.IsSuccess)
            {
                AvailableUsers.Clear();
                foreach (var user in usersResult.Value!)
                {
                    AvailableUsers.Add(new UserSelectionItem
                    {
                        Id = user.Id,
                        Name = user.FullName,
                        IsSelected = false
                    });
                }
            }
            else
            {
                SetError(usersResult.Error);
                return;
            }

            // If editing, load event data
            if (IsEdit)
            {
                var eventResult = await _apiService.GetEventAsync(EventId);
                if (eventResult.IsSuccess)
                {
                    var eventDto = eventResult.Value!;
                    Name = eventDto.Name;
                    Location = eventDto.Location;
                    StartDate = eventDto.StartDate.Date;
                    StartTime = eventDto.StartDate.TimeOfDay;

                    // Mark selected participants
                    if (eventDto.ParticipantIds != null)
                    {
                        foreach (var user in AvailableUsers)
                        {
                            user.IsSelected = eventDto.ParticipantIds.Contains(user.Id);
                        }
                    }
                }
                else
                {
                    SetError(eventResult.Error);
                }
            }
        }
        catch (Exception ex)
        {
            SetError($"Error initializing form: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task SubmitAsync()
    {
        if (IsBusy) return;

        // Validate form
        if (string.IsNullOrWhiteSpace(Name))
        {
            SetError("Event name is required");
            return;
        }

        if (string.IsNullOrWhiteSpace(Location))
        {
            SetError("Location is required");
            return;
        }

        try
        {
            IsBusy = true;
            ClearError();

            var selectedUserIds = AvailableUsers
                .Where(u => u.IsSelected)
                .Select(u => u.Id)
                .ToList();

            var startDateTime = StartDate.Add(StartTime);

            var eventDto = new CreateEventDto
            {
                Name = Name,
                Location = Location,
                StartDate = startDateTime,
                ParticipantIds = selectedUserIds
            };

            var result = IsEdit 
                ? await _apiService.UpdateEventAsync(EventId, eventDto)
                : await _apiService.CreateEventAsync(eventDto);

            if (result.IsSuccess)
            {
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                SetError(result.Error);
            }
        }
        catch (Exception ex)
        {
            SetError($"Error saving event: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task CancelAsync()
    {
        await Shell.Current.GoToAsync("..");
    }

    public void SetEditMode(Guid eventId)
    {
        IsEdit = true;
        EventId = eventId;
        PageTitle = "Edit Event";
        SubmitButtonText = "Update";
    }

    public class UserSelectionItem : ObservableObject
    {
        private bool _isSelected;

        public Guid Id { get; set; }
        public string? Name { get; set; }

        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }
    }
}
