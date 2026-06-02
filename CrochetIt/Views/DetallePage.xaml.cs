using CommunityToolkit.Maui.Storage;
using CrochetIt.Models;
using CrochetIt.Services;

namespace CrochetIt.Views;

[QueryProperty(nameof(PatronId), "id")]
public partial class DetallePage : ContentPage
{
    private readonly IApiService apiService;
    private Patron? patron;
    public DetallePage(IApiService apiService)
    {
        InitializeComponent();
        this.apiService = apiService;
    }

    public int PatronId { get; set; }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarPatron(PatronId);
    }
    private async Task CargarPatron(int id)
    {
        var patron = await apiService.GetByIdAsync<Patron>("patron/obtenerpatron", id);

        if (patron != null)
        {
            lblNombre.Text = $"{patron.Nombre} por {patron.UserName}";
            if (!string.IsNullOrEmpty(patron.ImageUrl))
            {
                imgPatron.Source = patron.ImageUrl;
            }
        }

        this.patron = patron;
    }

    private async void OnDescargarClicked(object sender, EventArgs e)
    {
        try
        {
            if (patron == null) return;
            var httpClient = new HttpClient();
            var bytes = await httpClient.GetByteArrayAsync(patron.ImageUrl);
            var stream = new MemoryStream(bytes);

            await FileSaver.Default.SaveAsync("patron.png", stream);
        } catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudo descargar la imagen: {ex.Message}", "OK");
        }
    }
}