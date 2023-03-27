using ForgetMeNot.Services;

namespace ForgetMeNot.View;

public partial class BuddyOccasions : ContentPage
{

    public BuddyOccasions(BuddyOccasionsViewModel vm)
	{
        BindingContext = vm;
        InitializeComponent();
	}
}

