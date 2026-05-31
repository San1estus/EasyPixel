using CrochetIt.Views;

namespace CrochetIt
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent(); 
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(PatternEditorPage), typeof(PatternEditorPage));
            Routing.RegisterRoute(nameof(CatalogoPage), typeof(CatalogoPage));
            Routing.RegisterRoute(nameof(DetallePage), typeof(DetallePage));
        }
    }
}
