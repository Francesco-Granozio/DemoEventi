using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DemoEventi.Application.DTOs;
using DemoEventi.UI.Services;
using System.Collections.ObjectModel;

namespace DemoEventi.UI.ViewModels;

public partial class InterestsViewModel : BaseViewModel
{
    private readonly IApiService _apiService;
    private readonly IAuthService _authService;

    [ObservableProperty]
    private ObservableCollection<InterestSelectionItem> _interests = new();

    [ObservableProperty]
    private Guid _currentUserId;

    public InterestsViewModel(IApiService apiService, IAuthService authService)
    {
        _apiService = apiService;
        _authService = authService;
        Title = "My Interests";

        // Get current user ID from authentication service
        Task.Run(async () => await SetCurrentUserAsync());
    }

    [RelayCommand]
    public async Task LoadInterestsAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            ClearError();

            // Load all interests
            var interestsResult = await _apiService.GetInterestsAsync();
            if (!interestsResult.IsSuccess)
            {
                SetError(interestsResult.Error);
                return;
            }

            // Load current user's interests
            UserDto? currentUser = null;
            if (CurrentUserId != Guid.Empty)
            {
                var userResult = await _apiService.GetUserAsync(CurrentUserId);
                if (userResult.IsSuccess)
                {
                    currentUser = userResult.Value;
                }
            }

            var userInterestIds = currentUser?.InterestIds?.ToHashSet() ?? new HashSet<Guid>();

            Interests.Clear();
            foreach (var interest in interestsResult.Value!)
            {
                var selectionItem = new InterestSelectionItem
                {
                    Id = interest.Id,
                    Name = interest.Name,
                    IsSelected = userInterestIds.Contains(interest.Id)
                };
                selectionItem.PropertyChanged += OnInterestSelectionChanged;
                Interests.Add(selectionItem);
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
    public async Task SaveInterestsAsync()
    {
        if (CurrentUserId == Guid.Empty) return;

        try
        {
            IsBusy = true;
            ClearError();

            var selectedInterestIds = Interests
                .Where(i => i.IsSelected)
                .Select(i => i.Id)
                .ToList();

            // Get current user data
            var userResult = await _apiService.GetUserAsync(CurrentUserId);
            if (!userResult.IsSuccess)
            {
                SetError("Could not load user data");
                return;
            }

            var updateDto = new UpdateUserDto
            {
                FirstName = userResult.Value!.FirstName,
                LastName = userResult.Value.LastName,
                Email = userResult.Value.Email,
                ProfileImageUrl = userResult.Value.ProfileImageUrl,
                InterestIds = selectedInterestIds
            };

            var updateResult = await _apiService.UpdateUserAsync(CurrentUserId, updateDto);
            if (updateResult.IsSuccess)
            {
                await Microsoft.Maui.Controls.Application.Current!.MainPage!.DisplayAlert(
                    "Success", "Your interests have been updated!", "OK");
            }
            else
            {
                SetError(updateResult.Error);
            }
        }
        catch (Exception ex)
        {
            SetError($"Error saving interests: {ex.Message}");
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

    private async void OnInterestSelectionChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(InterestSelectionItem.IsSelected))
        {
            // Automatically save when selection changes
            await SaveInterestsAsync();
        }
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

    public class InterestSelectionItem : ObservableObject
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
