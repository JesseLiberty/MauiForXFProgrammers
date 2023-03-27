using CommunityToolkit.Mvvm.Messaging;
using ForgetMeNot.Api.Dto;
using ForgetMeNot.Services;

namespace ForgetMeNot.ViewModel;

[ObservableObject]
public partial class LoginViewModel {
    private AccountService accountService;
    [ObservableProperty] private string loginName;
    [ObservableProperty] private string password;
    [ObservableProperty] private bool showActivityIndicator = false;

    public LoginViewModel(AccountService accountService)
    {
        this.accountService = accountService;
    }


    [RelayCommand]
    public async Task DoLogin()
    {

        try
        {
            LoginRequest loginRequest = new LoginRequest
            {
                Username = LoginName,
                Password = Password
            };

            ShowActivityIndicator = true;
            await accountService.Login(loginRequest);
            ShowActivityIndicator = false;

            if (accountService.IsLoggedIn())
            {
                Application.Current.MainPage = new AppShell();
                await Shell.Current.GoToAsync("mainpage");
            }
            else
            {

                await Application.Current.MainPage.DisplayAlert("Login failure",
                    "Your username and password do not match our records", "Ok");
                ShowActivityIndicator = false;
            }

        }
        catch (Exception exception)
        {
            await Application.Current.MainPage.DisplayAlert("Authorization failure",
                "Your username and password do not match our records", "Ok");
            ShowActivityIndicator = false;

            Console.WriteLine(exception);
        }


    }

    [RelayCommand]
    public async Task ForgotPassword()
    {
        var message = new PasswordMessage(LoginName);
        WeakReferenceMessenger.Default.Send(message);
    }

    [RelayCommand]
    public async Task DoCreateAccount()
    {
        try
        {
            Application.Current.MainPage = new AppShell();

            await Shell.Current.GoToAsync($"createaccount");

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}

[ObservableObject]
public partial class PasswordMessage 
{
    [ObservableProperty] private string loginName;
    public PasswordMessage(string name)
    {
        LoginName = name;
    }



    [ObservableProperty] string info = $"Please set a robust password. ";

}
