using System.Globalization;

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
                fileName = $"roger{index}.roger2";
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
            writer.Write("Mbias = "); WriteAll(NeuralNetwork.Mbias, writer, true);
            writer.Write("Obias = "); WriteAll(NeuralNetwork.Obias, writer, true);
        }


        private static void WriteAll(dynamic array, StreamWriter writer, bool line_break = false)
        {
                foreach (var element in array)
                {
                    writer.Write(element + ";");
                }
                if (line_break == true)
                    writer.WriteLine();
        }
        private static void WriteMatrix(StreamWriter writer, double[,] matrix)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                for (int i = 0; i < matrix.GetLength(0); i++)
                    writer.Write(matrix[i, j].ToString(CultureInfo.InvariantCulture) + "/ ");
                writer.WriteLine();
            }
        }

        private static void WriteArray(StreamWriter writer, double[] array)
        {
            foreach (double v in array)
                writer.Write(v.ToString(CultureInfo.InvariantCulture) + "/ ");
            writer.WriteLine();
        }

        public static void LoadRoger() // TODO: Сделать загрузку, с новых правил записи (а точнее обрабатывать числа разделённые точками с запятой)
        {                                               
            if (!File.Exists(Parameters.roger2))
                UI.SendError("Roger file not found");
        }
    }
}
