using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Interpolation
{
    public struct vec2d
    {
        public int x;
        public int y;

        public vec2d(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public struct triangle
    {
        public vec2d[] vec2Ds;

        public triangle(int x1, int y1, int x2, int y2, int x3, int y3)
        {
            vec2Ds = new vec2d[3] {new vec2d(x1,y1), new vec2d(x2, y2), new vec2d(x3, y3) };                                                                                                                                                                                                 
        }                                                                                                                              
    }

    public partial class Form1 : Form
    {
        static int widthGouraud = 640, heightGouraud = 540;
        static int widthBar = 640, heightBar = 540;
        UInt32 RED = 0x00FF0000;
        UInt32 GREEN = 0x0000FF00;
        UInt32 BLUE = 0x000000FF;
        int step = 20;

        triangle[] figure = new triangle[]
        {
           new triangle(13, 8, 7, 4, 13, 2),
           new triangle(13, 8, 19, 4, 13, 2),
           new triangle(23, 10, 13, 8, 19, 4),
           new triangle(17, 12, 23, 10, 13, 8),
           new triangle(23, 16, 17, 12, 23, 10),
           new triangle(15, 16, 23, 16, 17, 12),
           new triangle(19, 20, 15, 16, 23, 16),
           new triangle(13, 22, 19, 20, 15, 16),
           new triangle(13, 22, 11, 16, 15, 16),
           new triangle(13, 22, 7, 20, 11, 16),
           new triangle(7, 20, 3, 16, 11, 16),
           new triangle(3, 16, 11, 16, 9, 12),
           new triangle(3, 16, 9, 12, 3, 10),
           new triangle(9, 12, 3, 10, 13, 8),
           new triangle(3, 10, 13, 8, 7, 4),
           new triangle(9, 12, 17, 12, 13, 8),
           new triangle(11, 16, 9, 12, 17, 12),
           new triangle(11, 16, 15, 16, 17, 12)
        };

        public Form1()
        {
            InitializeComponent();
        }

        public void PutPixel(int x, int y, bool[,] pixels)
        {
            pixels[x, y] = true;
        }

        public void PutPixel(int x, int y, bool[,] pixels, UInt32 color, UInt32[,] pixelsColor)
        {
            pixels[x, y] = true;
            pixelsColor[x, y] = color;
        }

        void Brezenham(int xn, int yn, int xk, int yk, bool[,] pixels)
        {
            int dx, dy;
            int s, sx, sy, kl, incr1, incr2;
            bool swap;

            sx = 0;
            if ((dx = xk - xn) < 0) { dx = -dx; --sx; } else if (dx > 0) ++sx;
            sy = 0;
            if ((dy = yk - yn) < 0) { dy = -dy; --sy; } else if (dy > 0) ++sy;


            swap = false;
            kl = dx; s = dy;
            if (kl < s)
            {
                dx = s; dy = kl; kl = s; swap = true;
            }

            s = (incr1 = 2 * dy) - dx;
            incr2 = 2 * dx;

            PutPixel(xn, yn, pixels);
            while (--kl >= 0)
            {
                if (s >= 0)
                {
                    if (swap) xn += sx; else yn += sy;
                    s -= incr2;
                }
                if (swap) yn += sy; else xn += sx;
                s += incr1;
                PutPixel(xn, yn, pixels);
            }
        }

        public void DrawGrid(int width, int height, bool[,] pixels, Graphics graphics, UInt32[,] pixelsColor)
        {
            for (int y = 0; y < height / step; y++)
            {
                for (int x = 0; x < width / step; x++)
                {
                    graphics.DrawRectangle(Pens.Black, x * step, height - 1 - y * step - step, step, step);
                    if (pixels[x, y])
                    {
                        //graphics.FillRectangle(Brushes.Green, x * step + 2, height - 1 - y * step - step + 2, step - 3, step - 3);
                        
                        Brush brush = new SolidBrush(Color.FromArgb((int)pixelsColor[x, y]));
                                                   
                        graphics.FillRectangle(brush, x * step + 2, height - 1 - y * step - step + 2, step - 3, step - 3);
                    }
                }
            }
        }

        static void Swap<T>(ref T lhs, ref T rhs)
        {
            T temp;
            temp = lhs;
            lhs = rhs;
            rhs = temp;
        }

        public void Gourand(triangle triangle, bool[,] pixels, UInt32[,] pixelsColor)
        {
            vec2d t0 = triangle.vec2Ds[2];
            vec2d t1 = triangle.vec2Ds[1];
            vec2d t2 = triangle.vec2Ds[0];
            Point A = new Point();
            Point B = new Point();

            int totalHeight = t2.y - t0.y;

            if (t1.y - t0.y != 0)
            {
                if (t1.x > t2.x || t0.x < t2.x && t1.x == t2.x)
                {
                    for (int y = t0.y + 1; y <= t1.y; y++)
                    {
                        A.Y = y;
                        B.Y = y;
                        int segmentHeight = t1.y - t0.y;

                        float alpha = (float)(y - t0.y) / totalHeight;
                        float beta = (float)(y - t0.y) / segmentHeight;

                        A.X = (int)(t0.x + (t2.x - t0.x) * alpha);
                        B.X = (int)(t0.x + (t1.x - t0.x) * beta);
                        if (A.X > B.X) Swap(ref A, ref B);

                        UInt32 colorLeft = getColorGourand(pixelsColor[t0.x, t0.y], pixelsColor[t2.x, t2.y], y, t0.y, t2.y);
                        UInt32 colorRight = getColorGourand(pixelsColor[t0.x, t0.y], pixelsColor[t1.x, t1.y], y, t0.y, t1.y);

                        for (int j = A.X; j <= B.X; j++)
                        {
                            UInt32 col = getColorGourand(colorRight, colorLeft, j, B.X, A.X);
                            PutPixel(j, y, pixels, col, pixelsColor);
                        }
                    }
                }
                else
                {
                    for (int y = t0.y + 1; y <= t1.y; y++)
                    {
                        A.Y = y;
                        B.Y = y;
                        int segmentHeight = t1.y - t0.y;
                        float alpha = (float)(y - t0.y) / segmentHeight;
                        float beta = (float)(y - t0.y) / totalHeight; 

                        A.X = (int)(t0.x + (t1.x - t0.x) * alpha);
                        B.X = (int)(t0.x + (t2.x - t0.x) * beta);
                        if (A.X > B.X) Swap(ref A, ref B);

                        UInt32 colorLeft = getColorGourand(pixelsColor[t0.x, t0.y], pixelsColor[t1.x, t1.y], y, t0.y, t1.y);
                        UInt32 colorRight = getColorGourand(pixelsColor[t0.x, t0.y], pixelsColor[t2.x, t2.y], y, t0.y, t2.y);

                        for (int j = A.X; j <= B.X; j++)
                        {
                            UInt32 col = getColorGourand(colorRight, colorLeft, j, B.X, A.X);
                            PutPixel(j, y, pixels, col, pixelsColor);
                        }
                    }
                }
            }
            if (t2.y - t1.y != 0)
            {
                int start;
                if (t1.y == t0.y)
                    start = t1.y;
                else
                    start = t1.y + 1;

                if (t1.x > t2.x || t0.x < t2.x && t1.x == t2.x)
                {
                    for (int y = start; y < t2.y; y++)
                    {
                        A.Y = y;
                        B.Y = y;
                        int segmentHeight = t2.y - t1.y;

                        float alpha = (float)(y - t0.y) / totalHeight;
                        float beta = (float)(y - t1.y) / segmentHeight;

                        A.X = (int)(t0.x + (t2.x - t0.x) * alpha);
                        B.X = (int)(t1.x + (t2.x - t1.x) * beta);
                        if (A.X > B.X) Swap(ref A, ref B);

                        UInt32 colorLeft = getColorGourand(pixelsColor[t0.x, t0.y], pixelsColor[t2.x, t2.y], y, t0.y, t2.y);
                        UInt32 colorRight = getColorGourand(pixelsColor[t1.x, t1.y], pixelsColor[t2.x, t2.y], y, t1.y, t2.y);

                        for (int j = A.X; j <= B.X; j++)
                        {
                            UInt32 col = getColorGourand(colorRight, colorLeft, j, B.X, A.X);
                            PutPixel(j, y, pixels, col, pixelsColor);
                        }
                    }
                }
                else
                {
                    for (int y = start; y < t2.y; y++)
                    {
                        A.Y = y;
                        B.Y = y;
                        int segmentHeight = t2.y - t1.y;

                        float alpha = (float)(y - t1.y) / segmentHeight;
                        float beta = (float)(y - t0.y) / totalHeight; 

                        A.X = (int)(t1.x + (t2.x - t1.x) * alpha);
                        B.X = (int)(t0.x + (t2.x - t0.x) * beta);
                        if (A.X > B.X) Swap(ref A, ref B);

                        UInt32 colorLeft = getColorGourand(pixelsColor[t1.x, t1.y], pixelsColor[t2.x, t2.y], y, t1.y, t2.y);
                        UInt32 colorRight = getColorGourand(pixelsColor[t0.x, t0.y], pixelsColor[t2.x, t2.y], y, t0.y, t2.y);

                        for (int j = A.X; j <= B.X; j++)
                        {
                            UInt32 col = getColorGourand(colorRight, colorLeft, j, B.X, A.X);
                            PutPixel(j, y, pixels, col, pixelsColor);
                        }
                    }
                }
            }
        }

        private UInt32 getColorGourand(UInt32 colorLeft, UInt32 colorRight, int x, int a, int b)
        {
            double d = (double)(x - b) / (double)(a - b);
            double d2 = (double)(a - x) / (double)(a - b);

            UInt32 redInterpolation = RGBInterpolation(d, d2, colorLeft, colorRight, 16, RED);
            UInt32 greenInterpolation = RGBInterpolation(d, d2, colorLeft, colorRight, 8, GREEN);
            UInt32 blueInterpolation = RGBInterpolation(d, d2, colorLeft, colorRight, 0, BLUE);

            return 0xFF000000 | redInterpolation | greenInterpolation | blueInterpolation;
        }

        private UInt32 RGBInterpolation(double firstParametr, double secondParametr, UInt32 colorLeft, UInt32 colorRight, int numberOfBits, UInt32 rgbPart)
        {
            return (UInt32)(firstParametr * ((colorLeft & rgbPart) >> numberOfBits) + secondParametr * ((colorRight & rgbPart) >> numberOfBits)) << numberOfBits;
        }

        public UInt32 getColorBarycentricCoordinates(int x1, int y1, int x2, int y2, int x3, int y3, int x, int y, UInt32 color1, UInt32 color2, UInt32 color3)
        {
            UInt32 pixelValue = 0xFFFFFFFF;
            
            double l1, l2, l3;
            l1 = ((y2 - y3) * ((double)(x) - x3) + (x3 - x2) * ((double)(y) - y3)) /
                ((y2 - y3) * (x1 - x3) + (x3 - x2) * (y1 - y3));
            l2 = ((y3 - y1) * ((double)(x) - x3) + (x1 - x3) * ((double)(y) - y3)) /
                ((y2 - y3) * (x1 - x3) + (x3 - x2) * (y1 - y3));
            l3 = 1 - l1 - l2;

            if (l1 >= 0 && l1 <= 1 && l2 >= 0 && l2 <= 1 && l3 >= 0 && l3 <= 1)
            {
                UInt32 redInterpolation = (UInt32)(l1 * ((color1 & RED) >> 16) + l2 * ((color2 & RED) >> 16) + l3 * ((color3 & RED) >> 16)) << 16;
                UInt32 greenInterpolation = (UInt32)(l1 * ((color1 & GREEN) >> 8) + l2 * ((color2 & GREEN) >> 8) + l3 * ((color3 & GREEN) >> 8)) << 8;
                UInt32 blueInterpolation = (UInt32)(l1 * (color1 & BLUE) + l2 * (color2 & BLUE) + l3 * (color3 & BLUE));

                pixelValue = 
                    (UInt32)0xFF000000 |
                    redInterpolation |
                    greenInterpolation |
                    blueInterpolation;
            }
            return pixelValue;
        }

        public void BarycentricCoordinates(triangle triangle, bool[,] pixels, UInt32[,] pixelsColor)
        {
            vec2d v0 = triangle.vec2Ds[0];
            vec2d v1 = triangle.vec2Ds[1];
            vec2d v2 = triangle.vec2Ds[2];

            UInt32 colorV0 = pixelsColor[v0.x, v0.y];
            UInt32 colorV1 = pixelsColor[v1.x, v1.y];
            UInt32 colorV2 = pixelsColor[v2.x, v2.y];

            int xStart = Math.Min(Math.Min(v0.x, v1.x), v2.x);
            int xEnd = Math.Max(Math.Max(v0.x, v1.x), v2.x);

            for (int x = xStart; x <= xEnd; x++)
            {
                for (int y = v2.y; y <= v0.y; y++)
                {
                    UInt32 pointColor = getColorBarycentricCoordinates(v0.x, v0.y, v1.x, v1.y, v2.x, v2.y, x, y, colorV0, colorV1, colorV2);
                    if (pointColor != 0xFFFFFFFF)
                        PutPixel(x, y, pixels, pointColor, pixelsColor);
                }
            }
        }

        public void getFigureGourand(bool[,] pixels, UInt32[,] pixelsColor)
        {
            var rand = new Random();

            foreach (var triangle in figure)
            {
                Brezenham(triangle.vec2Ds[0].x, triangle.vec2Ds[0].y, triangle.vec2Ds[1].x, triangle.vec2Ds[1].y, pixels);
                Brezenham(triangle.vec2Ds[1].x, triangle.vec2Ds[1].y, triangle.vec2Ds[2].x, triangle.vec2Ds[2].y, pixels);
                Brezenham(triangle.vec2Ds[2].x, triangle.vec2Ds[2].y, triangle.vec2Ds[0].x, triangle.vec2Ds[0].y, pixels);

                pixelsColor[triangle.vec2Ds[0].x, triangle.vec2Ds[0].y] = (UInt32)rand.Next(0, 0xFFFFFF);
                pixelsColor[triangle.vec2Ds[1].x, triangle.vec2Ds[1].y] = (UInt32)rand.Next(0, 0xFFFFFF);
                pixelsColor[triangle.vec2Ds[2].x, triangle.vec2Ds[2].y] = (UInt32)rand.Next(0, 0xFFFFFF);

                pixelsColor[triangle.vec2Ds[0].x, triangle.vec2Ds[0].y] = 0xFF000000 | pixelsColor[triangle.vec2Ds[0].x, triangle.vec2Ds[0].y];
                pixelsColor[triangle.vec2Ds[1].x, triangle.vec2Ds[1].y] = 0xFF000000 | pixelsColor[triangle.vec2Ds[1].x, triangle.vec2Ds[1].y];
                pixelsColor[triangle.vec2Ds[2].x, triangle.vec2Ds[2].y] = 0xFF000000 | pixelsColor[triangle.vec2Ds[2].x, triangle.vec2Ds[2].y];

                Gourand(triangle, pixels, pixelsColor);
            }
        }

        public void getFigureBarycentric(bool[,] pixels, UInt32[,] pixelsColor)
        {
            var rand = new Random();
            foreach (var triangle in figure)
            {
                Brezenham(triangle.vec2Ds[0].x, triangle.vec2Ds[0].y, triangle.vec2Ds[1].x, triangle.vec2Ds[1].y, pixels);
                Brezenham(triangle.vec2Ds[1].x, triangle.vec2Ds[1].y, triangle.vec2Ds[2].x, triangle.vec2Ds[2].y, pixels);
                Brezenham(triangle.vec2Ds[2].x, triangle.vec2Ds[2].y, triangle.vec2Ds[0].x, triangle.vec2Ds[0].y, pixels);

                pixelsColor[triangle.vec2Ds[0].x, triangle.vec2Ds[0].y] = (UInt32)rand.Next(0, 0xFFFFFF);
                pixelsColor[triangle.vec2Ds[1].x, triangle.vec2Ds[1].y] = (UInt32)rand.Next(0, 0xFFFFFF);
                pixelsColor[triangle.vec2Ds[2].x, triangle.vec2Ds[2].y] = (UInt32)rand.Next(0, 0xFFFFFF);

                BarycentricCoordinates(triangle, pixels, pixelsColor);
            }
        }

        private void Fill(UInt32[,] pixelsColor)
        {
            for (int i = 0; i < pixelsColor.GetLength(0); i++)
            {
                for (int j = 0; j < pixelsColor.GetLength(1); j++)
                {
                    pixelsColor[i, j] = 0xFFFFFF;
                }
            }
        }

        private Bitmap bilinealInterpolation(Bitmap initialBMP, int scaleX, int scaleY)
        {
            Bitmap scaledBMP = new Bitmap(initialBMP.Width * scaleX, initialBMP.Height * scaleY);
            UInt32[,] initialColors = new UInt32[initialBMP.Width, initialBMP.Height];
            UInt32[,] scaledColors = new UInt32[initialBMP.Width * scaleX, initialBMP.Height * scaleY];

            for (int x = 0; x < initialBMP.Width; x++)
            {
                for (int y = 0; y < initialBMP.Height; y++)
                {
                    initialColors[x, y] = (UInt32)initialBMP.GetPixel(x, y).ToArgb();
                    scaledColors[scaleX * x, scaleY * y] = initialColors[x, y];
                }
            }

            for (int y = 0; y < scaledBMP.Height; y += scaleY)
            {
                for (int initX = 0; initX < scaledBMP.Width - scaleX; initX += scaleX)
                {
                    int start = initX;
                    int end = initX + scaleX;

                    for (int x = start + 1; x < end; x++)
                    {
                        //var left = (end - x) / scaleX;
                        var right = (x - start) / scaleX;
                        var left = (1 - right);
                        UInt32 redInterpolation = (UInt32)(left * ((scaledColors[start, y] & RED) >> 16) + right * ((scaledColors[end, y] & RED) >> 16)) << 16;
                        UInt32 greenInterpolation = (UInt32)(left * ((scaledColors[start, y] & GREEN) >> 8) + right * ((scaledColors[end, y] & GREEN) >> 8)) << 8;
                        UInt32 blueInterpolation = (UInt32)(left * (scaledColors[start, y] & BLUE) + right * (scaledColors[end, y] & BLUE));



                        scaledColors[x, y] = 0xFF000000 | redInterpolation | greenInterpolation | blueInterpolation;
                    }
                }
            }

            for (int x = 0; x < scaledBMP.Width; x++)
            {
                for (int initY = 0; initY < scaledBMP.Height - scaleY; initY += scaleY)
                {
                    int start = initY;
                    int end = initY + scaleY;

                    for (int y = start + 1; y < end; y++)
                    {
                        //var left = (end - y) / scaleY;
                        var right = (y - start) / scaleY;
                        var left = 1 - right;
                        UInt32 redInterpolation = (UInt32)(left * ((scaledColors[x, start] & RED) >> 16) + right * ((scaledColors[x, end] & RED) >> 16)) << 16;
                        UInt32 greenInterpolation = (UInt32)(left * ((scaledColors[x, start] & GREEN) >> 8) + right * ((scaledColors[x, end] & GREEN) >> 8)) << 8;
                        UInt32 blueInterpolation = (UInt32)(left * (scaledColors[x, start] & BLUE) + right * (scaledColors[x, end] & BLUE));

                        var result = 0xFF000000 | redInterpolation | greenInterpolation | blueInterpolation;
                        if (result != 0xFF000000 && result != 0xFFFFFFFF)
                            scaledColors[x, y] = result;
                    }
                }
            }

            for (int x = 0; x < scaledBMP.Width; x++)
            {
                for (int y = 0; y < scaledBMP.Height; y++)
                {
                    scaledBMP.SetPixel(x, y, Color.FromArgb((int)scaledColors[x, y]));
                }
            }


            return scaledBMP;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            bool[,] pixelsGourand = new bool[(widthGouraud / step), (heightGouraud / step)];
            Bitmap bitmapGourand = new Bitmap(widthGouraud, heightGouraud);
            Graphics graphicsGourand = Graphics.FromImage(bitmapGourand);

            UInt32[,] pixelsColorGourand = new UInt32[(widthGouraud / step), (heightGouraud / step)];
            Fill(pixelsColorGourand);
            bool[,] pixelsBar = new bool[(widthBar / step), (heightBar / step)];
            Bitmap bitmapBar = new Bitmap(widthBar, heightBar);
            Graphics graphicsBar = Graphics.FromImage(bitmapBar);

            UInt32[,] pixelsColorBar = new UInt32[(widthBar / step), (heightBar / step)];

            Bitmap bitmapForInitialImage = new Bitmap("sample_resized.bmp");
            pbInitialImage.Image = bitmapForInitialImage;

            Bitmap bmpScaledImage = bilinealInterpolation(bitmapForInitialImage, 3, 3);
            pbScaledImage.Image = bmpScaledImage;

            getFigureGourand(pixelsGourand, pixelsColorGourand);
            getFigureBarycentric(pixelsBar, pixelsColorBar);
            DrawGrid(widthGouraud, heightGouraud, pixelsGourand, graphicsGourand, pixelsColorGourand);
            DrawGrid(widthBar, heightBar, pixelsBar, graphicsBar, pixelsColorBar);

            pbGouraud.Image = bitmapGourand;
            pbBarycentric.Image = bitmapBar;
        }
    }
}
