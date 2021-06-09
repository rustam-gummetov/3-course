using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MyWork
{
    public class Vector
    {
        public double x, y, z, w;
        public PointF ToXY()
        {
            return new PointF((float)x, (float)y);
        }
        public Vector()
        {
            x = 0.0;
            y = 0.0;
            z = 0.0;
            w = 1.0;
        }
        public Vector(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            w = 1.0;
        }

        public Vector(double x, double y, double z, double w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public Vector MatrixMultVector(Matrix m) //Умножение матрицы на вектор
        {
            double[] vector = new double[4];
            vector[0] = x;
            vector[1] = y;
            vector[2] = z;
            vector[3] = w;
            double[] arr_res = new double[4];

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    arr_res[i] += m.uMatrix[i, j] * vector[j];
                }
            }

            Vector v_res = new Vector(arr_res[0], arr_res[1], arr_res[2], arr_res[3]);
            return v_res;
        }

        static public Vector operator ^(Vector v1, Vector v2)   //Умножение двух векторов
        {
            Vector v_res = new Vector(v1.y * v2.z - v1.z * v2.y, v1.z * v2.x - v1.x * v2.z, v1.x * v2.y - v1.y * v2.x);
            return v_res;
        }

        static public Vector operator -(Vector v1, Vector v2)   //Вычитание векторов
        {
            Vector v_res = new Vector(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
            return v_res;
        }

        public double LenVec()  //длина вектора
        {
            double lenVec = Math.Sqrt(x * x + y * y + z * z);
            return lenVec;
        }

        public void Normalize()  //нормализация вектора
        {
            x /= LenVec();
            y /= LenVec();
            z /= LenVec();
        }
    }

    public class Matrix
    {
        public double[,] uMatrix;
        public Matrix()
        {
            uMatrix = new double[4, 4];
            for (int i = 0; i < 4; i++)
                uMatrix[i, i] = 1.0;
        }

        static public Matrix operator *(Matrix m1, Matrix m2)   //Умножение матрицы на матрицу
        {
            double[,] arr = new double[4, 4];

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        arr[i, j] += m1.uMatrix[i, k] * m2.uMatrix[k, j];
                    }                        
                }
            }

            Matrix m_res = new Matrix();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    m_res.uMatrix[i, j] = arr[i, j];
                }                   
            }
               
            return m_res;
        }


        static public Matrix Viewport(int x, int y, int w, int h)  //Матрица окна вывода,  меняет мои координаты на координаты экрана
        {
            Matrix m_res = new Matrix();
            m_res.uMatrix[0, 3] = x + w / 2.0;
            m_res.uMatrix[1, 3] = y + h / 2.0;
            m_res.uMatrix[2, 3] = 255.0 / 2.0;

            m_res.uMatrix[0, 0] = w / 2.0;
            m_res.uMatrix[1, 1] = h / -2.0;
            m_res.uMatrix[2, 2] = 255.0 / 2.0;
            return m_res;
        }
        static public Matrix ProjectionOrthographic(double l, double r, double t, double b, double n, double f)
        {
            var matrix = new Matrix();
            matrix.uMatrix[0, 0] = 2.0 / (r - l);
            matrix.uMatrix[1, 1] = 2.0 / (t - b);
            matrix.uMatrix[2, 2] = -2.0 / (f - n);
            matrix.uMatrix[0, 3] = -(r + l) / (r - l);
            matrix.uMatrix[1, 3] = -(t + b) / (t - b);
            matrix.uMatrix[2, 3] = -(f + n) / (f - n);
            matrix.uMatrix[3, 3] = 1;
            return matrix;
        }
        public static Matrix LookAt(Vector eye, Vector center, Vector up)
        {
            var matrix = new Matrix();
            var Z = eye - center;
            Z.Normalize();
            var X = up ^ Z;
            X.Normalize();
            var Y = Z ^ X;
            Y.Normalize();
            matrix.uMatrix[0, 0] = X.x;
            matrix.uMatrix[0, 1] = X.y;
            matrix.uMatrix[0, 2] = X.z;
            matrix.uMatrix[0, 3] = -center.x;
            matrix.uMatrix[1, 0] = Y.x;
            matrix.uMatrix[1, 1] = Y.y;
            matrix.uMatrix[1, 2] = Y.z;
            matrix.uMatrix[1, 3] = -center.y;
            matrix.uMatrix[2, 0] = Z.x;
            matrix.uMatrix[2, 1] = Z.y;
            matrix.uMatrix[2, 2] = Z.z;
            matrix.uMatrix[2, 3] = -center.z;
            matrix.uMatrix[3, 3] = 1;
            return matrix;
        }

        static public Matrix Scale(double sx, double sy, double sz)
        {
            Matrix m_res = new Matrix();
            m_res.uMatrix[0, 0] = sx;
            m_res.uMatrix[1, 1] = sy;
            m_res.uMatrix[2, 2] = sz;

            return m_res;
        }

        static public Matrix Translated(double tx, double ty, double tz)
        {
            Matrix m_res = new Matrix();
            m_res.uMatrix[0, 3] = tx;
            m_res.uMatrix[1, 3] = ty;
            m_res.uMatrix[2, 3] = tz;

            return m_res;
        }

        static public Matrix Rotate(double x_alfa,double y_alfa,double z_alfa)
        {
            double rad = 0.0174533;
            double alfa = rad * x_alfa + rad * y_alfa + rad * z_alfa;
            Matrix m_res = new Matrix();
            if (z_alfa != 0)
            {
                m_res.uMatrix[0, 0] = Math.Cos(alfa);
                m_res.uMatrix[1, 0] = Math.Sin(alfa);

                m_res.uMatrix[0, 1] = -Math.Sin(alfa);
                m_res.uMatrix[1, 1] = Math.Cos(alfa);
            }
            if (x_alfa != 0)
            {
                m_res.uMatrix[1, 1] = Math.Cos(alfa);
                m_res.uMatrix[1, 2] = -Math.Sin(alfa);

                m_res.uMatrix[2, 1] = Math.Sin(alfa);
                m_res.uMatrix[2, 2] = Math.Cos(alfa);
            }
            if (y_alfa != 0)
            {
                m_res.uMatrix[0, 0] = Math.Cos(alfa);
                m_res.uMatrix[0, 2] = Math.Sin(alfa);

                m_res.uMatrix[2, 0] = -Math.Sin(alfa);
                m_res.uMatrix[2, 2] = Math.Cos(alfa);
            }
            return m_res;
        }

        static public Matrix RotateX(double x_alfa)
        {
            double alfa = x_alfa * Math.PI / 180.0;
            Matrix m_res = new Matrix();
            if (x_alfa != 0)
            {
                m_res.uMatrix[0, 0] = 1;

                m_res.uMatrix[1, 1] = Math.Cos(alfa);
                m_res.uMatrix[1, 2] = -Math.Sin(alfa);

                m_res.uMatrix[2, 1] = Math.Sin(alfa);
                m_res.uMatrix[2, 2] = Math.Cos(alfa);

                m_res.uMatrix[3, 3] = 1;
            }
            return m_res;
        }

        static public Matrix RotateY(double y_alfa)
        {
            double alfa = y_alfa * Math.PI / 180.0;
            Matrix m_res = new Matrix();
            if (y_alfa != 0)
            {
                m_res.uMatrix[0, 0] = Math.Cos(alfa);
                m_res.uMatrix[0, 2] = Math.Sin(alfa);

                m_res.uMatrix[1, 1] = 1;

                m_res.uMatrix[2, 0] = -Math.Sin(alfa);
                m_res.uMatrix[2, 2] = Math.Cos(alfa);
            }
            return m_res;
        }

        static public Matrix RotateZ(double z_alfa)
        {
            double alfa = z_alfa * Math.PI / 180.0;
            Matrix m_res = new Matrix();
            if (z_alfa != 0)
            {
                m_res.uMatrix[0, 0] = Math.Cos(alfa);
                m_res.uMatrix[1, 0] = Math.Sin(alfa);

                m_res.uMatrix[0, 1] = -Math.Sin(alfa);
                m_res.uMatrix[1, 1] = Math.Cos(alfa);

                m_res.uMatrix[2, 2] = 1;
            }
            
            return m_res;
        }
    }
}