using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;
namespace CrochetIt.Services
{
    public class ImageProcessingService : IImageProcessingService
    {
        public bool[,] ProcessImage(Stream imageStream, int gridSize)
        {
            var original = SKBitmap.Decode(imageStream);

            var resized = original.Resize(new SKImageInfo(gridSize, gridSize), SKSamplingOptions.Default);

            var pattern = new bool[gridSize, gridSize];

            for(int y = 0; y < gridSize; y++)
            {
                for (int x = 0; x < gridSize; x++)
                {
                    SKColor pixel = resized.GetPixel(x, y);
                    float luminance = 0.2126f*pixel.Red + 0.7152f*pixel.Green + 0.0722f*pixel.Blue;

                    pattern[y, x] = (luminance < 128 ? true : false);
                }
            }

            return pattern;
        }
    }
}
