#pragma once
#include <ctime>
#include <fstream>
#include <iomanip>
#include <iostream>
#include <vector>

typedef std::vector<std::vector<double>> Matrix;
typedef  std::vector <double> Massiv;

void Task();
void Task1();
void Task2();
void Task3();
double Task3_1(int, int, int, int);
double Task3_2(int, int, int, int, double);
void Input(int&, int&, int&, int&);
void Print(const Matrix&, const Massiv&, std::string);
void Print(const Matrix&, const Matrix&);
void Print(const Massiv&, const Massiv&, std::string = "");
void Print(const Matrix&, std::string = "", bool = false);
void Print(const Massiv&, std::string = "", bool = true);
void FileInput(Matrix&, Massiv&, int&, int&);
void InitMassiv(Massiv&, int);
void InitMatrix(Matrix&, int, bool = false);
void ScaleMatrix(Matrix&, int, double);
void Create(Matrix&, Massiv&, int, int, int, int);
void CreateBeltMatrix(Matrix&, int, int, int);
void CreateMassiv(Massiv&, size_t, int, bool = true);
double GenerateNum(int);
void CalculateColumnMatrixB(Massiv&, Matrix&, int, int);
void MakeB(Matrix&, Matrix&, int);
void MakeC(Matrix&, Matrix&, int);
void Decomposition(Matrix&, Matrix&, Matrix&, int);
double SumforXY(Matrix&, Massiv&, int, int);
double SumforFY(Matrix&, Massiv&, int, int);
double Calculate(Matrix&, Matrix&, Matrix&, Massiv&, Massiv&, Massiv&, int);
void CalculateFromFToY(Matrix&, Massiv&, Massiv&, int);
void CalculateFromYToX(Matrix&, Massiv&, Massiv&, int);
void CalculateFromXToY(Matrix&, Massiv&, Massiv&, int);
void CalculateFromYToF(Matrix&, Massiv&, Massiv&, int);
void CalculateFromFToX(Matrix&, Matrix&, Massiv&, Massiv&, int);
void CalculateFromXToF(Matrix&, Matrix&, Massiv&, Massiv&, int);
double CalculateInaccuracyForX(Massiv&, Massiv&, int);
double CalculateInaccuracy(double, double);

void Task()
{
	bool flag = true;
	while (flag)
	{

		int k;

		std::cout << "Выберите: \n1. Считать из файла\n2. Сгенерировать случайную матрицу\n3. Сгенерировать несколько матриц\n4. Выход\n";
		std::cin >> k;
		while (k < 0 and k > 3)
		{
			std::cout << "Неккоректный ввод, попробуйте ещё раз\n";
			std::cin >> k;
		}
		if (k == 1)
			Task1();

		if (k == 2)
			Task2();

		if (k == 3)
			Task3();

		if (k == 4)
			flag = false;

	}
}

void Task1()
{
	int N, L;
	Matrix B, C, belt_matrix;
	Massiv f_main, x;
	FileInput(belt_matrix, f_main, N, L);
	Print(belt_matrix, f_main, "Вектор f");

	Decomposition(belt_matrix, B, C, N);
	Print(B, C);

	CalculateFromFToX(B, C, f_main, x, N);
	Print(x, "Вектор x", true);

}

void Task2()
{
	int N, L, range_matrix, range_x;
	Massiv x_main, x_new, f;
	Matrix belt_matrix, B, C;
	double inaccuracy = 0;

	Input(N, L, range_matrix, range_x);
	Create(belt_matrix, x_main, N, L, range_matrix, range_x);
	Print(belt_matrix, x_main, "Вектор x");

	inaccuracy = Calculate(belt_matrix, B, C, x_main, f, x_new, N);
	Print(f, "Вектор f", true);
	Print(x_main, x_new, "Сравнение сгенерированного и посчитанного векторов x");
	
	std::cout << "Относительная погрешность: " << std::scientific << std::setprecision(16) << inaccuracy << std::endl;

}

void Task3()
{
	bool flag = true;
	double inaccuracy = 0, scale;
	int k, count, m, N, L, range_matrix, range_x;
	
	std::cout << "Сколько матриц сгенерировать?\n";
	std::cin >> count;
	std::cout << "Какие матрицы сгенерировать?\n1. Хорошо обусловленную\n2. Плохо обусловленную\n";
	std::cin >> m;

	if (m != 1)
		flag = false;
	if (!flag)
	{
		std::cout << "Введите множитель: ";
		std::cin >> scale;
	}
	Input(N, L, range_matrix, range_x);
	k = count;
	if (flag)
		while (k--)
			inaccuracy += Task3_1(N, L, range_matrix, range_x);
	else
		while (k--)
			inaccuracy += Task3_2(N, L, range_matrix, range_x, scale);
	std::cout << "Средняя относительная погрешность: " << std::scientific << std::setprecision(16) << inaccuracy / count << std::endl;
}

