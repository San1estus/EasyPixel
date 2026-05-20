using System;
using System.Collections.Generic;
using System.Text;

namespace CrochetIt.Services
{
    public interface IImageProcessingService
    {
        public bool[,] ProcessImage(Stream imageStream, int gridSize);
    }
}
