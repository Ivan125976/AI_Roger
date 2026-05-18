using System.Globalization;
using System.Text;
using static Yocto_Roger.IO;

namespace Yocto_Roger
{
    internal class Auxiliary
    {
        public static void WriteAll(dynamic array, StreamWriter writer, bool line_break = false)
        {
            foreach (var element in array)
                writer.Write(element.ToString(CultureInfo.InvariantCulture) + ";");

            if (line_break == true)
                writer.WriteLine();
        }

        public static void WriteMatrix(StreamWriter writer, double[,] matrix)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                for (int i = 0; i < matrix.GetLength(0); i++)
                    writer.Write(matrix[i, j].ToString(CultureInfo.InvariantCulture) + ";");
                writer.WriteLine();
            }
        }

        public static string BuildStringMatrix(double[,] matrix)
        {
            StringBuilder builder = new();

            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                for (int i = 0; i < matrix.GetLength(0); i++)
                    builder.Append(matrix[i, j].ToString(CultureInfo.InvariantCulture) + ";");
            }

            return builder.ToString()
                [..(builder.Length - 1)];
        }

        public static string BuildStringJaggedMatrix(double[][,] jaggedMatrix, byte maxIndexOfMatrix = 2)
        {
            StringBuilder builder = new();

            for (int j = 0; j < jaggedMatrix[0].GetLength(1); j++)
            {
                for (int i = 0; i < jaggedMatrix[0].GetLength(0); i++)
                {
                    for (int iM = 0; iM < Math.Min(maxIndexOfMatrix, jaggedMatrix.Length); iM++)
                    {
                        if (i < jaggedMatrix[iM].GetLength(0) && j < jaggedMatrix[iM].GetLength(1)) // Эту проверку если что не я написал
                            builder.Append(Convert.ToString(jaggedMatrix[iM][i, j], CultureInfo.InvariantCulture) + ";");
                    }
                }
            }

            return builder.ToString()
                [..(builder.Length - 1)]; // Удаляет последнюю ненужную точку с запятой, которая при превращении с массив разделяя точкой с запятой образует ненужные посдений пустой элемент, который лучше сразу здесь удалять чтобы потом не мучатся и не думать где у тебя проблема
        }

        public static string BuildStringArray(dynamic array)
        {
            StringBuilder builder = new();

            foreach (dynamic v in array)
            {
                builder.Append(v.ToString(CultureInfo.InvariantCulture) + ';');
            }

            return builder.ToString()
                [..(builder.Length - 1)]; // Удаляет последний ненужный символ ';'
        }

        private static void WriteArray(StreamWriter writer, double[] array)
        {
            foreach (double v in array)
                writer.Write(v.ToString(CultureInfo.InvariantCulture) + ';');
            //writer.WriteLine();
        }

        /// <summary>
        /// Преобразует в нужные типы и инициализирует данные (строки) из переданного объекта в соответствуюшие переменные 
        /// </summary>
        /// <param name="roger"></param>
        public static void InitRogersData(Roger roger)
        {
            //Parameters.version = roger.AIversion; если надо -- разкомментируй

            NeuralNetwork.inputNeurons = roger.InputNeurons.Split(';').Select(int.Parse).ToArray();
            NeuralNetwork.middleNeurons = ReadMatrixFromArray([.. roger.MiddleNeurons.Split(';').Select(int.Parse)]);
            NeuralNetwork.outputNeurons = roger.OutputNeurons.Split(';').Select(double.Parse).ToArray();

            NeuralNetwork.inputWeights = ReadMatrixFromArray([.. roger.InputWeights.Split(';').Select(int.Parse)]);
            NeuralNetwork.middleWeights = ReadJaggedMatrixFromArray([.. roger.MiddleNeurons.Split(';').Select(double.Parse)]);
            NeuralNetwork.outputWeights = ReadMatrixFromArray([.. roger.OutputWeights.Split(';').Select(int.Parse)]);

            NeuralNetwork.Mbias = ReadMatrixFromArray([.. roger.Mbias.Split(';').Select(int.Parse)]);
            NeuralNetwork.Obias = roger.Obias.Split(';').Select(double.Parse).ToArray();
        }

        public static double[,] ReadMatrixFromArray(int[] obj)
        {
            byte rows = 3;
            byte columns = 2;

            if (obj.Length < rows * columns)
            {
                throw new ArgumentException("В исходном массиве недостаточно элементов для заполнения матрицы 3х2.");
                // Не уверен насчет надобности выкидывания исключения ибо мешает, да и вообще неудобно получается
            }

            double[,] matrix = new double[rows, columns];
            int index = 0;

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    matrix[r, c] = obj[index];
                    index++;
                }
            }

            return matrix;
        }

        public static double[][,] ReadJaggedMatrixFromArray(double[] obj, byte indexOfMatrix = 0)
        {
            byte rows = 3;
            byte columns = 2;

            double[][,] matrix = [];
            int index = 0;

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    for (int i = 0; i < indexOfMatrix; i++)
                    {
                        matrix[i][r, c] = obj[index];
                        index++;
                    }
                }
            }

            return matrix;
        }
    }
}
