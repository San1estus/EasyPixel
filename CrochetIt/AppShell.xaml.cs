using CrochetIt.Services.AuthServices;
using CrochetIt.Views;

namespace CrochetIt
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(PatternEditorPage), typeof(PatternEditorPage));
            Routing.RegisterRoute(nameof(CatalogoPage), typeof(CatalogoPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            Routing.RegisterRoute(nameof(DetallePage), typeof(DetallePage));
        }
        protected override async void OnHandlerChanged()
        {
            base.OnHandlerChanged();

            var authService = Handler?.MauiContext?.Services.GetService<IAuthService>();
            if (authService != null)
            {
                bool isAuthenticated = await authService.IsAuthenticatedAsync();
                if (!isAuthenticated)
                {
                    await GoToAsync(nameof(LoginPage));
                }
            }
        }
    }
}