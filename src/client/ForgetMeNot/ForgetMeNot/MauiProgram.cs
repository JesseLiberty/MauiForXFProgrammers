using ForgetMeNot.ApiClient;
using ForgetMeNot.Services;
namespace ForgetMeNot;

public static class MauiProgram
{
  public static MauiApp CreateMauiApp()
  {
    var builder = MauiApp.CreateBuilder();
    builder
      .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
      {
        fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
        fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
      });


    // Pages
    builder.Services.AddTransient<About>();
    builder.Services.AddTransient<BuddyDetail>();
    builder.Services.AddTransient<BuddyList>();
    builder.Services.AddTransient<BuddyOccasions>();
    builder.Services.AddTransient<BuddyPreferences>();
    builder.Services.AddTransient<NewOccasion>();
    builder.Services.AddTransient<View.Preferences>();
    builder.Services.AddTransient<LoginPage>();
    builder.Services.AddTransient<CreateAccount>();
    builder.Services.AddTransient<MainPage>();

    // Services
    // TODO: use configuration to set the base url instead
    //var apiClient = new Client("https://forgetmenotglobal.azurewebsites.net/");
    var apiClient = new Client("https://forgetmenotapi20230113114628.azurewebsites.net/");

    builder.Services.AddSingleton(apiClient);
    builder.Services.AddTransient<UtilService>();
    builder.Services.AddSingleton<IBuddyService, BuddyService>();
    builder.Services.AddTransient<IPreferencesService, PreferencesService>();
    builder.Services.AddTransient<AccountService>();

    // ViewModel
    builder.Services.AddTransient<AboutViewModel>();
    builder.Services.AddTransient<BuddyListViewModel>();
    builder.Services.AddTransient<BuddyDetailViewModel>();
    builder.Services.AddTransient<BuddyPreferencesViewModel>();
    builder.Services.AddTransient<BuddyOccasionsViewModel>();
    builder.Services.AddTransient<PreferencesViewModel>();
    builder.Services.AddTransient<NewOccasionViewModel>();
    builder.Services.AddTransient<LoginViewModel>();
    builder.Services.AddTransient<CreateAccountViewModel>();

    return builder.Build();
  }
}
