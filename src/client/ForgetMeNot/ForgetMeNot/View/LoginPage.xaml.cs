using CommunityToolkit.Maui.Core.Views;
using CommunityToolkit.Mvvm.Messaging;

namespace ForgetMeNot.View;

public partial class LoginPage : ContentPage {
    public LoginPage(LoginViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        passwordEntry.Text = "";

        WeakReferenceMessenger.Default.Register<PasswordMessage>(this, async (sender, message) =>
        {

            await Application.Current.MainPage.DisplayAlert($"New Password for {message.LoginName}", message.Info, "Ok");

        });


    }
}