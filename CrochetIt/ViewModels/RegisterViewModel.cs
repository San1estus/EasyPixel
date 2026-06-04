using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CrochetIt.Services.AuthServices;
using CrochetIt.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrochetIt.ViewModels
{
    public partial class RegisterViewModel : ObservableObject
    {
        private readonly IAuthService authService;
        private readonly IServiceProvider serviceProvider;
        [ObservableProperty]
        private string email = string.Empty;

        [ObservableProperty]
        private string username = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;

        [ObservableProperty]
        private string errorMessage = string.Empty;

        [ObservableProperty]
        private bool isLoading = false;

        public RegisterViewModel(IAuthService authService, IServiceProvider serviceProvider)
        {
            this.authService = authService;
            this.serviceProvider = serviceProvider;
        }

        [RelayCommand]
        public async Task Register()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Todos los campos son requeridos";
                return;
            }

            IsLoading = true;
            ErrorMessage = string.Empty;

            try
            {
                var result = await authService.RegisterAsync(Email, Username, Password);

                if (result)
                {
                    var loginPage = serviceProvider.GetRequiredService<LoginPage>();
                    await Application.Current.MainPage.Navigation.PushAsync(loginPage);
                }

                else
                {
                    ErrorMessage = "No se pudo registrar el usuario";
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
        public async Task GoToLogin()
        {
            var loginPage = serviceProvider.GetRequiredService<LoginPage>();
            await Application.Current.MainPage.Navigation.PushAsync(loginPage);
        }
    }
}