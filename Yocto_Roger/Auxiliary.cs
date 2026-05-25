using System.Globalization;
using System.Text;
using static Yocto_Roger.IO;

namespace Yocto_Roger
/* 
Yocto Roger ;)
*****************
*Emotion Corp ;)*
*****************
Copyright 2025-2026 Emotion Corp.
Internal extension I/O lib
*/
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

        /// <summary>
        /// Узкоспециализированная функция для записи матрицы врайтером
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="matrix"></param>
        /// <param name="line_break">Перенос строки после записи</param>
        public static void WriteMatrix(StreamWriter writer, double[,] matrix, bool line_break = false)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                for (int i = 0; i < matrix.GetLength(0); i++)
                    writer.Write(matrix[i, j].ToString(CultureInfo.InvariantCulture) + ";");

                if (line_break)
                    writer.WriteLine();
            }
        }

        public static void WriteJaggedMatrix(StreamWriter writer, double[,,] matrix, bool line_break = false)
        {
            for (byte j = 0; j < matrix.GetLength(1); j++)
            {
                for (byte i = 0; i < matrix.GetLength(0); i++)
                {
                    for (byte c = 0; c < matrix.GetLength(2); c++)
                        writer.Write(matrix[i, j, c].ToString(CultureInfo.InvariantCulture) + ";");
                }

                if (line_break)
                    writer.WriteLine();
            }
        }
        /// <summary>
        /// Преобразует матрицу в строку, разделяя каждое значение точкой с запятой
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns>Строка в которой по порядку содержатся элементы матрицы, разделённые точкой с запятой</returns>
        public static string BuildStringMatrix(double[,] matrix) // TODO: Добавить сюда и в другие функции такого же типа, принимать символ по которому будут разделятся значения
        {
            StringBuilder builder = new();

            for (byte j = 0; j < matrix.GetLength(1); j++)
            {
                for (byte i = 0; i < matrix.GetLength(0); i++)
                    builder.Append(matrix[i, j].ToString(CultureInfo.InvariantCulture) + ";");
            }

            if (builder.ToString().EndsWith(';'))
                builder.Length--; // Удаляет последний ненужный символ ';'

            return builder.ToString();
        }

        /// <summary>
        /// Строит все значения из матрицы с вложенными массивами в одну строчку разделяя их точкой с запятой. Очень важно вводить корректный второй параметр, от этого зависит упадёт ли программа сс исключением или нет
        /// </summary>
        /// <param name="jaggedMatrix">Матрица со вложенными массивами</param>
        /// <param name="maxIndexOfMatrix">Колл-во вложенных массивов</param>
        /// <returns></returns>
        public static string BuildStringJaggedMatrix(double[][,] jaggedMatrix, byte maxIndexOfMatrix)
        {
            StringBuilder builder = new();

            for (int iM = 0; iM < maxIndexOfMatrix; iM++)
            {
                for (int j = 0; j < jaggedMatrix[iM].GetLength(1); j++)
                {
                    for (int i = 0; i < jaggedMatrix[iM].GetLength(0); i++)
                    {
                        builder.Append(Convert.ToString(jaggedMatrix[iM][i, j], CultureInfo.InvariantCulture) + ";");
                    }
                }
            }

            if (builder.ToString().EndsWith(';'))
                builder.Length--; // Удаляет последний ненужный символ ';'

            return builder.ToString();
        }

        public static string BuildStringArray(dynamic array)
        {
            StringBuilder builder = new();

            foreach (dynamic v in array)
            {
                builder.Append(v.ToString(CultureInfo.InvariantCulture) + ';');
            }

            if (builder.ToString().EndsWith(';'))
                builder.Length--; // Удаляет последний ненужный символ ';'

            return builder.ToString();
        }

        private static void WriteArray(StreamWriter writer, double[] array, bool line_break = false)
        {
            foreach (double v in array)
                writer.Write(v.ToString(CultureInfo.InvariantCulture) + ';');

            if (line_break)
                writer.WriteLine();
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
