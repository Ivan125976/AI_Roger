namespace Yocto_Roger
{
    internal class NeuralNetwork
    {
        public static string[,]? educationArray;

        public static int[] inputNeurons = new int[Parameters.inputNeuronsCount];
        public static double[,] middleNeurons = new double[Parameters.Mlayers, Parameters.middleNeuronsCount];
        public static double[] outputNeurons = new double[Parameters.outputNeuronsCount];

        public static double[,] inputWeights = new double[inputNeurons.Length, middleNeurons.Length];
        public static double[][,] middleWeights = new double[Parameters.Mlayers - 1][,];
        public static double[,] outputWeights = new double[middleNeurons.Length, outputNeurons.Length];

        public static double[,] Mbias = new double[Parameters.Mlayers, middleNeurons.Length];
        public static double[] Obias = new double[outputNeurons.Length];

        public static void StartAI(int mode)
        {
            Console.WriteLine("StartAI in mode " + mode);
            switch (mode)
            {
                case 0:
                    Console.Write("Initialization biases...");
                    Biases.Init(ref Mbias);
                    Biases.Init(ref Obias);
                    UI.Send("done", "message");
                    Console.Write("Initialization weights...");
                    Weights.Init(ref inputWeights);
                    Weights.Init(ref outputWeights);
                    Weights.Init(ref middleWeights);
                    UI.Send("done", "message");
                    UI.Send("Initialization complete", "message");
                    Console.Write("SetUp education array...");
                    //d3ath_script: сделай инициализацию массива обучения, в зависимости от созданной нейросети, тоесть в зависимости от размера входных нейронов и выходных. Придумай свою логику, только шоб работало)
                    UI.Send("done", "message");
                    Console.Write("Read knowledge...");
                    //и еще тут запись в массив обучения из файла, json, .roger2, какой надо. Сделай только чтобы он поддерживал .roger2, надо обратную поддержку оставить с версии 2.1 на 2.2
                    UI.Send("done", "message");
                    Console.Write("Education...");
                    UI.DrawLine(ConsoleColor.DarkRed, "Creating your Roger, please wait :D");
                    Training.Education(ref inputNeurons, ref middleNeurons, ref outputNeurons, ref inputWeights, ref middleWeights, ref outputWeights, ref Mbias, ref Obias, educationArray);
                    UI.Send("done", "message");
                    Console.Write("Cleaning...");
                    //и тут после этого еще чистку массива до нуля, или если можно вообще его удаление. Давай, полагаюсь на тебя)))
                    UI.Send("done", "message");
                    break;

                case 1:
                    Console.Write("Loading your Roger...");
                    Save_Load.LoadRoger();
                    break;
            }
            Console.WriteLine("Hello! I'm Roger, the neuron network from Emotion!");
            while (true)
            {
                UI.DrawLine(ConsoleColor.DarkGreen, "Not-ready AI Interface v2.2");
                int[] userInput = new int[inputNeurons.Length];
                Console.Write("\nInput>>>");
                userInput = AIMath.NumToBin(Convert.ToInt32(Console.ReadLine()), inputNeurons.Length);
                ForwardPropagation(userInput, inputNeurons, inputWeights, middleNeurons, middleWeights, Mbias, outputNeurons, Obias, outputWeights, GenerateDropOut());
                Console.Write("Output>>>");
                for (int i = 0; i < outputNeurons.Length; i++)
                    Console.Write(outputNeurons[i] + " ");
            }
        }

        public static float[,] GenerateDropOut()
        {
            if (Parameters.isDebug)
                Console.WriteLine("DropOut Matrix = ");
            float[,] masks = new float[Parameters.Mlayers, Parameters.middleNeuronsCount];
            float keepProb = 1.00f - (Parameters.DropOutPercent * 0.01f);

            if (Parameters.DropOutPercent == 0)
            {
                for (int i = 0; i < masks.GetLength(0); i++)
                {
                    for (int j = 0; j < masks.GetLength(1); j++)
                    {
                        masks[i, j] = 1.0f;
                        if (Parameters.isDebug)
                            Console.Write(masks[i, j] + " ");
                    }
                    if (Parameters.isDebug)
                        Console.WriteLine();
                }
                return masks;
            }
            else
            {
                for (int i = 0; i < masks.GetLength(0); i++)
                {
                    for (int j = 0; j < masks.GetLength(1); j++)
                    {
                        if (AIMath.rand.Next(0, 101) < Parameters.DropOutPercent)
                            masks[i, j] = 0;
                        else
                            masks[i, j] = 1.0f / keepProb;
                        if (Parameters.isDebug)
                            Console.Write(masks[i, j] + " ");
                    }
                    if (Parameters.isDebug)
                        Console.WriteLine();
                }
            }
            return masks;
        }
        public static void SumWeights(double[,] oldweights, int[] oldNeurons, double[,] newNeurons, double[,] biases) //нахождение новых нейронов (input -> middle)
        {
            if (Parameters.isDebug)
                Console.Write("Sum of weights ([]->[,]) - ");
            for (int i = 0; i < newNeurons.GetLength(0); i++)
            {
                double temp = 0;
                for (int j = 0; j < oldNeurons.Length; j++)
                    temp += oldweights[j, i] * oldNeurons[j];
                temp += biases[0, i];
                newNeurons[0, i] = AIMath.Sigmoida(temp);
                if (Parameters.isDebug)
                    Console.Write(newNeurons[0, i] + " ");
            }
            if (Parameters.isDebug)
                Console.WriteLine();
        }
        public static void SumWeights(double[,] oldweights, double[,] oldNeurons, double[,] newNeurons, double[,] biases, int layer) //нахождение новых нейронов (middle -> middle)
        {
            if (Parameters.isDebug)
                Console.Write("Sum of weights ([,]->[,]) - ");
            for (int i = 0; i < newNeurons.GetLength(0); i++)
            {
                double temp = 0;
                for (int j = 0; j < oldNeurons.GetLength(0); j++)
                    temp += oldweights[j, i] * oldNeurons[layer, j];
                temp += biases[layer, i];
                newNeurons[layer, i] = AIMath.Sigmoida(temp);
                if (Parameters.isDebug)
                    Console.Write(newNeurons[layer, i] + " ");
            }
            if (Parameters.isDebug)
                Console.WriteLine();
        }
        public static void SumWeights(double[,] oldweights, double[,] oldNeurons, double[] newNeurons, double[] biases) //нахождение новых нейронов (middle -> output)
        {
            if (Parameters.isDebug)
                Console.Write("Sum of weights ([,]->[]) - ");
            for (int i = 0; i < newNeurons.GetLength(0); i++)
            {
                double temp = 0;
                for (int j = 0; j < oldNeurons.GetLength(0); j++)
                    temp += oldweights[j, i] * oldNeurons[oldNeurons.GetLength(0) - 1, j];
                temp += biases[i];
                newNeurons[i] = AIMath.Sigmoida(temp);
                if (Parameters.isDebug)
                    Console.Write(newNeurons[i] + " ");
            }
            if (Parameters.isDebug)
                Console.WriteLine();
        }

        public static void WriteToNN(int[] neurons, int[] writeArray)
        {
            if (neurons.Length == writeArray.Length)
                for (int i = 0; i < neurons.Length; i++)
                    neurons[i] = writeArray[i];
            else
                UI.Send("NeuralNetwork.WriteToNN>The size of the neuron array and the data array do not match, it is impossible to write data", "error");
        }

        public static void ForwardPropagation(int[] NNinput, int[] inputNeurons, double[,] inputWeights, double[,] middleNeurons, double[][,] middleWeights,
            double[,] middleBiases, double[] outputNeurons, double[] outputBiases, double[,] outputWeights, float[,] dropOut)
        {
            WriteToNN(inputNeurons, NNinput);
            SumWeights(inputWeights, inputNeurons, middleNeurons, middleBiases);
            for (int l = 0; l < Parameters.Mlayers; l++) //DropOut 
            {
                for (int k = 0; k < Parameters.middleNeuronsCount; k++)
                    middleNeurons[l, k] *= dropOut[l, k];
                SumWeights(middleWeights[l], middleNeurons, middleNeurons, middleBiases, l);
            }
            SumWeights(outputWeights, middleNeurons, outputNeurons, outputBiases);
        }
    }
}
