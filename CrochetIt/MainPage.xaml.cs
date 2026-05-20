using CrochetIt.Views;

namespace CrochetIt
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnCrearPatronClicked(object? sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(PatternEditorPage));
        }

        private async void OnCatalogoClicked(object? sender, EventArgs e) {
            throw new NotImplementedException();
        }
    }
}
    