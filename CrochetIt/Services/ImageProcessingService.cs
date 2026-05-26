using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;
namespace CrochetIt.Services
{
    public class ImageProcessingService : IImageProcessingService
    {

        public SKBitmap Pixelate(SKBitmap original, int pixelSize)
        {
            int width = original.Width;
            int height = original.Height;

            int smallWidth = Math.Max(1, width / pixelSize);
            int smallHeight = Math.Max(1, height / pixelSize);

            // Imagen reducida
            SKBitmap smallBitmap = new SKBitmap(smallWidth, smallHeight);

            using (var canvas = new SKCanvas(smallBitmap))
            {
                canvas.DrawBitmap(
                    original,
                    new SKRect(0, 0, smallWidth, smallHeight),
                    new SKPaint
                    {
                        FilterQuality = SKFilterQuality.None
                    });
            }

            // Imagen pixelada
            SKBitmap pixelated = new SKBitmap(width, height);

            using (var canvas = new SKCanvas(pixelated))
            {
                canvas.DrawBitmap(
                    smallBitmap,
                    new SKRect(0, 0, width, height),
                    new SKPaint
                    {
                        FilterQuality = SKFilterQuality.None,
                        IsAntialias = false
                    });
            }

            return pixelated;
        }

        public byte[] ConvertToPNG(bool[,] imageMat)
        {
            int size = imageMat.GetLength(0);
            SKBitmap patron = new SKBitmap(size, size);
            for(int y = 0; y < size; y++)
            {
                for(int x = 0;x < size; x++)
                {
                    patron.SetPixel(x, y, imageMat[y,x] ? SKColors.Black : SKColors.White);
                }
            }
            var imagen = SKImage.FromBitmap(patron).Encode(SKEncodedImageFormat.Png, 100);

            return imagen.ToArray();
        }
        public byte[] ConvertToPNG(SKBitmap bitmap)
        {
            using var image = SKImage.FromBitmap(bitmap);

            using var data = image.Encode(
                SKEncodedImageFormat.Png,
                100);

            return data.ToArray();
        }
    }
}