double Task3_1(int N, int L, int range_matrix, int range_x)
{
	Massiv x_main, x_new, f;
	Matrix belt_matrix, B, C;

	Create(belt_matrix, x_main, N, L, range_matrix, range_x);
	
	return Calculate(belt_matrix, B, C, x_main, f, x_new, N);
}

double Task3_2(int N, int L, int range_matrix, int range_x, double scale)
{
	Massiv x_main, x_new, f;
	Matrix belt_matrix, B, C;

	Create(belt_matrix, x_main, N, L, range_matrix, range_x);
	ScaleMatrix(belt_matrix, N, scale);
	
	return Calculate(belt_matrix, B, C, x_main, f, x_new, N);

}

void Input(int& N, int& L, int& range_matrix, int& range_x)
{
	std::cout << "Введите N: ";
	std::cin >> N;
	std::cout << "Введите L: ";
	std::cin >> L;
	std::cout << "Введите диапазон значений для матрицы: ";
	std::cin >> range_matrix;
	std::cout << "Введите диапазон значение для вектора x: ";
	std::cin >> range_x;
}

void Print(const Matrix& matrix, const Massiv& f_main, std::string name)
{
	Print(matrix, "Ленточная матрица");
	Print(f_main, name);
}

void Print(const Matrix& B, const Matrix& C)
{
	Print(B, "Нижнетреугольная матрица B:");
	Print(C, "Верхнетреугольная матрица C", true);
}

void Print(const Massiv& A, const Massiv& B, std::string name)
{
	if (name.length())
	{
		std::cout << name << std::endl;
		std::cout.clear();
	}

	for (int i(0); i < A.size(); ++i)
		std::cout << std::scientific << std::setprecision(16) << std::setw(8) << A[i] << '\t' << B[i] << std::endl;
	std::cout << std::endl;
}

void Print(const Massiv& massiv, std::string name, bool flag)
{
	if (name.length())
	{
		std::cout << name << std::endl;
		std::cout.clear();
	}
	for (const auto& elem : massiv)
	{
		std::cout << std::setprecision(4) << std::setw(8) << std::right << elem;
		if (flag)
			std::cout << std::endl;
	}
	if (flag)
		std::cout << std::endl;
}

void Print(const Matrix& matrix, std::string name, bool flag)
{
	std::cout << "-----------------------------------------------------------------------" << std::endl;
	std::cout << name << std::endl;
	std::cout.clear();
	int count = 0;
	for (const auto& massiv : matrix)
	{
		if (flag)
			for (int i(0); i < count; ++i)
				std::cout << "\t";
		Print(massiv, "", false);
		std::cout << std::endl;
		++count;
	}
	std::cout << "-----------------------------------------------------------------------" << std::endl;
}

void FileInput(Matrix& belt_matrix, Massiv& f, int& N, int& L)
{
	std::ifstream file("Input.txt");
	file >> N >> L;
	belt_matrix.resize(N);
	f.resize(N);
	for (int i(0); i < N; ++i)
	{
		int temp;
		int K = std::min(L, N - i);
		belt_matrix[i].resize(K);
		for (int j(0); j < K; ++j)
			file >> belt_matrix[i][j];
		for (int k(0); k < L - K; ++k)
			file >> temp;
	}
	for (int i(0); i < N; ++i)
		file >> f[i];
}

void InitMassiv(Massiv& mas, int N)
{
	mas.resize(N);
	for (auto& e : mas)
		e = 0;
}

void InitMatrix(Matrix& X, int N, bool flag)
{
	X.resize(N);
	for (int i(0); i < N; ++i)
		InitMassiv(X[i], i + 1);
	if (flag)
		std::reverse(X.begin(), X.end());
}

void ScaleMatrix(Matrix& belt, int N, double scale)
{
	double k = 1;
	while (scale--)
		k /= 10;
	for (auto& line : belt)
		for (auto& elem : line)
			elem *= k;
}

void Create(Matrix& belt_matrix, Massiv& vector, int N, int L, int range_matrix, int range_x)
{
	CreateBeltMatrix(belt_matrix, N, L, range_matrix);
	InitMassiv(vector, N);
	CreateMassiv(vector, N, range_x, false);
}

void CreateBeltMatrix(Matrix& matrix, int N, int L, int range)
{
	matrix.resize(N);
	for (int i(0); i < N; ++i)
		CreateMassiv(matrix[i], std::min(L, N - i), range);
}

