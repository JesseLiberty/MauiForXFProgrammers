using ForgetMeNot.ApiClient;

namespace ForgetMeNot.Services
{
    public class UtilService : IUtilService
    {
        readonly Client apiClient;
        
        public UtilService(Client apiClient)
        {
            this.apiClient = apiClient;
        }

        public Task<string> ApiVersion()
        {
            return apiClient.Version();
        }
    }
}
