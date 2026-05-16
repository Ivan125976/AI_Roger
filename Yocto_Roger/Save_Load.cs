using IniParser;
using IniParser.Model;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace Yocto_Roger
{
    internal static class Save_Load
    {
        public static void SaveRoger()
        {
            string fileName;
            int index = 0;

            do
            {
                if (index == 0)
                    fileName = $"roger.roger";
                else
                    fileName = $"roger{index}.roger";
                index++;
            }
            while (File.Exists(fileName));

            using StreamWriter writer = new(fileName);

            writer.WriteLine("[roger]");
            writer.WriteLine($"AIversion = {Parameters.version}");
            writer.WriteLine();

            writer.WriteLine("[neurons]");
            writer.Write("inputNeurons = "); WriteAll(NeuralNetwork.inputNeurons, writer, true);
            writer.Write("middleNeurons = "); WriteAll(NeuralNetwork.middleNeurons, writer, true);
            writer.Write("outputNeurons = "); WriteAll(NeuralNetwork.outputNeurons, writer, true);
            writer.WriteLine();

            writer.WriteLine("[weights]");
            writer.Write("inputWeights = "); WriteAll(NeuralNetwork.inputWeights, writer, true);
            writer.Write("middleWeights = "); WriteAll(NeuralNetwork.middleWeights, writer, true);
            writer.Write("outputWeights = "); WriteAll(NeuralNetwork.outputWeights, writer, true);

            writer.WriteLine("[biases]");
            writer.Write("Mbias = "); WriteMatrix(writer, NeuralNetwork.Mbias);
            writer.Write("Obias = "); WriteAll(NeuralNetwork.Obias, writer, true);
        }

        public static void SaveRogerToJson()
        {
            string fileName;
            int index = 0;

            do
            {
                if (index == 0)
                    fileName = $"roger.json";
                else
                    fileName = $"roger{index}.json";
                index++;
            }
            while (File.Exists(fileName));

            Roger roger = new()
            {
                AIversion = Parameters.version,

                InputNeurons = BuildStringArray(NeuralNetwork.inputNeurons),
                MiddleNeurons = BuildStringMatrix(NeuralNetwork.middleNeurons),
                OutputNeurons = BuildStringArray(NeuralNetwork.outputNeurons),

                inputWeights = BuildStringArray(NeuralNetwork.inputWeights),
                middleWeights = BuildStringArray(NeuralNetwork.middleWeights),
                outputWeights = BuildStringArray(NeuralNetwork.outputWeights),

                Mbias = BuildStringMatrix(NeuralNetwork.Mbias),
                Obias = BuildStringArray(NeuralNetwork.Obias),
            };

            string jsonData = JsonSerializer.Serialize(roger, new JsonSerializerOptions { WriteIndented = true });

            using StreamWriter writer = new(fileName);
            writer.Write(jsonData);
        }

        private static void WriteAll(dynamic array, StreamWriter writer, bool line_break = false)
        {
            foreach (var element in array)
                writer.Write(element.ToString(CultureInfo.InvariantCulture) + ";");

            if (line_break == true)
                writer.WriteLine();
        }
        private static void WriteMatrix(StreamWriter writer, double[,] matrix)
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

        private static void WriteArray(StreamWriter writer, double[] array)
        {
            foreach (double v in array)
                writer.Write(v.ToString(CultureInfo.InvariantCulture) + ';');
            //writer.WriteLine();
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

        /// <summary>
        /// Этот метод определяет тип и парсит файл Роджера и предоставляет объект в котором все данные лежат в виде строк, их придётся приводить в нужные типы данных с помощью соответствующих функций которые вроде есть в этом классе
        /// </summary>
        /// <returns>Объект класса Roger</returns>
        public static Roger LoadRoger()
        {
            if (!File.Exists(Parameters.roger2))
                UI.Send("Roger file not found", "error");
            else // Сделал else чтобы при отсутствии файла код дальше не выполнялся
            {
                switch (CheckFormat())
                {
                    case true: // Json
                        return LoadRogerFromJson();

                    case false: // Roger 
                        return LoadRogerFromRoger();
                }
            }
            return null; // Заглушка чтобы компилятор не ругался, я хз как это исправлять. Я могу это с помощью GOTO исправить, но блин...
        }

        /// <summary>
        /// Проверка формата записи
        /// </summary>
        /// <returns>true - json формат, false - roger формат/returns>
        private static bool CheckFormat()
        {
            if (Parameters.roger2.EndsWith(".json"))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Класс который будет хранить в себе данные для загрузки/сохранения нейросети.
        /// Данные хранятся в виде строк, поэтому их придётся конвертировать с помощью соответствующих методов (вроде все методы для этого написаны)
        /// For Axolotl: Если каких-то данных не хватает, просто допиши их в класс 
        /// </summary>
        public class Roger
        {
            public string AIversion { get; set; }

            public string InputNeurons { get; set; }
            public string MiddleNeurons { get; set; }
            public string OutputNeurons { get; set; }

            public string InputWeights { get; set; }
            public string MiddleWeights { get; set; }
            public string OutputWeights { get; set; }

            public string Mbias { get; set; }
            public string Obias { get; set; }
        }


        /// <summary>
        /// Функция которая возвращает объект класса Roger, с данными извлечёнными из файла формата .roger
        /// </summary>
        /// <returns>Объект класса Roger</returns>
        private static Roger LoadRogerFromRoger()
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile(Parameters.roger2);

            Roger roger = new()
            {
                AIversion = data["roger"]["AIversion"],

                InputNeurons = data["neurons"]["inputNeurons"],
                MiddleNeurons = data["neurons"]["middleNeurons"],
                OutputNeurons = data["neurons"]["outputNeurons"],

                InputWeights = data["weights"]["inputWeights"],
                MiddleWeights = data["weights"]["middleWeights"],
                OutputWeights = data["weights"]["outputWeights"],

                Mbias = data["biases"]["Mbias"],
                Obias = data["biases"]["Obias"]
            };

            return roger;
        }

        /// <summary>
        /// Примечание: Проверку на json это или нет надо делать заранее, эта функция подразумевает что Paramaters.roger2 это json файл!!!
        /// </summary>
        /// <returns>
        /// Возвращает обьект класса Roger со всеми нужными данными для загрузки нейросети
        /// </returns>
        private static Roger LoadRogerFromJson()
        {
            using JsonDocument document = JsonDocument.Parse(Parameters.roger2);
            JsonElement root = document.RootElement;

            Roger roger = new()
            {
                AIversion = root.GetProperty("AIversion").GetString(),

                InputNeurons = root.GetProperty("inputNeurons").GetString(),
                MiddleNeurons = root.GetProperty("middleNeurons").GetString(),
                OutputNeurons = root.GetProperty("outputNeurons").GetString(),

                InputWeights = root.GetProperty("inputWeights").GetString(),
                MiddleWeights = root.GetProperty("middleWeights").GetString(),
                OutputWeights = root.GetProperty("outputWeights").GetString(),

                Mbias = root.GetProperty("Mbias").GetString(),
                Obias = root.GetProperty("Obias").GetString()
            };

            return roger;
        }


        public static dynamic[,] ReadMatrixFromArray(dynamic[] obj)
        {
            byte rows = 3;
            byte columns = 2;

            if (obj.Length < rows * columns)
            {
                throw new ArgumentException("В исходном массиве недостаточно элементов для заполнения матрицы 3х2.");
            }

            dynamic[,] matrix = new dynamic[rows, columns];
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
    }

}
