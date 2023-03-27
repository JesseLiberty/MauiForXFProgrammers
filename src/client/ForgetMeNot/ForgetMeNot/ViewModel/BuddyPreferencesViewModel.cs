using ForgetMeNot.Model;
using ForgetMeNot.Services;

namespace ForgetMeNot.ViewModel;

[ObservableObject]
[QueryProperty(nameof(BuddyId), "id")]
[QueryProperty(nameof(BuddyName), "buddyname")]
public partial class BuddyPreferencesViewModel
{
    [ObservableProperty] private List<Preference> buddyPreferences;
    [ObservableProperty] private bool showActivityIndicator;
    [ObservableProperty] private string buddyName;

    private readonly IBuddyService buddyService;

    public BuddyPreferencesViewModel(IBuddyService buddyService)
    {
        ShowActivityIndicator = true;
        this.buddyService = buddyService;
    }

    private string buddyId;
    public string BuddyId
    {
        get => buddyId;
        set
        {
            SetProperty(ref buddyId, value);
            BuddyPreferences = buddyService.GetBuddyPreferences(buddyId);
            ShowActivityIndicator = false;

        }
    }
}

