using ForgetMeNot.Api.Domain;
using ForgetMeNot.Model;
using ForgetMeNot.Services;

namespace ForgetMeNot.ViewModel;

[ObservableObject]
public partial class PreferencesViewModel
{
    [ObservableProperty] private List<Preference> preferences;

    private readonly IPreferencesService preferencesService;

    public PreferencesViewModel(IPreferencesService preferencesService)
    {
        this.preferencesService = preferencesService;
    }

    public async Task Init()
    {
        Preferences = await preferencesService.GetPreferences();
    }

    [RelayCommand]
    private async Task SavePreferencesAsync()
    {
        await preferencesService.Save(preferences);
    }
    
}

