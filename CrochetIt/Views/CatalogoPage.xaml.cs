using CrochetIt.Models;
using CrochetIt.Services;

namespace CrochetIt.Views;

public partial class CatalogoPage : ContentPage
{
    private readonly IApiService apiService;

    public CatalogoPage(IApiService apiService)
	{
		InitializeComponent();
        this.apiService = apiService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        CargarCatalogo();
    }

    private async void CargarCatalogo()
    {
        var result = await apiService.GetAsync<List<Patron>>("patron/todos");

        CatalogoCollection.ItemsSource = result;
    }

    private async void OnPatronTapped(object sender, TappedEventArgs e)
    {
        var layout = sender as VerticalStackLayout;
        var patron = layout.BindingContext as Patron;
        await Shell.Current.GoToAsync($"{nameof(DetallePage)}?id={patron.Id}");
    }
}