using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrochetIt.Services
{
    public interface IImageProcessingService
    {
        public SKBitmap Pixelate(SKBitmap original, int pixelSize);
        public byte[] ConvertToPNG(bool[,] imageMat);
        public byte[] ConvertToPNG(SKBitmap bitmap);
    }
}
