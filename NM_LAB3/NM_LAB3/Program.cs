
using NM_LAB3;

namespace NM_LAB3
{
    class Laba
    {
        public int N;
        public int diap;
        public int count_iterations;
        public decimal accuracy;
        public decimal acc_lambda;
        public decimal acc_x;
        public decimal r;

        public Laba()
        {
            N = 0;
            diap = 0;
            accuracy = 0;
            acc_lambda = 0;
            acc_x = 0;
            r = 0;
            count_iterations = 0;
        }
        public Laba(int n, int Diap, int it, decimal eps)
        {
            N = n;
            diap = Diap;
            accuracy = eps;
            Test(n, Diap, it, eps, ref count_iterations, ref acc_lambda, ref acc_x, ref r);

        }
        static void Generation(int n, int diap)
        {
            decimal[] arr = new decimal[n];
            int ind = 0;
            Random rand = new Random();
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = (decimal)(rand.Next(2 * diap * 1000) / 1000.0 - diap);
                if (Math.Abs(arr[i]) >= Math.Abs(arr[ind]))
                    ind = i;
            };


            Matrix lambd = new Matrix(arr); // матрица со собственными значениями
            Matrix w = new Matrix(n, 1);
            for (int i = 0; i < w.rows; i++) // генерация вектора
                w.matrix[i, 0] = (decimal)(rand.Next(2 * diap * 1000) / 1000.0 - diap);
            w.Normalizing(); // нормализация вектора

            for (int i = 0; i < arr.Length; i++)
                arr[i] = 1;

            Matrix E = new(arr);

            Matrix H = E - 2 * (w * w.Transp()); // матрицы собственных векторов
            Matrix A = H * lambd * H;

            using StreamWriter sw = new("data.txt"); // запись в файл
            sw.WriteLine(n); // размерность
            sw.WriteLine();

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                    sw.Write(string.Format("{0:F30}", A.matrix[i, j]) + ' ');
                sw.WriteLine();
            }
            sw.WriteLine();

            sw.Write(string.Format("{0:F30}", lambd.matrix[ind, ind]) + ' ');
            sw.WriteLine('\n');

