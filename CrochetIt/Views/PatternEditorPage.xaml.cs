using CrochetIt.Services;
using CrochetIt.ViewModels;
using SkiaSharp;
    using SkiaSharp.Views.Maui;

    namespace CrochetIt.Views;

    public partial class PatternEditorPage : ContentPage
    {
        private SKBitmap? pixelatedBitmap;
        private SKBitmap? originalBitmap;
        private string imageName;
        private readonly IImageProcessingService imageService;
        private readonly IApiService apiService;
        private readonly PatternEditorViewModel viewModel;
        private readonly HttpClient httpClient;

        public PatternEditorPage(IImageProcessingService imageService, IApiService apiService, PatternEditorViewModel viewModel)
        {
            InitializeComponent();
            this.imageService = imageService;
            this.apiService = apiService;
            this.httpClient = new HttpClient();

            BindingContext = viewModel;
            this.viewModel = viewModel;

            viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(viewModel.PixelSize))
                {
                    UpdatePixelation();
                }
            };
    }
        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKCanvas canvas = e.Surface.Canvas;
            canvas.Clear(SKColors.White);

            if (pixelatedBitmap == null)
                return;

            float canvasWidth = e.Info.Width;
            float canvasHeight = e.Info.Height;

            var destRect = new SKRect(
                0,
                0,
                canvasWidth,
                canvasHeight);

            using var paint = new SKPaint
            {
                FilterQuality = SKFilterQuality.None,
                IsAntialias = false
            };

            canvas.DrawBitmap(pixelatedBitmap, destRect, paint);
        }
    private async void OnSelectImageClicked(object sender, EventArgs e)
        {
            var result = await MediaPicker.PickPhotoAsync();
            if (result == null) return;
            imageName = result.FileName;

            using var stream = await result.OpenReadAsync();
            originalBitmap = SKBitmap.Decode(stream);

            UpdatePixelation();

            subirImagen.IsVisible = true;
            subirImagen.IsEnabled = true;
            cancelar.IsVisible = true;
            cancelar.IsEnabled = true;
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            double minDim = Math.Min(width, height);

            patternCanvas.HeightRequest = minDim * 0.75;
            patternCanvas.WidthRequest = minDim * 0.75;
        }

    private async void OnUploadImageClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(nombrePatron.Text))
        {
            await DisplayAlertAsync("Falta asignar nombre", "Indique un nombre para su patrón", "Ok");
            return;
        }
            try
            {
                if (pixelatedBitmap == null)
                {
                    await DisplayAlert("Error", "No hay patrón para subir", "OK");
                    return;
                }

                // Convertir a PNG y generar nombre
                byte[] imagen = imageService.ConvertToPNG(pixelatedBitmap);
                var fileName = Guid.NewGuid() + Path.GetExtension(imageName);

                // Solicitar URL SAS al backend
                var sasUrl = await apiService.PostAsync<string>("upload/generate-url", fileName);
                if (string.IsNullOrWhiteSpace(sasUrl))
                {
                    await DisplayAlert("Error", "No se pudo obtener la URL de subida", "OK");
                    return;
                }

                // Subir el blob usando PUT al SAS URL. Azure Blob REST requiere la cabecera x-ms-blob-type
                using var content = new ByteArrayContent(imagen);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");

                using var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Put, sasUrl)
                {
                    Content = content
                };
                request.Headers.Add("x-ms-blob-type", "BlockBlob");

                var putResponse = await httpClient.SendAsync(request);
                if (!putResponse.IsSuccessStatusCode)
                {
                    await DisplayAlert("Error", $"No se pudo subir la imagen. Status: {putResponse.StatusCode}", "OK");
                    return;
                }

                // Obtener la URL pública del blob (sin el query string SAS) y guardar esa URL
                var publicUrl = sasUrl.Contains('?') ? sasUrl.Split('?')[0] : sasUrl;
                var patronObj = new {
                    Nombre = nombrePatron.Text,
                    ImageUrl = publicUrl
                };

                // Deshabilitar botón mientras se guarda
                subirImagen.IsEnabled = false;
                cancelar.IsEnabled = false;
                subirImagen.Text = "Guardando...";

                // Guardar patrón en el backend
                var created = await apiService.PostAsync<object>("patron/nuevopatron", patronObj);

                await DisplayAlert("Éxito", "Patrón registrado correctamente", "OK");
            }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudo guardar el paatrón: {ex.Message}", "OK");
        }
        finally
        {
            LimpiarFormulario();
        }
    }

    private async void OnCancelarClicked(object sender, EventArgs e)
    {
        bool confirmar = await DisplayAlert("Confirmar",
            "¿Está seguro que desea cancelar? Los datos no guardados se perderán.",
            "Sí", "No");

        if (confirmar)
        {
            LimpiarFormulario();
        }
    }

    private void OnSliderDragCompleted(object sender, EventArgs e)
    {
        if (sender is Slider slider)
        {
            slider.Value = Math.Round(slider.Value);
        }
    }

    private void LimpiarFormulario()
    {
        subirImagen.Text = "Guardar";
        subirImagen.IsEnabled = false;
        subirImagen.IsVisible = false;
        cancelar.IsEnabled = false;
        cancelar.IsVisible = false;
        nombrePatron.Text = string.Empty;
    }

    private void UpdatePixelation()
    {
        if (originalBitmap == null)
            return;

        pixelatedBitmap = imageService.Pixelate(
            originalBitmap,
            (int)viewModel.PixelSize);

        patternCanvas.InvalidateSurface();
    }
}