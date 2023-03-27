using ForgetMeNot.Model;
using ForgetMeNot.Services;

namespace ForgetMeNot.ViewModel;

[ObservableObject]
[QueryProperty(nameof(BuddyId), "id")]
public partial class BuddyOccasionsViewModel
{
    [ObservableProperty] private List<OccasionModel> occasions;
    private readonly IBuddyService buddyService;

    public BuddyOccasionsViewModel(IBuddyService buddyService)
    {
        this.buddyService = buddyService;
    }


    private string buddyId;

    public string BuddyId
    {
        get => buddyId;
        set
        {
            SetProperty(ref buddyId, value);
            Occasions = buddyService.GetBuddyOccasions(buddyId);
        }
    }


     [RelayCommand]
    public async Task AddOccasion()
    {
        await Shell.Current.GoToAsync($"newoccasion?id={BuddyId}");

    }
}

