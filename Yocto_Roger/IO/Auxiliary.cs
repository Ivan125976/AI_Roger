using System.Globalization;
using System.Text;
using Yocto_Roger.Yocto_Roger;
using static Yocto_Roger.IO.MainIO;

namespace Yocto_Roger.IO
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
        public static string BuildStringMatrix(double[,]? matrix) // TODO: Добавить сюда и в другие функции такого же типа, принимать символ по которому будут разделятся значения
        {
            if (matrix != null)
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
            else
                return string.Empty;
        }

        /// <summary>
        /// Строит все значения из матрицы с вложенными массивами в одну строчку разделяя их точкой с запятой. Важно, что если переденная переменная будет пустой (null), то программа упадёт с исключением, NullReference исключения, требется обрабатывать (если честно то не только здесь, а всегда).
        /// </summary>
        /// <param name="jaggedMatrix">Матрица со вложенными массивами</param>
        /// <returns></returns>
        public static string? BuildStringJaggedMatrix(double[][,]? jaggedMatrix)
        {
            if (jaggedMatrix == null)
                return null;

            StringBuilder builder = new();

            for (int iM = 0; iM < jaggedMatrix.Length; iM++)
            {
                int rows = jaggedMatrix[iM].GetLength(0);
                int cols = jaggedMatrix[iM].GetLength(1);

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        builder.Append(Convert.ToString(jaggedMatrix[iM][i, j], CultureInfo.InvariantCulture)).Append(';');
                    }
                }
            }

            if (builder.Length > 0)
                builder.Length--; // Удаление последнего символа ';'

            return builder.ToString();
        }

        public static string BuildStringArray(dynamic? array)
        {
            StringBuilder builder = new();

            if (array != null)
            {
                foreach (object v in array)
                {
                    builder.Append(Convert.ToString(v, CultureInfo.InvariantCulture) + ';');
                }

                if (builder.ToString().EndsWith(';'))
                    builder.Length--; // Удаляет последний ненужный символ ';'

                return builder.ToString();
            }
            else
                return string.Empty;
        }

        private static void WriteArray(StreamWriter writer, double[] array, bool line_break = false)
        {
            if (array != null)
            {
                foreach (double v in array)
                    writer.Write(v.ToString(CultureInfo.InvariantCulture) + ';');
            }

            if (line_break)
                writer.WriteLine();
        }

        /// <summary>
        /// Преобразует в нужные типы и инициализирует данные (строки) из переданного объекта в соответствующие переменные. Если передан null, он инициализирует значения по умолчанию
        /// </summary>
        /// <param name="roger"></param>
<<<<<<< HEAD:Yocto_Roger/IO/Auxiliary.cs
        public static void InitRogersData(MainIO.Roger roger)
=======
        public static void InitRogersData(Roger? roger)
>>>>>>> c496e0713b22dc4e19d5bf43cc380c0767d57f46:Yocto_Roger/Auxiliary.cs
        {
            //Parameters.version = roger.AIversion; если надо -- разкомментируй

            Parameters.passes = roger?.Passes ?? 500;
            Parameters.learningRate = roger?.LearingRate ?? 0.02f;
            Parameters.DropOutPercent = roger?.DropOutPercent ?? 3.0f;

            Parameters.inputNeuronsCount = roger?.InputNeuronsCount ?? 14;
            Parameters.middleNeuronsCount = roger?.MiddleNeuronsCount ?? 16;
            Parameters.outputNeuronsCount = roger?.OutputNeuronsCount ?? 8;

            Parameters.layers = roger?.Layers ?? 3;
            Parameters.Mlayers = roger?.MLayers ?? 1; // Layers -2
        }

        public static double[,]? ReadMatrixFromArray(int[]? obj)
        {
            if (obj != null && obj.Length > 0)
            {
                byte rows = 3;
                byte columns = 2;

                double[,] matrix = new double[rows, columns];
                int index = 0;
                for (int r = 0; r < rows; r++)
                {
                    for (int c = 0; c < columns; c++)
                    {
                        if (index < obj.Length)
                        {
                            matrix[r, c] = obj[index];
                            index++;
                        }
                        else
                        {
                            matrix[r, c] = 0.0;
                        }
                    }
                }
                return matrix;
            }
            else return null;
        }

        public static double[,]? ReadMatrixFromDoublesArray(double[]? obj)
        {
            byte rows = 3;
            byte columns = 2;

            double[,] matrix = new double[rows, columns];
            int index = 0;

            if (obj != null && obj.Length != 0)
            {
                for (int r = 0; r < rows; r++)
                {
                    for (int c = 0; c < columns; c++)
                    {
                        if (index < obj!.Length)
                        {
                            matrix[r, c] = obj[index];
                            index++;
                        }
                        else
                        {
                            matrix[r, c] = 0.0;
                        }
                    }
                }
            }
            else return null;

            return matrix;
        }

        public static double[][,] ReadJaggedMatrixFromArray(double[]? obj, byte matrixCount = 1)
        {
            if (obj == null || obj.Length == 0 || matrixCount == 0)
            {
                return [];
            }

            byte rows = 3;
            byte columns = 2;

            double[][,] matrix = new double[matrixCount][,];
            int index = 0;

            for (int i = 0; i < matrixCount; i++)
            {
                matrix[i] = new double[rows, columns];

                for (int r = 0; r < rows; r++)
                {
                    for (int c = 0; c < columns; c++)
                    {
                        if (index < obj.Length)
                        {
                            matrix[i][r, c] = obj[index];
                            index++;
                        }
                    }
                }
            }

            return matrix;
        }
    }
}
