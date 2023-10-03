using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Map_Generator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Define the width and height of the image
            int width = 100;
            int height = 100;

            // Create a new Bitmap object with the specified width and height
            Bitmap image = new Bitmap(width, height);
            Color[] data = new Color[width * height];

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = Color.FromArgb(255, 255, 0, 0); // Example: Red color for each pixel
            }

            // Loop through each pixel in the image and set its color based on the data array
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // Get the index of the current pixel in the data array
                    int dataIndex = y * width + x;

                    // Get the RGB values from the data array
                    int red = data[dataIndex].R;
                    int green = data[dataIndex].G;
                    int blue = data[dataIndex].B;

                    // Create a Color object with the RGB values
                    Color pixelColor = Color.FromArgb(red, green, blue);

                    // Set the pixel color in the image
                    image.SetPixel(x, y, pixelColor);
                }
            }

            // Save the image to a file
            image.Save("image.jpg");
        }
    }
}