void CreateMassiv(Massiv& massiv, size_t N, int range, bool flag)
{
	massiv.resize(N);
	int i = 0;
	if (flag)
	{
		do
			massiv[i] = GenerateNum(range);
		while (massiv[i] == 0);
		++i;
	}
	for (; i < N; ++i)
		massiv[i] = GenerateNum(range);
}

double GenerateNum(int range)
{
	double number = (double)rand() * (2 * range) / RAND_MAX - range;
	return number;
}

void CalculateColumnMatrixB(Massiv& belt_line, Matrix& B, int line, int N)
{
	int column = line;
	for (; line < N; ++line)
	{
		double sum = 0;
		for (int k(0); k < column; ++k)
			sum += B[line][k] * B[column][k] / B[k][k];
		if (line - column < belt_line.size())
			B[line][column] = belt_line[line - column] - sum;
		else
			B[line][line] = 0;
	}
}

void MakeB(Matrix& B, Matrix& belt_matrix, int N)
{
	for (int col(0); col < N; ++col)
		CalculateColumnMatrixB(belt_matrix[col], B, col, N);

}

void MakeC(Matrix& B, Matrix& C, int N)
{
	/*	i	j	l	k
	*	0	0	0	0
	*	0	1	1	0
	*	0	2	2	0
	*	0	N	N	0
	*	1	0	1	1
	*	1	1	2	1
	*	1	N-i	N	1
	*	2	0	2	2
	*/

	for (int i(0); i < N; ++i)
		for (int j(0), l(i); j < N - i; ++j, ++l)
			C[i][j] = B[l][i] / B[i][i];

}

void Decomposition(Matrix& belt_matrix, Matrix& B, Matrix& C, int N)
{
	InitMatrix(B, N);
	InitMatrix(C, N, true);
	MakeB(B, belt_matrix, N);
	MakeC(B, C, N);
}

double SumforXY(Matrix& C, Massiv& x, int N, int i)
{
	double sum = 0;
	for (int j(i + 1), l(1); j < N; ++j, ++l)
		sum += C[i][l] * x[j];
	return sum;
}

double SumforFY(Matrix& B, Massiv& y, int N, int i)
{
	double sum = 0;
	for (int k(0); k < i; ++k)
		sum += B[i][k] * y[k];
	return sum;
}

double Calculate(Matrix& belt_matrix, Matrix& B, Matrix& C, Massiv& x_main, Massiv& f, Massiv& x_new, int N)
{
	Decomposition(belt_matrix, B, C, N);

	CalculateFromXToF(B, C, x_main, f, N);

	CalculateFromFToX(B, C, f, x_new, N);

	return CalculateInaccuracyForX(x_main, x_new, N);
}

void CalculateFromFToY(Matrix& B, Massiv& f, Massiv& y, int N)
{
	for (int i(0); i < N; ++i)
		y[i] = (f[i] - SumforFY(B, y, N, i)) / B[i][i];
}

void CalculateFromYToX(Matrix& C, Massiv& y, Massiv& x, int N)
{
	for (int i(N - 1); i >= 0; --i)
		x[i] = y[i] - SumforXY(C, x, N, i);
}

void CalculateFromXToY(Matrix& C, Massiv& x, Massiv& y, int N)
{
	for (int i(N - 1); i >= 0; --i)
		y[i] = x[i] + SumforXY(C, x, N, i);
}

void CalculateFromYToF(Matrix& B, Massiv& y, Massiv& f, int N)
{
	for (int i(0); i < N; ++i)
		f[i] = y[i] * B[i][i] + SumforFY(B, y, N, i);
	//y[i] = (f[i] - SumforFY(B, y, N, i)) / B[i][i];
}

void CalculateFromFToX(Matrix& B, Matrix& C, Massiv& f, Massiv& x, int N)
{
	Massiv y;
	InitMassiv(y, N);
	InitMassiv(x, N);

	CalculateFromFToY(B, f, y, N);
	CalculateFromYToX(C, y, x, N);

}

void CalculateFromXToF(Matrix& B, Matrix& C, Massiv& x, Massiv& f, int N)
{
	Massiv y;
	InitMassiv(y, N);
	InitMassiv(f, N);

	CalculateFromXToY(C, x, y, N);
	CalculateFromYToF(B, y, f, N);
}

double CalculateInaccuracyForX(Massiv& x1, Massiv& x2, int N)
{
	double arithmetic_mean = 0;
	for (int i(0); i < N; ++i)
		arithmetic_mean = std::max(CalculateInaccuracy(x1[i], x2[i]), arithmetic_mean);
	return arithmetic_mean;
}

double CalculateInaccuracy(double x1, double x2)
{
	double abs = x1 - x2;
	return std::abs(abs / std::fmax(1, x2));

}


int main()
{
	setlocale(LC_ALL, "Russian");
	Task();
	return 0;
}