using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace ForgetMeNot.View;

public partial class MainPage : ContentPage {

    private bool canBeSeen = true;

    private MainViewModel vm = new MainViewModel();

    public MainPage()
    {
        InitializeComponent();
        BindingContext = vm;
    }

    public void OnImageTapped(Object sender, EventArgs e)
    {
        // BigFlowerImage.IsVisible = false;
    }

    private void Button_OnClicked(object sender, EventArgs e)
    {
        canBeSeen = !canBeSeen;
        //BigFlowerImage.IsVisible = canBeSeen;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        vm.FullName = "Jesse Liberty";
        vm.FavoriteFlower = "flower.png";
    }

    private async void ShowSnackBar(object sender, EventArgs e)
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        var message = "Your pushed the button.";
        var dismissalText = "Click Here to Close the SnackBar";
        TimeSpan duration = TimeSpan.FromSeconds(10);

        Action action = async () =>
              await DisplayAlert(
            "Snackbar Dismissed!",
            "The user has dismissed the snackbar",
            "OK");

        var snackbarOptions = new SnackbarOptions
        {
            BackgroundColor = Colors.Red,
            TextColor = Colors.Yellow,
            ActionButtonTextColor = Colors.Black,
            CornerRadius = new CornerRadius(20),
            Font = Microsoft.Maui.Font.SystemFontOfSize(14),
            ActionButtonFont = Microsoft.Maui.Font.SystemFontOfSize(14)
        };

        var snackbar = Snackbar.Make(
          message,
          action,
          dismissalText,
          duration,
          snackbarOptions);

        await snackbar.Show(cancellationTokenSource.Token);
    }

}

