    using CrochetIt.Services;
    using SkiaSharp;
    using SkiaSharp.Views.Maui;

    namespace CrochetIt.Views;

    public partial class PatternEditorPage : ContentPage
    {
        private bool[,]? _pattern;
    private readonly IImageProcessingService imageService;
    private readonly IApiService apiService;

    public PatternEditorPage(IImageProcessingService imageService, IApiService apiService)
        {
            InitializeComponent();
            this.imageService = imageService;
            this.apiService = apiService;
        }

        // Llamar esto después de procesar la imagen
        public void LoadPattern(bool[,] pattern)
        {
            _pattern = pattern;
            patternCanvas.InvalidateSurface(); // fuerza redibujar el canvas
        }

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKCanvas canvas = e.Surface.Canvas;
            canvas.Clear(SKColors.White);

            if (_pattern == null) return;

            int gridSize = _pattern.GetLength(0);
            float cellSize = (float) Math.Min(e.Info.Width, e.Info.Height) / gridSize;

            for (int y = 0; y < gridSize; y++)
            {
                for (int x = 0; x < gridSize; x++)
                {
                    SKColor color = _pattern[y, x] ? SKColors.Black : SKColors.White;

                    using var paint = new SKPaint { Color = color };

                    canvas.DrawRect(new SKRect(
                        left: x * cellSize,
                        top: y * cellSize,
                        right: (x + 1) * cellSize,
                        bottom: (y + 1) * cellSize
                    ), paint);
                }
            }

            // Dibuja la cuadrícula encima
            using var gridPaint = new SKPaint
            {
                Color = SKColors.LightGray,
                IsStroke = false,
                StrokeWidth = 1.0f
            };

            for (int y = 0; y < gridSize; y++)
                for (int x = 0; x < gridSize; x++)
                    canvas.DrawRect(new SKRect(
                        x * cellSize, y * cellSize,
                        (x + 1) * cellSize, (y + 1) * cellSize
                    ), gridPaint);
        }
        private async void OnSelectImageClicked(object sender, EventArgs e)
        {
            var result = await MediaPicker.PickPhotoAsync();
            if (result == null) return;

            int gridSize = int.Parse(gridSizePicker.SelectedItem?.ToString() ?? "64");

            using var stream = await result.OpenReadAsync();
            var pattern = imageService.ProcessImage(stream, gridSize);

            LoadPattern(pattern);
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            double minDim = Math.Min(width, height);

            patternCanvas.HeightRequest = minDim * 0.75;
            patternCanvas.WidthRequest = minDim * 0.75;
        }

    private void OnUploadImageClicked(object sender, EventArgs e)
    {

    }
}