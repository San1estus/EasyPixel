using Microsoft.Extensions.DependencyInjection;
using CrochetIt.Services.AuthServices;
using CrochetIt.Views;

namespace CrochetIt
{
    public partial class App : Application
    {
        private readonly IAuthService authService;
        private readonly IServiceProvider serviceProvider;

        public App(IAuthService authService, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            this.authService = authService;
            this.serviceProvider = serviceProvider;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }

        protected override async void OnStart()
        {
            base.OnStart();

            bool isAuthenticated = await authService.IsAuthenticatedAsync();

            if (!isAuthenticated)
            {
                var loginPage = serviceProvider.GetRequiredService<LoginPage>();
                MainPage = new NavigationPage(loginPage);
            }
        }
    }
}