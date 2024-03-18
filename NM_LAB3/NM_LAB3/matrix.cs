using NM_LAB3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NM_LAB3
{

    internal class Matrix
    {
        public int rows, cols;
        public decimal[,] matrix;

        public Matrix()
        {
            rows = 0;
            cols = 0;
        }
        public Matrix(int n)
        {
            rows = n;
            cols = n;
            matrix = new decimal[rows, cols];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    matrix[i, j] = 0;
        }

        public Matrix(int rows, int cols)
        {
            this.rows = rows;
            this.cols = cols;
            matrix = new decimal[rows, cols];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    matrix[i, j] = 0;
        }

        public Matrix(int rows, int cols, decimal[,] matrix) : this(rows, cols)
        {
            this.matrix = matrix;
        }

        public Matrix(Matrix matrix) : this(matrix.rows, matrix.cols, matrix.matrix)
        { }

        public Matrix(decimal[] arr) : this(arr.Length)
        {
            for (int i = 0; i < this.rows; i++)
                matrix[i, i] = arr[i];
        }

        public Matrix Transp()
        {
            Matrix res = new(this.cols, this.rows);
            for (int i = 0; i < this.rows; i++)
                for (int j = 0; j < this.cols; j++)
                    res.matrix[j, i] = this.matrix[i, j];
            return res;
        }

        public Matrix E(int n)
        {
            Matrix res = new(n);
            for (int i = 0; n < res.rows; n++)
                res.matrix[i, i] = 1;
            return res;
        }

        public void Normalizing()
        {
            decimal n = 0;
            bool flag = false;
            for (int i = 0; i < this.rows; i++)
            {
                if (!flag)
                {
                    if (Math.Abs(this.matrix[i, 0]) - 1E-10M > 0)
                        n += this.matrix[i, 0] * this.matrix[i, 0];
                    else
                    {
                        flag = true;
                        n *= 1E+28M;
                        this.matrix[i, 0] *= 1E+14M;
                        n += this.matrix[i, 0] * this.matrix[i, 0];
                        this.matrix[i, 0] /= 1E+14M;
                    }
                }
                else
                {
                    this.matrix[i, 0] *= 1E+14M;
                    n += this.matrix[i, 0] * this.matrix[i, 0];
                    this.matrix[i, 0] /= 1E+14M;
                }
            }
            n = Laba.Sqrt(n);
            if (flag)
                n /= 1E+14M;
            for (int i = 0; i < this.rows; i++)
                this.matrix[i, 0] /= n;
        }

        public static Matrix operator +(Matrix left, Matrix right)
        {
            if (left.rows == right.rows && left.cols == right.cols)
            {
                Matrix res = new Matrix(left.rows, left.cols);
                for (int i = 0; i < left.rows; i++)
                    for (int j = 0; j < left.cols; j++)
                        res.matrix[i, j] = left.matrix[i, j] + right.matrix[i, j];
                return res;
            }
            else return new Matrix();
        }
        public static Matrix operator -(Matrix left, Matrix right)
        {
            if (left.rows == right.rows && left.cols == right.cols)
            {
                Matrix res = new Matrix(left.rows, left.cols);
                for (int i = 0; i < left.rows; i++)
                    for (int j = 0; j < left.cols; j++)
                        res.matrix[i, j] = left.matrix[i, j] - right.matrix[i, j];
                return res;
            }
            else return new Matrix();
        }

        public static Matrix operator *(Matrix left, Matrix right)
        {
            if (right.rows == left.cols)
            {
                Matrix res = new(left.rows, right.cols);
                for (int i = 0; i < left.rows; i++)
                    for (int j = 0; j < right.cols; j++)
                        for (int k = 0; k < left.cols; k++)
                            res.matrix[i, j] += left.matrix[i, k] * right.matrix[k, j];
                return res;
            }
            else return new Matrix();
        }

        public static Matrix operator *(decimal num, Matrix matrix)
        {

            Matrix res = new Matrix(matrix);
            for (int i = 0; i < res.rows; i++)
                for (int j = 0; j < res.cols; j++)
                    res.matrix[i, j] *= num;
            return res;
        }
        public void Print()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                    Console.Write(matrix[i, j] + " ");
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
