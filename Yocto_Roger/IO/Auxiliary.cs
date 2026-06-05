using System.Globalization;
using System.Text;
using Yocto_Roger.Yocto_Roger;

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
    /// <summary>
    /// Auxiliary class, and contains methods for work with array's
    /// </summary>
    public class Auxiliary(Parameters param)
    {
        private readonly Parameters _param = param;

        /// <summary>
        /// Function which writing any array, with you writer using foreach.
        /// </summary>
        /// <param name="array">array you want to write</param>
        /// <param name="writer"></param>
        /// <param name="line_break">if true, function will do writer.WriteLine() in the end, after it foreach your array</param>
        public static void WriteAll(dynamic array, StreamWriter writer, bool line_break = false)
        {
            foreach (var element in array)
                writer.Write(element.ToString(CultureInfo.InvariantCulture) + ";");

            if (line_break == true)
                writer.WriteLine();
        }

        /// <summary>
        /// Function, is analnog WriteAll, but writting matrix ([,])
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="matrix">double[,]</param>
        /// <param name="line_break">Line break after all operation with array</param>
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

        /// <summary>
        /// Writing double[,,] array in the string
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="matrix">double[,,]</param>
        /// <param name="line_break">Line break after all operation with your array</param>
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
        /// Matrix to string, separating values with ';'
        /// </summary>
        /// <param name="matrix">double[,] (Can be null, then return is String.Empty)</param>
        /// <returns>String with all values in your matrix, separated ';'</returns>
        public static string BuildStringMatrix(double[,]? matrix)
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
        /// Building the string from your double[][,] array, separating values with ';'
        /// </summary>
        /// <param name="jaggedMatrix">double[][,] (Can be null, then function return null, it's fair, right?)</param>
        /// <returns>String with all values in your matrix, separated ';'</returns>
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
                builder.Length--; // Removing the last symbol ';'

            return builder.ToString();
        }


        /// <summary>
        /// Simple building the string with your array, which can have any type
        /// </summary>
        /// <param name="array">Any type array. Can be null, then function returning String.Empty</param>
        /// <returns></returns>
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
                    builder.Length--; // Removing the last symbol ';'

                return builder.ToString();
            }
            else
                return string.Empty;
        }

        /// <summary>
        /// Analog function WriteAll, i don't know, why it's still exists
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="array"></param>
        /// <param name="line_break"></param>
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
        public void InitRogersData(MainIO.Roger? roger)
        {

            _param.passes = roger?.Passes ?? 500;
            _param.learningRate = roger?.LearingRate ?? 0.02f;
            _param.DropOutPercent = roger?.DropOutPercent ?? 3.0f;

            _param.inputNeuronsCount = roger?.InputNeuronsCount ?? 14;
            _param.middleNeuronsCount = roger?.MiddleNeuronsCount ?? 16;
            _param.outputNeuronsCount = roger?.OutputNeuronsCount ?? 8;

            _param.layers = roger?.Layers ?? 3;
        }

        /// <summary>
        ///Reading int array, and building from it, double matrix array. 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>double[,] array. If null was passed to the function, then it gonna return null too. </returns>
        public static double[,]? ReadMatrixFromArray(int[]? obj)
        {
            if (obj != null && obj.Length > 0)
            {
                double[,] matrix = new double[3, 2];
                int index = 0;
                for (int r = 0; r < 3; r++)
                {
                    for (int c = 0; c < 2; c++)
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

        /// <summary>
        /// Simple reading doubles array, and tranforming it in the double[,] array. 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>double[,] array, If null was passed to the function the it gonna return null too</returns>
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

        /// <summary>
        ///Reading double[] array, and building double[][,] from it 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="matrixCount"></param>
        /// <returns>double[][,] array.If null or empty array was passed to the function, then it returns fully empty array</returns>
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
