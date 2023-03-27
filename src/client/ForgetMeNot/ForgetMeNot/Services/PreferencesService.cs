using ForgetMeNot.Api.Domain;
using ForgetMeNot.Api.Dto;
using ForgetMeNot.ApiClient;
using ForgetMeNot.Model;

namespace ForgetMeNot.Services;

public partial class PreferencesService : IPreferencesService
{
  readonly Client apiClient;

  public PreferencesService(Client apiClient)
  {
    this.apiClient = apiClient;
  }

  public async Task<List<Preference>> GetPreferences()
  {
    try
    {
      var response = await apiClient.GetProfile();
      return response?.Preferences.Select(p => new Preference
      {
        PreferencePrompt = p.PreferencePrompt,
        PreferenceValue = p.PreferenceValue

      }).ToList();
    }
    catch (Exception e)
    {
      await Application.Current.MainPage.DisplayAlert("Preferences error",
          "We were unable to get your preferences", "Ok");

      Console.WriteLine(e);
    }

    return null;

  }

  public async Task Save(List<Preference> preferences)
  {

    var response = await apiClient.GetProfile();
    var fullName = response?.FullName;

    var profileUpdateRequest = new ProfileUpdateRequest()
    {
      FullName = fullName,
      Preferences = preferences.Select(p => new UserPreference()
      {
        PreferencePrompt = p.PreferencePrompt,
        PreferenceValue = p.PreferenceValue
      }).ToList()
    };

    await apiClient.UpdateProfile(profileUpdateRequest);
  }

}

