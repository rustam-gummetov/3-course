using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyWork
{
    public partial class Form1 : Form
    {
        List<Vector> points = new List<Vector>();

        List<Polygon> polygons = new List<Polygon>();
        List<Vector> displaypoints = new List<Vector>();

        public Polygon zAverageUpdate(Polygon polygon)
        {
            double sum = 0.0;
            for (int i = 0; i < polygon.vertex.Count; i++)
            {
                sum += displaypoints[polygon.vertex[i]].z;
            }
            polygon.zAverage = sum / polygon.vertex.Count;
            return polygon;
        }
        
        public struct Polygon
        {
            public List<int> vertex;
            public double zAverage;
       
            public PointF[] ToXYarray(ref List<Vector> pointslist)
            {
                var list = new List<PointF>();
                foreach (var point in vertex)
                {
                    list.Add(pointslist[point].ToXY());
                }
                return list.ToArray();
            }
        }
        public void PolsSort()
        {
            polygons.Sort(delegate (Polygon a, Polygon b) {
                return a.zAverage.CompareTo(b.zAverage);
            });
        }

        public void PictureRead()
        {
            string pathToPicture = "guard post.obj";
            StreamReader fileobj = new StreamReader(pathToPicture, Encoding.UTF8);

            points = new List<Vector>();
            string str;
            while (!fileobj.EndOfStream)
            {
                str = fileobj.ReadLine();
                if (str.StartsWith("v") && !str.StartsWith("vn") && !str.StartsWith("vt"))
                {
                    str = str.TrimEnd();
                    var pointsl = str.Split(' ').Skip(1).Select(v => Double.Parse(v.Replace('.', ','))).ToArray();
                    points.Add(new Vector(pointsl[0], pointsl[1], pointsl[2]));
                }
                else if (str.StartsWith("f"))
                {
                    str = str.TrimEnd();
                    var pols = str.Split(' ').Skip(1).Select(v => int.Parse(v.Split('/').First())).ToArray();
                    for (int i = 0; i < pols.Length; i++)
                    {
                        //if (pols[i] > 0)
                            pols[i] = pols[i] - 1;
                        //else
                            //pols[i] = points.Count + pols[i];
                    }
                    Polygon polygon = new Polygon();
                    polygon.vertex = pols.ToList();
                    if (polygon.vertex.Count != 0)
                        polygons.Add(polygon);
                }
            }
            List<Polygon> temp = new List<Polygon>();
            foreach (var polygon in polygons)
            {
                Polygon t = polygon;
                for (int i = 0; i < polygon.vertex.Count; i++)
                {
                    t.zAverage += points[polygon.vertex[i]].z;
                }
                t.zAverage /= polygon.vertex.Count;
                temp.Add(t);
            }
            polygons = temp;
            fileobj.Close();
        }
        public void TFModel(Matrix m, ref List<Vector> listpoints)
        {
            List<Vector> temp = new List<Vector>();
            foreach (var point in points)
            {
                temp.Add(point.MatrixMultVector(m));
            }
            listpoints = temp;

            List<Polygon> t = new List<Polygon>();
            foreach (var pol in polygons)
            {
                t.Add(zAverageUpdate(pol));
            }
            polygons = t;

            PolsSort();
        }
    }
}
