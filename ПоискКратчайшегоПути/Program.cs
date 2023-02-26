using System;
using System.CodeDom;
using System.IO;
using System.Linq;

namespace ПоискКратчайшегоПути
{
	internal class Program
	{

		static void Main(string[] args)
		{
			int[,] M = new int[19, 3] { { 8, 5, 6 },
									   { 8, 6, 4 },
									   { 8, 7, 1 },
									   { 7, 2, 1 },
									   { 7, 3, 5 },
									   { 7, 6, 3 },
									   { 6, 4, 5 },
									   { 6, 5, 3 },
									   { 5, 4, 1 },
									   { 5, 1, 4 },
									   { 4, 1, 2 },
									   { 4, 3, 1 },
									   { 4, 5, 1 },
									   { 3, 1, 4 },
									   { 3, 2, 3 },
									   { 3, 4, 1 },
									   { 2, 1, 7 },
									   { 2, 3, 3 },
									   { 2, 7, 1 },
									};
			int[,] Y = makeY(M);

			for (int i = 0; i < Y.GetLength(0); i++)
			{
				for (int j = 0; j < 2; j++)
					Console.Write(Y[i, j] + "  ");
				Console.WriteLine();
			}

			Y = calcY(M);
			Console.WriteLine();
			Console.WriteLine();
			for (int i = 0; i < Y.GetLength(0); i++)
			{
				for (int j = 0; j < Y.GetLength(1); j++)
					Console.Write(Y[i, j] + "  ");
				Console.WriteLine();

			}

			var tuple = minPath(M);
			Console.Write(tuple.Item1 + " ед.  ");
			foreach (var item in tuple.Item2)
			{
				Console.Write("->" + item);
			}
			Console.ReadLine();
		}
		//Программно  реализовать  модуль,
		//который  создает  таблицу  с начальными значениями переменных y(i) для всех узлов сети.
		static int[,] makeY(int[,] M)
		{
			int rows = M.GetLength(0);

			int[] massive_third = new int[rows * 2];
			int k = 0;
			for (int i = 0; i < rows * 2; i++)
			{
				if (i < rows)
				{
					massive_third[i] = M[i, 0];
				}
				else
				{
					massive_third[i] = M[k, 1];
					k++;
				}
			}

			int[] pred_vektor = new int[rows * 2];
			int size = 0;
			for (int i = 0; i < rows * 2; i++)
			{
				if (!pred_vektor.Contains(massive_third[i]))
				{
					pred_vektor[i] = massive_third[i];
					size++;
				}
			}
			int[] vektor = new int[size];
			for (int i = pred_vektor.Length - 1; i >= 0; i--)
			{
				if (pred_vektor[i] != 0)
				{
					vektor[size - 1] = pred_vektor[i];
					size--;
				}
			}

			int[,] Y = new int[vektor.Length, 2];
			for (int i = 0; i < vektor.Length; i++)
			{
				for (int j = 0; j < 2; j++)
				{
					if (j == 0)
					{
						Y[i, j] = vektor[i];
					}
					if (j == 1)
					{
						if (i == vektor.Length - 1)
							Y[i, j] = 0;
						else
							Y[i, j] = 10000;
					}
				}
			}
			return Y;
		}

		static int[,] calcY(int[,] M)
		{
			int[,] Y = makeY(M);
			int sizeY = Y.GetLength(0);
			int sizeM = M.GetLength(0);

			int[] array_inputs = new int[sizeY];
			int[] cache = new int[sizeY];
			for (int i = 0; i < sizeY; i++)
			{
				array_inputs[i] = Y[i, 0];
				cache[i] = Y[i, 1];
			}


			while (true)
			{
				for (int i = sizeM - 1; i >= 0; i--)
				{
					int output = M[i, 0];
					int input = M[i, 1];
					for (int j = sizeY - 1; j >= 0; j--)
					{
						if (output == Y[j, 0])
						{
							int indexOfInput = Array.IndexOf(array_inputs, input);
							int sum = M[i, 2] + cache[indexOfInput];
							if (sum < cache[j])
								cache[j] = sum;
						}
					}
				}

				bool flag = false;
				for (int i = 0; i < sizeY; i++)
					if (Y[i, 1] != cache[i])//тогда нужно еще одну прогонку делать
						flag = true;

				if (flag)
					for (int j = 0; j < sizeY; j++)
						Y[j, 1] = cache[j];
				else
					break;
			}
			return Y;
		}
		static (int, char[]) minPath(int[,] M)
		{
			int[,] Y = calcY(M);
			int minDist = Y[0, 1];
			int sizeY = Y.GetLength(0);
			int sizeM = M.GetLength(0);

			int[] arrayOfM1 = new int[sizeM];
			for (int i = 0; i < sizeM; i++)
				arrayOfM1[i] = M[i, 0];

			int[] array_inputs = new int[sizeY];
			for (int i = 0; i < sizeY; i++)
				array_inputs[i] = Y[i, 0];

			string shortestPath = "";
			shortestPath += Y[0, 0].ToString();

			for (int i = 0; i < sizeM; i++)
			{
				int input = M[i, 1];
				int output = M[i, 0];
				int indexOfInput = Array.IndexOf(array_inputs, input);
				int indexOfOutPut = Array.IndexOf(array_inputs, output);
				if (M[i, 2] + Y[indexOfInput, 1] == Y[indexOfOutPut, 1])
				{
					shortestPath += M[i,1].ToString();
					if (Y[indexOfInput, 0] == Y[sizeY - 1, 0])
						return (minDist, shortestPath.ToCharArray());
					i = Array.IndexOf(arrayOfM1, M[i, 1]) - 1;
				}
			}
			return (0, null);
		}
	}
}

