using System;
using System.Drawing;
using System.Numerics;

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

            //data = Noise(size, range, data);
            data = Perlin(size, range, data);

            CreateImage(size, data);

            Console.ReadKey();
        }

        //AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA








        // Function to linearly interpolate between a0 and a1
        // Weight w should be in the range [0.0, 1.0]

        static float interpolate(float a0, float a1, float w)
        {
            /* // You may want clamping by inserting:
             * if (0.0 > w) return a0;
             * if (1.0 < w) return a1;
             */
            //return (a1 - a0) * w + a0;
            // Use this cubic interpolation [[Smoothstep]] instead, for a smooth appearance:
            return (float) ((a1 - a0) * (3.0 - w * 2.0) * w * w + a0);
          
            
            // Use [[Smootherstep]] for an even smoother result with a second derivative equal to zero on boundaries:
            //return (a1 - a0) * ((w * (w * 6.0 - 15.0) + 10.0) * w * w * w) + a0;
        }

        // Create pseudorandom direction vector
        static Vector2 randomGradient(int ix, int iy)
        {
            // No precomputed gradients mean this works for any number of grid coordinates
            const uint w = 8 * sizeof(uint);
            const uint s = w / 2; // rotation width
            uint a = (uint) ix, b = (uint) iy;
            a *= 3284157443; b ^= (uint)((int)a << (int)s | (int)a >> (int)(w - s));
            b *= 1911520717; a ^= (uint) ((int)b << (int)s | (int) b >> (int) (w - s));
            a *= 2048419325;
            float random = (float) (a * (3.14159265 / ~(~0u >> 1))); // in [0, 2*Pi]
            Vector2 v;
            v.X = (float)Math.Cos(random); v.Y = (float)Math.Sin(random);
            return v;
        }

        // Computes the dot product of the distance and gradient vectors.
        static float dotGridGradient(int ix, int iy, float x, float y)
        {
            // Get gradient from integer coordinates
            Vector2 gradient = randomGradient(ix, iy);

            // Vector from corner to candidate point
            Vector2 distance = new Vector2(x - ix, y - iy);

            return Vector2.Dot(distance, gradient);
        }

        // Compute Perlin noise at coordinates x, y
        static float perlin(float x, float y)
        {
            // Determine grid cell coordinates
            int x0 = (int)Math.Floor(x);
            int x1 = x0 + 1;
            int y0 = (int)Math.Floor(y);
            int y1 = y0 + 1;

            // Determine interpolation weights
            // Could also use higher order polynomial/s-curve here
            float sx = x - (float)x0;
            float sy = y - (float)y0;

            // Interpolate between grid point gradients
            float n0, n1, ix0, ix1, value;

            // Compute and interpolate top two corners
            n0 = dotGridGradient(x0, y0, x, y);
            n1 = dotGridGradient(x1, y0, x, y);
            ix0 = interpolate(n0, n1, sx);

            // Compute and interpolate bottom two corners
            n0 = dotGridGradient(x0, y1, x, y);
            n1 = dotGridGradient(x1, y1, x, y);
            ix1 = interpolate(n0, n1, sx);

            // Interpolate between the two previously interpolated values
            value = interpolate(ix0, ix1, sy);
            return value; // value will return in range -1 to 1. To make it in range 0 to 1, multiply by 0.5 and add 0.5
        }







        //AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA

        static Color[,] Perlin(int size, int range, Color[,] data)
        {
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    float val = 0;

                    float freq = 1;
                    float amp = 1;
                    val += perlin(x * freq / 100, y * freq / 100) * amp;
                    for (int i = 0; i < 12; i++)
                    {
                        val += perlin(x * freq / 100, y * freq / 100) * amp;

                        freq *= 2;
                        amp /= 2;

                    }

                    // Clipping
                    if (val > 1.0f)
                        val = 1.0f;
                    else if (val < -1.0f)
                        val = -1.0f;

                    // Convert 1 to -1 into 255 to 0
                    int color = (int)(((val + 1.0f) * 0.5f) * 255);

                    data[x, y] = Color.FromArgb(color, color, color);
                }
            }
            return data;
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
