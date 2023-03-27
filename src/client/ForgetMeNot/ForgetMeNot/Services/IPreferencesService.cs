using ForgetMeNot.Api.Domain;
using ForgetMeNot.Model;

namespace ForgetMeNot.Services;

public interface IPreferencesService
{
    Task<List<Preference>> GetPreferences();
    Task Save(List<Preference> preferences);
}