            for (int j = 0; j < n; j++)
                sw.Write(string.Format("{0:F30}", H.matrix[ind, j]) + ' ');
            sw.WriteLine();
            sw.Close();
        }

        static bool Solution(int it_count, decimal eps)
        {
            using StreamReader sr = new("data.txt"); // чтение с файла
            using (StreamWriter sw = new("res.txt")) // нужно только часть информации, поэтому сразу записываем в результат остальное
            {
                int n = Convert.ToInt32(sr.ReadLine()); // размерность
                string nums;
                string[] nums_arr;
                sr.ReadLine();

                Matrix A = new(n); // матрица
                for (int i = 0; i < n; i++)
                {
                    nums = sr.ReadLine();
                    nums_arr = nums.Split();
                    for (int j = 0; j < n; j++)
                        A.matrix[i, j] = decimal.Parse(nums_arr[j]);
                }
                sr.ReadLine();

                nums = sr.ReadLine();
                sw.WriteLine(nums);
                sr.ReadLine();
                sw.WriteLine();

                Matrix x_n = new(n, 1); // собственный вектор
                for (int i = 0; i < n; i++)
                    x_n.matrix[i, 0] = 1;

                nums = sr.ReadLine();
                sw.WriteLine(nums);
                sw.WriteLine();

                Matrix v = new();
                int it = 0;
                Matrix prev_v = new();
                decimal acc = 0;
                Matrix prev_lambda = new();
                Matrix lambda = new();

                // обработка
                while (it < it_count && (eps < acc || it <= 1))
                {
                    v = x_n;
                    v.Normalizing();

                    x_n = A * v;

                    lambda = v.Transp() * x_n;

                    if (it != 0)
                    {
                        acc = 0;
                        for (int i = 0; i < v.cols; i++)
                            acc = Math.Max(acc, Math.Abs(prev_v.matrix[0, i] - v.matrix[0, i]));
                        acc = Math.Max(acc, Math.Abs(lambda.matrix[0, 0] - prev_lambda.matrix[0, 0]));
                    }

                    prev_lambda = lambda;
                    prev_v = v;
                    it++;
                }

                sw.WriteLine(string.Format("{0:F30}", lambda.matrix[0, 0]) + "\n");

                for (int i = 0; i < n; i++)
                    sw.Write(string.Format("{0:F30}", v.matrix[i, 0]) + " ");
                sw.WriteLine("\n");

                sw.WriteLine(it + "\n");

                decimal r = 0M;
                Matrix delt = (A * x_n) - (x_n * lambda);
                for (int i = 0; i < n; i++)
                    r = Math.Max(Math.Abs(delt.matrix[i, 0]), r);
                sw.WriteLine(string.Format("{0:F30}", r) + "\n");

                sw.Close();
            }

            sr.Close();
            return true;
        }

        public static decimal Sqrt(decimal n, decimal eps = 0.0M)
        {

            decimal current = (decimal)Math.Sqrt((double)n), previous;
            do
            {
                previous = current;
                if (previous == 0.0M) return 0;
                current = (previous + n / previous) / 2;
            } while (Math.Abs(previous - current) > eps);
            return current;
        }

        public static void Test(int n, int diap, int it, decimal eps, ref int count_iterations, ref decimal acc_lambda, ref decimal acc_x, ref decimal r_new)
        {
            Generation(n, diap);
            Solution(it, eps);

            using StreamReader sr = new("res.txt");
            decimal lambda = decimal.Parse(sr.ReadLine());
            sr.ReadLine();

            string str = sr.ReadLine();
            string[] strs = str.Split();
            Matrix vec = new(1, n);
            for (int i = 0; i < n; i++)
                vec.matrix[0, i] = Math.Abs(decimal.Parse(strs[i]));
            sr.ReadLine();

            lambda -= decimal.Parse(sr.ReadLine());
            lambda = Math.Abs(lambda); //Важно
            sr.ReadLine();

            str = sr.ReadLine(); //Считывание нового вектора x
            strs = str.Split();
            for (int i = 0; i < n; i++)
                vec.matrix[0, i] -= Math.Abs(decimal.Parse(strs[i]));
            sr.ReadLine();

            decimal vec_acc = 0;
            for (int i = 0; i < n; i++)
                vec_acc = Math.Max(vec_acc, vec.matrix[0, i]); //Важно

            int it_t = Convert.ToInt32(sr.ReadLine()); //Важно
            sr.ReadLine();

            decimal r = decimal.Parse(sr.ReadLine()); //Важно

            acc_lambda = lambda;
            acc_x = vec_acc;
            r_new = r;
            count_iterations = it_t;

            using StreamWriter sw = new("acc_res.txt", true);
            sw.WriteLine(n + " " + diap + " " + string.Format("{0:E2}", eps) + " " + string.Format("{0:E2}", lambda) + " " + string.Format("{0:E2}", vec_acc) + " " + string.Format("{0:E2}", r) + " " + it_t);
            sw.Close();
        }

        public static void Main(string[] args)
        {
            int [] nn = {10, 30, 50};
            int[] lam = { 2, 50 };
            decimal[] ac = { 1E-5M, 1E-8M};
            //for (int s = 0; s < 3; ++s)
                //for (int l = 0; l < 2; ++l)
                    for (int k = 0; k < 2; ++k)
                    {
                        const int N = 10;
                        Laba[] m = new Laba[N];
                        for (int i = 0; i < N; i++)
                            m[i] = new Laba(nn[2], lam[1], 1000, ac[k]);

                        int N_ = m[0].N;
                        int diap = m[0].diap;
                        decimal accuracy = m[0].accuracy;
                        decimal average_acc_lambda = 0;
                        decimal average_acc_x = 0;
                        decimal average_r = 0;
                        decimal average_count_iterations = 0;

                        for (int i = 0; i < N; i++)
                        {
                            average_acc_lambda += m[i].acc_lambda;
                            average_acc_x += m[i].acc_x;
                            average_r += m[i].r;
                            average_count_iterations += m[i].count_iterations;
                        }
                        average_acc_lambda /= N;
                        average_acc_x /= N;
                        average_r /= N;
                        average_count_iterations /= N;
                        Console.WriteLine($"{average_acc_lambda} {average_acc_x} {average_r} {average_count_iterations}");



                    }
        }
    }

}