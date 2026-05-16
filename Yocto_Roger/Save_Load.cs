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

            Roger roger = new Roger
            {
                AIversion = Parameters.version,

                inputNeurons = BuildStringArray(NeuralNetwork.inputNeurons),
                middleNeurons = BuildStringMatrix(NeuralNetwork.middleNeurons),
                outputNeurons = BuildStringArray(NeuralNetwork.outputNeurons),

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

            return builder.ToString().Remove(builder.Length - 1);
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

            return builder.ToString().Remove(builder.Length - 1); // Удаляет последний ненужный символ ';'
        }

        public static void LoadRoger() // TODO: Сделать загрузку, с новых правил записи (а точнее обрабатывать числа разделённые точками с запятой)
        {
            if (!File.Exists(Parameters.roger2))
                UI.Send("Roger file not found", "error");
        }

        private class Roger // Я сделал без под-секций, ибо они не нужны, без них удообнее и писать меньше в целом
        { 
            public string AIversion { get; set;  }

            public string inputNeurons { get; set; }
            public string middleNeurons { get; set; }
            public string outputNeurons { get; set; }

            public string Mbias { get; set; }
            public string Obias { get; set; }
        }
    }

}
