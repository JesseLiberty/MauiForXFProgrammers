namespace ForgetMeNot.View;

public partial class CreateAccount : ContentPage
{
	public CreateAccount(CreateAccountViewModel vm)
    {
        BindingContext = vm;
		    InitializeComponent();
	}
}