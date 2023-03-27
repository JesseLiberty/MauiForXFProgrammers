using ForgetMeNot.Api.Dto;
using ForgetMeNot.Model;

namespace ForgetMeNot.Services;

public interface IBuddyService
{
    Task<List<Buddy>> GetBuddies();
    List<Preference> GetBuddyPreferences(string buddyId);
    List<OccasionModel> GetBuddyOccasions(string buddyId);
    void SaveOccasion(string buddyId, OccasionModel occasionModel);
    List<OccasionModel> GetBuddyOccasionsMock(string buddyId);
}