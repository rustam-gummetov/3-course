using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyWork
{
    public partial class Form1 : Form
    {
        public int width, height;
        Matrix ViewPortMatrix, MT;
        double angle = 0;
        const double dif = 2.5;
        double delta = 0;
        Color color;
        private int r1;
        private int r2;
        private int g1;
        private int g2;
        private int b1;
        private int b2;

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                delta = dif;
                Invalidate();
            }
            else if (e.KeyCode == Keys.Right)
            {
                delta = -dif;
                Invalidate();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (angle < 0)
            {
                angle = 360 - Math.Abs(angle) % 360;
            }
            else
            {
                angle %= 360;
            }
            angle += delta;
            

            Bitmap bitmap = new Bitmap(width, height);
            Graphics g;
            g = Graphics.FromImage(bitmap);

            g.Clear(Color.LightGray);

            Matrix TransRotMatrix = new Matrix();
            TransRotMatrix *= Matrix.RotateY(angle) * Matrix.RotateY(180);

            TFModel(MT * TransRotMatrix, ref displaypoints);

            double max = int.MinValue, min = int.MaxValue;
            foreach (var pol in polygons)
            {
                if (pol.zAverage > max)
                    max = pol.zAverage;
                if (pol.zAverage < min)
                    min = pol.zAverage;
            }

            var diff = max - min;

            foreach (var pol in polygons)
            {
                var colint = (int)(((pol.zAverage - min) / (diff)) * (255) + 0);
                var rc = (int)(((pol.zAverage - min) / (diff)) * (255 - r1) + r2);
                var gc = (int)(((pol.zAverage - min) / (diff)) * (255 - g1) + g2);
                var bc = (int)(((pol.zAverage - min) / (diff)) * (255 - b1) + b2);
                g.FillPolygon(new SolidBrush(Color.FromArgb(rc,gc,bc)), pol.ToXYarray(ref displaypoints));
                g.DrawPolygon(new Pen(Color.DarkBlue, 0.01f), pol.ToXYarray(ref displaypoints));
            }

            bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
            pictureBox1.Image = bitmap;
            pictureBox1.Refresh();
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            height = pictureBox1.Height;
            width = pictureBox1.Width;

            Random r = new Random();

            color = Color.FromArgb(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255));
            r1 = color.R;
            r2 = r.Next(0, r1);
            g1 = color.G;
            g2 = r.Next(0, g1);
            b1 = color.B;
            b2 = r.Next(0, b1);

            PictureRead();

            ViewPortMatrix = Matrix.Viewport(0, 0, width, height);
            var coefficient = 2.2;
            Matrix MP = Matrix.ProjectionOrthographic(points.Min(v => v.x) * coefficient * 2.2, points.Max(v => v.x) * coefficient * 2.2, 
                                        points.Min(v => v.y) * coefficient - 1, points.Max(v => v.y) * coefficient, 
                                        points.Min(v => v.z) * coefficient, points.Max(v => v.z) * coefficient);
            MT = ViewPortMatrix * MP;
            var vectorEye = new Vector() { x = 0, y = 0, z = 1 };
            var vectorCenter = new Vector() { x = 0, y = 0, z = 0 };
            var vectorUp = new Vector() { x = 0, y = 1, z = 0 };
            var ModelView = Matrix.LookAt(vectorEye, vectorCenter, vectorUp);
            MT = MT * ModelView;

            timer1.Start();
        }
    }
}
