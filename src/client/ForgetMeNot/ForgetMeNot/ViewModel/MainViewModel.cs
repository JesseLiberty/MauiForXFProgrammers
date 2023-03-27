using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Messaging;


namespace ForgetMeNot.ViewModel;
[ObservableObject]
public partial class MainViewModel 
{
    [ObservableProperty] private bool flowerIsVisible = true;
    [ObservableProperty] private string fullName;
    [ObservableProperty] private string favoriteFlower = "flower.png";

    public MainViewModel() 
    {

    }



    [RelayCommand]
    private void ToggleFlowerVisibility()
    {
        FlowerIsVisible = !FlowerIsVisible;

    }

    [RelayCommand]
    private void ImageTapped()
    {
        FlowerIsVisible = !FlowerIsVisible;
    }

    [RelayCommand]
    private async Task GoToLoginPage()
    {
        await Shell.Current.GoToAsync("aboutpage");
    }

    

}

