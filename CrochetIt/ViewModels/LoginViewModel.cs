using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CrochetIt.Services.AuthServices;
using CrochetIt.Views;

namespace CrochetIt.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly IAuthService authService;
    private readonly IServiceProvider serviceProvider;
    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    [ObservableProperty]
    private bool isLoading = false;

    public LoginViewModel(IAuthService authService, IServiceProvider serviceProvider)
    {
        this.authService = authService;
        this.serviceProvider = serviceProvider;
    }

    [RelayCommand]
    public async Task Login()
    {
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Email y contraseña son requeridos";
            return;
        }

        IsLoading = true;
        ErrorMessage = string.Empty;

        try
        {
            var result = await authService.LoginAsync(Email, Password);

            if (result != null)
            {
                Application.Current.MainPage = new AppShell();
            }
            else
            {
                ErrorMessage = "Email o contraseña inválidos";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    public async Task GoToRegister()
    {
        var registerPage = serviceProvider.GetRequiredService<RegisterPage>();
        await Application.Current.MainPage.Navigation.PushAsync(registerPage);
    }
}