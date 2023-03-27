namespace ForgetMeNot.Model;

[ObservableObject]
public partial class OccasionModel 
{
    [ObservableProperty] private string name;
    [ObservableProperty] private DateTime date;
    [ObservableProperty] private int numDaysToNotify;
}

