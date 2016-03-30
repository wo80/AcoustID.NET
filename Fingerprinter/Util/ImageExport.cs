// -----------------------------------------------------------------------
// <copyright file="PngExport.cs" company="">
// </copyright>
// -----------------------------------------------------------------------

namespace Fingerprinter.Util
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;

    using Chroma = AcoustID.Chromaprint;

    /// <summary>
    /// Export Chroma image to a PNG file.
    /// </summary>
    /// <remarks>
    /// Usage:
    /// <code>
    /// var image = ImageGenerator.ComputeChromagram(new NAudioDecoder("audio.wav"));
    /// ImageExport.Save(image, "chromagram.png");
    ///
    /// var image = ImageGenerator.ComputeSpectrogram(new NAudioDecoder("audio.wav"));
    /// ImageExport.Save(image, "spectrogram.png", 0.5);
    /// </code>
    /// </remarks>
    public static class ImageExport
    {
        /// <summary>
        /// Export image to a PNG file
        /// </summary>
        public static void Save(Chroma.Image data, string fileName, double power = 1.0)
        {
            int numColors = 6;
            int[,] colors = {
			    { 0, 0, 0 },
			    { 218, 38, 0 },
			    { 221, 99, 0 },
			    { 255, 253, 0 },
			    { 255, 254, 83 },
			    { 255, 255, 200 },
			    { 255, 255, 255 },
			    { 0, 0, 0 }, // TODO: prevent index out of range exception below
		    };

            Bitmap image = new Bitmap(data.Columns, data.Rows);

            double min_value = data[0, 0], max_value = data[0, 0];
            for (int y = 0; y < data.Rows; y++)
            {
                for (int x = 0; x < data.Columns; x++)
                {
                    double value = data[y, x];
                    min_value = Math.Min(min_value, value);
                    max_value = Math.Max(max_value, value);
                }
            }

            int r, g, b;
            double dr, dg, db;

            for (int y = 0; y < data.Rows; y++)
            {
                for (int x = 0; x < data.Columns; x++)
                {
                    double value = (data[y, x] - min_value) / (max_value - min_value);
                    value = Math.Pow(value, power);
                    double color_value = numColors * value;
                    int color_index = (int)color_value;
                    double color_alpha = color_value - color_index;
                    if (color_index < 0)
                    {
                        color_index = 0;
                        color_alpha = 0;
                    }
                    else if (color_index > numColors)
                    {
                        color_index = numColors;
                        color_alpha = 0;
                    }

                    dr = colors[color_index, 0] + (colors[color_index + 1, 0] - colors[color_index, 0]) * color_alpha;
                    dg = colors[color_index, 1] + (colors[color_index + 1, 1] - colors[color_index, 1]) * color_alpha;
                    db = colors[color_index, 2] + (colors[color_index + 1, 2] - colors[color_index, 2]) * color_alpha;
                    
                    r = (int)dr;
                    g = (int)dg;
                    b = (int)db;
                    //int color = 255 * vlue + 0.5;
                    image.SetPixel(data.Columns - x - 1, y, Color.FromArgb(r, g, b));
                }
            }

            image.Save(fileName, ImageFormat.Png);
        }
    }
}
