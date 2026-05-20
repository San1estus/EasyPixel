using CrochetIt.Views;

namespace CrochetIt
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(PatternEditorPage), typeof(PatternEditorPage));
        }
    }
}
