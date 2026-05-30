using CommunityToolkit.Mvvm.ComponentModel;
using CrochetIt.Services;
using CrochetIt.Services.AuthServices;
using SkiaSharp;
using System.IO;

namespace CrochetIt.ViewModels;

public partial class PatternEditorViewModel : ObservableObject
{

    private SKBitmap _originalBitmap;

    [ObservableProperty]
    private ImageSource processedImage;

    [ObservableProperty]
    private double pixelSize = 10;

    public int PixelSizeInt => (int)Math.Round(PixelSize);
    private readonly IImageProcessingService imageService;
    private readonly IAuthService authService;

    public PatternEditorViewModel(IImageProcessingService imageService, IAuthService authService)
    {
        this.imageService = imageService;
        this.authService = authService;
    }

    partial void OnPixelSizeChanged(double value)
    {
        OnPropertyChanged(nameof(PixelSizeInt));
        UpdatePixelation();
    }

    public void LoadImage(Stream stream)
    {
        _originalBitmap = SKBitmap.Decode(stream);

        UpdatePixelation();
    }

    private void UpdatePixelation()
    {
        if (_originalBitmap == null)
            return;

        var result = imageService.Pixelate(
            _originalBitmap,
            (int)PixelSize);

        ProcessedImage = BitmapToImageSource(result);
    }

    private ImageSource BitmapToImageSource(SKBitmap bitmap)
    {
        using var image = SKImage.FromBitmap(bitmap);
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);

        return ImageSource.FromStream(() =>
        {
            return data.AsStream();
        });
    }
}