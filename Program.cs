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
            Random rnd = new Random();

            // Define the width and height of the image
            int size = 100;
            int colorDiffMax = 255, colorDiffNormal = 51, whiteDiffMax = 100, whiteDiffNormal = 20;

            // Create a new Bitmap object with the specified width and height
            Bitmap image = new Bitmap(size, size);
            Color[,] data = new Color[size, size];

            // Filling up the array
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    int blue = rnd.Next(105, 255);
                    int white = (255 - blue) / 2;
                    bool isTop = i < 1, isLeft = j < 1;
                    // All except Left & Top borders
                    if (!isLeft && !isTop)
                    {
                        int r1a = data[i, j - 1].R;
                        int r1b = data[i - 1, j].R;
                        int r1c = r1a + r1b;
                        int r2 = r1c / 2;
                        int r3 = white - r2;
                        int r4 = r3 / 5;
                        int r5 = r2 + r4;
                        int g1a = data[i, j - 1].G;
                        int g1b = data[i - 1, j].G;
                        int g1c = g1a + g1b;
                        int g2 = g1c / 2;
                        int g3 = white - g2;
                        int g4 = g3 / 5;
                        int g5 = g2 + g4;
                        int b1a = data[i, j - 1].B;
                        int b1b = data[i - 1, j].B;
                        int b1c = b1a + b1b;
                        int b2 = b1c / 2;
                        int b3 = blue - b2;
                        int b4 = b3 / 5;
                        int b5 = b2 + b4;
                        //Console.Write(r1a + " ");
                        //Console.Write(r1b + " ");
                        //Console.Write(r1c + " ");
                        //Console.Write(r2 + " ");
                        //Console.Write(white + " ");
                        //Console.Write(r3 + " ");
                        //Console.Write(r4 + " ");
                        //Console.WriteLine(r5 + " ");
                        data[i, j] = Color.FromArgb(0,
                            r5,
                            g5,
                            b5);
                    }
                    // No Lefts
                    else if (!isLeft)
                        data[i, j] = Color.FromArgb(0,
                            data[i, j - 1].R + (white - data[i, j - 1].R) / 5,
                            data[i, j - 1].G + (white - data[i, j - 1].G) / 5,
                            data[i, j - 1].B + (blue - data[i, j - 1].B) / 5);
                    // No Tops
                    else if (!isTop)
                        data[i, j] = Color.FromArgb(0,
                            data[i - 1, j].R + (white - data[i - 1, j].R) / 5,
                            data[i - 1, j].G + (white - data[i - 1, j].G) / 5,
                            data[i - 1, j].B + (blue - data[i - 1, j].B) / 5);
                    // The only pixel - Top Left
                    else data[i, j] = Color.FromArgb(0, white, white, rnd.Next(white + 50, 255));
                }
            }
            
            // Loop through each pixel in the image and set its color based on the data array
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    // Get the RGB values from the data array
                    int red = data[y, x].R;
                    int green = data[y, x].G;
                    int blue = data[y, x].B;

                    // Create a Color object with the RGB values
                    Color pixelColor = Color.FromArgb(red, green, blue);

                    // Set the pixel color in the image
                    image.SetPixel(x, y, pixelColor);
                }
            }

            // Save the image to a file
            image.Save("image.png");

            //Console.ReadKey();
        }
    }
}
