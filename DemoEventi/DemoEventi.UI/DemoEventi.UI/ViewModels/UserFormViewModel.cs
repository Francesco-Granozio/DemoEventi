using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DemoEventi.Application.DTOs;
using DemoEventi.UI.Services;
using System.Collections.ObjectModel;

namespace DemoEventi.UI.ViewModels;

public partial class UserFormViewModel : BaseViewModel
{
    private readonly IApiService _apiService;

    [ObservableProperty]
    private string? _firstName;

    [ObservableProperty]
    private string? _lastName;

    [ObservableProperty]
    private ObservableCollection<InterestSelectionItem> _availableInterests = new();

    [ObservableProperty]
    private bool _isEdit;

    [ObservableProperty]
    private Guid _userId;

    [ObservableProperty]
    private string _pageTitle = "Create User";

    [ObservableProperty]
    private string _submitButtonText = "Create";

    public UserFormViewModel(IApiService apiService)
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

            // Load interests
            var interestsResult = await _apiService.GetInterestsAsync();
            if (interestsResult.IsSuccess)
            {
                AvailableInterests.Clear();
                foreach (var interest in interestsResult.Value!)
                {
                    AvailableInterests.Add(new InterestSelectionItem
                    {
                        Id = interest.Id,
                        Name = interest.Name,
                        IsSelected = false
                    });
                }
            }
            else
            {
                SetError(interestsResult.Error);
                return;
            }

            // If editing, load user data
            if (IsEdit)
            {
                var userResult = await _apiService.GetUserAsync(UserId);
                if (userResult.IsSuccess)
                {
                    var user = userResult.Value!;
                    FirstName = user.FirstName;
                    LastName = user.LastName;

                    // Mark selected interests
                    if (user.InterestIds != null)
                    {
                        foreach (var interest in AvailableInterests)
                        {
                            interest.IsSelected = user.InterestIds.Contains(interest.Id);
                        }
                    }
                }
                else
                {
                    SetError(userResult.Error);
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

        try
        {
            IsBusy = true;
            ClearError();

            var selectedInterestIds = AvailableInterests
                .Where(i => i.IsSelected)
                .Select(i => i.Id)
                .ToList();

            var result = IsEdit
                ? await _apiService.UpdateUserAsync(UserId, new UpdateUserDto
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    InterestIds = selectedInterestIds
                })
                : await _apiService.CreateUserAsync(new CreateUserDto
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    InterestIds = selectedInterestIds
                });

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
            SetError($"Error saving user: {ex.Message}");
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

    public void SetEditMode(Guid userId)
    {
        IsEdit = true;
        UserId = userId;
        PageTitle = "Edit User";
        SubmitButtonText = "Update";
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
