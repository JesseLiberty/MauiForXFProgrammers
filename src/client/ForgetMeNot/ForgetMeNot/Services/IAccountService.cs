using ForgetMeNot.Api.Dto;

namespace ForgetMeNot.Services;

public interface IAccountService
{
    Task CreateAccount(AccountCreateRequest accountCreateRequest);
    Task GetNewPassword();
    Task Login(LoginRequest request);
    bool IsLoggedIn();
}