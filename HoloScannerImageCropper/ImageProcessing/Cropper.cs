using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Advanced;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;
using System.Threading.Tasks;

namespace HoloScannerImageCropper.ImageProcessing
{
    public static class Cropper
    {
        public static void CropMax(List<string> imagesPaths, string destinationDirectiory)
        {
            Parallel.ForEach(imagesPaths, (path) =>
            {
                using (Image<Rgba32> image = Image.Load<Rgba32>(path))
                {
                    Rectangle bounds = GetVisibleBounds(image);
                    image.Mutate(img =>
                    {
                        img.Crop(bounds);
                    });
                    image.Save(Path.Combine(destinationDirectiory, Path.GetFileName(path)));
                }
            });
        }

        public static Rectangle GetVisibleBounds(Image<Rgba32> image)
        {
            int left = int.MaxValue;
            int right =  int.MinValue;
            int top = int.MaxValue;
            int bottom =  int.MinValue;

            for (int y = 0; y < image.Height; y++)
            {
                var row = image.GetPixelRowSpan(y);
                for (int x = 0; x < image.Width; x++)
                {
                    var pixel = row[x];
                    if (pixel.A == 0)
                        continue;
                    if (x < left)
                        left = x;
                    if (x > right)
                        right = x;
                    if (y < top)
                        top = y;
                    if (y > bottom)
                        bottom = y;
                }
            }

            int width = right - left;
            int height = bottom - top;
            return new Rectangle(left, top, width, height);
        }
    }
}

