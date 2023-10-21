using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;

namespace Map_Generator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("map size (px): ");
            int size = int.Parse(Console.ReadLine());
            Console.WriteLine("map roughness (0 - 255): ");
            int range = int.Parse(Console.ReadLine());

            Color[,] data = new Color[size, size];

            data = Noise(size, range, data);

            CreateImage(size, data);

            Console.ReadKey();
        }

        static Color[,] Noise(int size, int range, Color[,] data) // Generating noise texture
        {
            Random rnd = new Random();

            Color[,] data2 = new Color[size, size];
            Color[,] data3 = new Color[size, size];

            // Filling up the array
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {

                    bool isTop = i < 1, isLeft = j < 1;
                    // All except Left & Top borders
                    if (!isLeft && !isTop)
                    {
                        if (data[i, j - 1].R < data[i - 1, j].R)
                        {
                            int rndMin = data[i, j - 1].R - range, rndMax = data[i - 1, j].R + range;
                            if (rndMin < 0) rndMin = 0;
                            if (rndMax > 255) rndMax = 255;
                            int white = rnd.Next(rndMin, rndMax);
                            data[i, j] = Color.FromArgb(0, white, white, white);
                        }
                        else
                        {
                            int rndMin = data[i - 1, j].R - range, rndMax = data[i, j - 1].R + range;
                            if (rndMin < 0) rndMin = 0;
                            if (rndMax > 255) rndMax = 255;
                            int white = rnd.Next(rndMin, rndMax);
                            data[i, j] = Color.FromArgb(0, white, white, white);
                        }
                    }
                    // No Lefts
                    else if (!isLeft)
                    {
                        int rndMin = data[i, j - 1].R - range, rndMax = data[i, j - 1].R + range;
                        if (rndMin < 0) rndMin = 0;
                        if (rndMax > 255) rndMax = 255;
                        int white = rnd.Next(rndMin, rndMax);
                        data[i, j] = Color.FromArgb(0, white, white, white);
                    }
                    // No Tops
                    else if (!isTop)
                    {
                        int rndMin = data[i - 1, j].R - range, rndMax = data[i - 1, j].R + range;
                        if (rndMin < 0) rndMin = 0;
                        if (rndMax > 255) rndMax = 255;
                        int white = rnd.Next(rndMin, rndMax);
                        data[i, j] = Color.FromArgb(0, white, white, white);
                    }
                    // The only pixel - Top Left
                    else data[i, j] = Color.FromArgb(0, rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));
                }
            }
            //Second array
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {

                    bool isTop = i < 1, isLeft = j < 1;
                    // All except Left & Top borders
                    if (!isLeft && !isTop)
                    {
                        if (data2[i, j - 1].R < data2[i - 1, j].R)
                        {
                            int rndMin = data2[i, j - 1].R - range, rndMax = data2[i - 1, j].R + range;
                            if (rndMin < 0) rndMin = 0;
                            if (rndMax > 255) rndMax = 255;
                            int white = rnd.Next(rndMin, rndMax);
                            data2[i, j] = Color.FromArgb(0, white, white, white);
                        }
                        else
                        {
                            int rndMin = data2[i - 1, j].R - range, rndMax = data2[i, j - 1].R + range;
                            if (rndMin < 0) rndMin = 0;
                            if (rndMax > 255) rndMax = 255;
                            int white = rnd.Next(rndMin, rndMax);
                            data2[i, j] = Color.FromArgb(0, white, white, white);
                        }
                    }
                    // No Lefts
                    else if (!isLeft)
                    {
                        int rndMin = data2[i, j - 1].R - range, rndMax = data2[i, j - 1].R + range;
                        if (rndMin < 0) rndMin = 0;
                        if (rndMax > 255) rndMax = 255;
                        int white = rnd.Next(rndMin, rndMax);
                        data2[i, j] = Color.FromArgb(0, white, white, white);
                    }
                    // No Tops
                    else if (!isTop)
                    {
                        int rndMin = data2[i - 1, j].R - range, rndMax = data2[i - 1, j].R + range;
                        if (rndMin < 0) rndMin = 0;
                        if (rndMax > 255) rndMax = 255;
                        int white = rnd.Next(rndMin, rndMax);
                        data2[i, j] = Color.FromArgb(0, white, white, white);
                    }
                    // The only pixel - Top Left
                    else data2[i, j] = Color.FromArgb(0, rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));
                }
            }

            //Rotating array
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    data3[y, x] = data2[x, size - y - 1];
                }
            }

            //Adding arrays
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    data[y, x] = Color.FromArgb((data[y, x].R + data3[y, x].R) / 2, (data[y, x].G + data3[y, x].G) / 2, (data[y, x].B + data3[y, x].B) / 2);
                }
            }

            return data;
        }

        static void CreateImage(int size, Color[,] data) // Loop through each pixel in the image and set its color based on the data array
        {
            Bitmap image = new Bitmap(size, size);

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    Color pixelColor = Color.FromArgb(data[y, x].R, data[y, x].G, data[y, x].B);

                    image.SetPixel(x, y, pixelColor);
                }
            }

            image.Save("image.png");

            Console.WriteLine("saved to image.png");
        }
    }
}
