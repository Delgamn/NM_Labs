using System;
using System.Net.Http.Headers;
using System.IO;
using System.Drawing;

class Program
{


    public static void FirstStep(Matrix m)
    {
        for (int row = 0; row < m.size - 1; row++)
        {
            m.DivideLine(row);
            m.Subtract(row, row + 1);
        }

        m.DivideLine(m.size - 1);
        SecondStep(m);
    }

    public static void SecondStep(Matrix m)
    {
        for (int row = m.size - 1; row > 0; row--)
        {
            m.Subtract(row, row - 1);
        }
        ThirdStep(m);
    }

    public static void ThirdStep(Matrix m)
    {
        int k_row = m.size - m.k;
        m.DivideLine(k_row);

        for (int row = 0; row < k_row; ++row)
        {
            m.Subtract(k_row, row);
        }

        for (int row = m.size - 1; row > k_row; --row)
        {
            m.Subtract(k_row, row);
        }

        for (int row = 0; row < m.size; row++)
        {
            m.x[row] = m.f[m.size - 1 - row];
            m.x_one[row] = m.f_one[m.size - 1 - row];
        }
    }


    public static int InputSize(string path)
    {
        int size = 0;
        using (StreamReader reader = new StreamReader(path))
        {
            string line = reader.ReadLine();
            if (line != null)
            {
                size = line.Split('\t').Length;
            }
        }
        return size;
    }

    public static void Tests(string inputpath, string outputpath)
    {
        bool flag = false;
        StreamReader reader = new StreamReader(inputpath);
        while (!reader.EndOfStream)
        {
            string line = reader.ReadLine();
            string[] numbers = line.Split(' ');
            int size = int.Parse(numbers[0]);
            int k = size / 2 + 2;
            decimal left = decimal.Parse(numbers[1]);
            decimal right = decimal.Parse(numbers[2]);

            Matrix matrix = new Matrix(size, k);
            matrix.Generate(left, right);
            FirstStep(matrix);
            matrix.AccuracyTest(matrix.x_one);
            matrix.InaccuracyTest(matrix.x, matrix.x_generated);

            using (StreamWriter writer = new StreamWriter(outputpath, flag))
            {
                writer.WriteLine($"Size: {size}; Left: {left}; Right: {right}; Inaccuracy: {matrix.inaccuracy:e}; Accuracy: {matrix.accuracy:e}");
                flag = true;
            }
        }
    }
    static void Main()
    {
        bool menu = true;
        while (menu)
        {
            Console.WriteLine("1) Решить СЛАУ из файла;\n2) Решить сгенерированную СЛАУ;\n3) Набор тестов;\n4) Выход");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    int size = InputSize("D:\\NM_Lab1\\NM_Lab1\\variant.txt");
                    Matrix static_m = new Matrix(size, size / 2 + 2);
                    static_m.InputFromFile("D:\\NM_Lab1\\NM_Lab1\\variant.txt");
                    static_m.PrintToFile("D:\\NM_Lab1\\NM_Lab1\\initial_static_matrix.txt");
                    FirstStep(static_m);
                    static_m.PrintSolutionsToFile("D:\\NM_Lab1\\NM_Lab1\\static_matrix_solutions.txt");
                    static_m.AccuracyTest(static_m.x_one);
                    static_m.PrintAccuracy("D:\\NM_Lab1\\NM_Lab1\\static_matrix_solutions.txt");
                    break;
                case "2":
                    Console.Write("Размер матрицы: ");
                    int n = Convert.ToInt32(Console.ReadLine());
                    int k = n / 2 + 2;
                    Matrix generated_m = new Matrix(n, k);
                    generated_m.Generate(-1, 1);
                    generated_m.PrintToFile("D:\\NM_Lab1\\NM_Lab1\\initial_generated_matrix.txt");
                    FirstStep(generated_m);
                    generated_m.PrintSolutionsToFile("D:\\NM_Lab1\\NM_Lab1\\generated_matrix_solutions.txt");
                    generated_m.PrintGeneratedSolutionsToFile("D:\\NM_Lab1\\NM_Lab1\\generated_matrix_solutions.txt");
                    generated_m.AccuracyTest(generated_m.x_one);
                    generated_m.InaccuracyTest(generated_m.x, generated_m.x_generated);
                    generated_m.PrintAccuracy("D:\\NM_Lab1\\NM_Lab1\\generated_matrix_solutions.txt");
                    generated_m.PrintInaccuracy("D:\\NM_Lab1\\NM_Lab1\\generated_matrix_solutions.txt");
                    break;
                case "3":
                    Tests("D:\\NM_Lab1\\NM_Lab1\\tests.txt", "D:\\NM_Lab1\\NM_Lab1\\result_tests.txt");
                    break;
                case "4":
                    menu = false;
                    break;
                default:
                    Console.WriteLine("Неправильный ввод");
                    break;
            }
        }
    }
}