// ----------------------------------------------------------------------
// <copyright file="ImageComparison.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SkiaSharp;

namespace SoloX.BlazorLayout.E2ETests.Utils
{
    public static class ImageComparison
    {
        public static bool CompareAndGenerateDifferenceImage(string imagePath1, byte[] imageBytes2)
        {
            if (!File.Exists(imagePath1))
            {
                File.WriteAllBytes(imagePath1, imageBytes2);

                return true;
            }

            using var imageStream1 = File.OpenRead(imagePath1);
            using var imageStream2 = new MemoryStream(imageBytes2);

            using var resultStream = new MemoryStream();

            var isDifferences = CompareAndGenerateDifferenceImage(imageStream1, imageStream2, resultStream);

            if (isDifferences)
            {
                var folder = Path.GetDirectoryName(imagePath1) ?? ".";
                var fileName = Path.GetFileNameWithoutExtension(imagePath1);
                var fileExt = Path.GetExtension(imagePath1);

                var imageFile2 = Path.Combine(folder, $"{fileName}-res{fileExt}");
                var resultFile = Path.Combine(folder, $"{fileName}-dif{fileExt}");

                File.WriteAllBytes(imageFile2, imageBytes2);

                resultStream.Position = 0;
                File.WriteAllBytes(resultFile, resultStream.ToArray());
            }

            return isDifferences;
        }

        public static bool CompareAndGenerateDifferenceImage(Stream imageStream1, Stream imageStream2, Stream resultStream)
        {
            var isDifferences = false;

            // Load both images
            using var bitmap1 = SKBitmap.Decode(imageStream1);
            using var bitmap2 = SKBitmap.Decode(imageStream2);

            var maxWidth = Math.Max(bitmap1.Width, bitmap2.Width);
            var maxHeight = Math.Max(bitmap1.Height, bitmap2.Height);
            var minWidth = Math.Min(bitmap1.Width, bitmap2.Width);
            var minHeight = Math.Min(bitmap1.Height, bitmap2.Height);

            // Create a result image to hold the differences
            using (var resultBitmap = new SKBitmap(maxWidth, maxHeight))
            {
                for (var y = 0; y < minHeight; y++)
                {
                    for (var x = 0; x < minWidth; x++)
                    {
                        // Get the pixel color from each image
                        var pixel1 = bitmap1.GetPixel(x, y);
                        var pixel2 = bitmap2.GetPixel(x, y);

                        // Compare the pixels
                        if (pixel1 != pixel2)
                        {
                            // Highlight differences with a red color
                            resultBitmap.SetPixel(x, y, new SKColor(255, 0, 0)); // Red for difference
                            isDifferences = true;
                        }
                        else
                        {
                            // Copy the original pixel if no difference
                            resultBitmap.SetPixel(x, y, pixel1);
                        }
                    }
                    for (var x = minWidth; x < maxWidth; x++)
                    {
                        // Highlight differences with a red color
                        resultBitmap.SetPixel(x, y, new SKColor(255, 0, 0)); // Red for difference
                        isDifferences = true;
                    }
                }
                for (var y = minHeight; y < maxHeight; y++)
                {
                    for (var x = 0; x < maxWidth; x++)
                    {
                        // Highlight differences with a red color
                        resultBitmap.SetPixel(x, y, new SKColor(255, 0, 0)); // Red for difference
                        isDifferences = true;
                    }
                }

                if (isDifferences)
                {
                    // Save the resulting image with highlighted differences
                    using var image = SKImage.FromBitmap(resultBitmap);
                    using var data = image.Encode(SKEncodedImageFormat.Png, 100);

                    data.SaveTo(resultStream);
                }
            }

            return isDifferences;
        }
    }
}
