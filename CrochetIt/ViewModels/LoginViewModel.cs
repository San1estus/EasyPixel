using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CrochetIt.Services.AuthServices;
using CrochetIt.Views;

namespace CrochetIt.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly IAuthService authService;

    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    [ObservableProperty]
    private bool isLoading = false;

    public LoginViewModel(IAuthService authService)
    {
        this.authService = authService;
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
                await Shell.Current.GoToAsync(nameof(MainPage));
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
        await Shell.Current.GoToAsync(nameof(RegisterPage));
    }
}