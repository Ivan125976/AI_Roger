using System.Globalization;

namespace Yocto_Roger
{
    /* 
Yocto Roger ;)
*****************
*Emotion Corp ;)*
*****************
Copyright 2025-2026 Emotion Corp.
    Yocto Roger 
*/
    internal class NeuralNetwork
    {
        static bool rogerIsCreated = false;

        public static string[,] educationArray;

        public static int[] inputNeurons;
        public static double[,] middleNeurons;
        public static double[] outputNeurons;

        public static double[,] inputWeights;
        public static double[][,] middleWeights;
        public static double[,] outputWeights;

        public static double[,] Mbias;
        public static double[] Obias;
        public static void StartAI(int mode)
        {
            Console.WriteLine("StartAI in mode " + mode);
            switch (mode)
            {
                case 0:
                    if (!File.Exists(Parameters.knowledgeFile))
                    {
                        UI.Send("I can't find the training file!", "error");
                        break;
                    }
                    Console.Write("SetUp education array and reading knowledge...");
                    string[] allLines = File.ReadAllLines(Parameters.knowledgeFile);

                    //инициализация массива обучения

                    string[] parsedString = allLines[0].Split(' ');
                    int[] input = AIMath.StringParse(parsedString[0], ',');
                    string[] splitingSecond = parsedString[1].Split(';');
                    double[] output = new double[splitingSecond.Length];
                    for (int j = 0; j < splitingSecond.Length; j++)
                        output[j] = Convert.ToDouble(splitingSecond[j], CultureInfo.InvariantCulture);
                    int length = input.Length + output.Length;

                    if (input.Length != Parameters.inputNeuronsCount)
                    {
                        Console.WriteLine();
                        UI.Send("NeuralNetwork.StartAI.InputNeurons>The training file doesn't match your neural network! Please reconfigure it in the settings menu (need value " + input.Length + ")", "error");
                        break;
                    }
                    else if (output.Length != Parameters.outputNeuronsCount)
                    {
                        Console.WriteLine();
                        UI.Send("NeuralNetwork.StartAI.OutputNeurons>The training file doesn't match your neural network! Please reconfigure it in the settings menu (need value " + output.Length + ")", "error");
                        break;
                    }

                    educationArray = new string[allLines.Length, length];

                    for (int i = 0; i < allLines.Length; i++)
                    {
                        parsedString = allLines[i].Split(' ');
                        input = AIMath.StringParse(parsedString[0], ',');
                        splitingSecond = parsedString[1].Split(';');
                        for (int j = 0; j < input.Length; j++)
                            educationArray[i, j] = Convert.ToString(input[j]);
                        for (int j = 0; j < splitingSecond.Length; j++)
                            output[j] = Convert.ToDouble(splitingSecond[j], CultureInfo.InvariantCulture);
                        for (int j = 0; j < splitingSecond.Length; j++)
                            educationArray[i, j + input.Length] = Convert.ToString(output[j]);
                    }

                    UI.Send("done", "message");
                    Console.Write("Initialization RogerHubEngine...");
                    inputNeurons = new int[Parameters.inputNeuronsCount];
                    middleNeurons = new double[Parameters.Mlayers, Parameters.middleNeuronsCount];
                    outputNeurons = new double[Parameters.outputNeuronsCount];
                    inputWeights = new double[Parameters.inputNeuronsCount, Parameters.middleNeuronsCount];
                    middleWeights = new double[Parameters.Mlayers - 1][,];
                    outputWeights = new double[Parameters.middleNeuronsCount, Parameters.outputNeuronsCount];
                    Mbias = new double[Parameters.Mlayers, Parameters.middleNeuronsCount];
                    Obias = new double[Parameters.outputNeuronsCount];
                    UI.Send("done", "message");
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
                    Console.Write("Education...");
                    UI.DrawLine(ConsoleColor.DarkRed, "Creating your Roger, please wait :D");

                    Training.Education(ref inputNeurons, ref middleNeurons, ref outputNeurons, ref inputWeights, ref middleWeights, ref outputWeights, ref Mbias, ref Obias, educationArray);
                    UI.Send("done", "message");

                    Console.Write("Cleaning...");
                    educationArray = null;
                    rogerIsCreated = true;
                    UI.Send("done", "message");
                    break;

                case 1:
                    Console.Write("Loading your Roger...");
                    IO.LoadRoger();
                    break;
            }
            if (rogerIsCreated)
            {
                float[,]? disabledDropOut = null;

                //Console.TreatControlCAsInput = true; // Блокирование закрытия программы по нажатия ctrl+c ибо нужно чтобы оно выходило из цикла, а не из программы
                //TODO: Сделать выход из training mode в главное меню, по нажатию CTRL + C асинхронно, чтобы проверка была не в конкретном куске кода, а в любой момент
                //UI.Send("Press CTRL + C to quit outta here", "warning");
                Console.WriteLine("Hello! I'm Roger, the neuron network from Emotion!");
                while (true)
                {
                    UI.DrawLine(ConsoleColor.DarkGreen, "Not-ready AI Interface v2.2");
                        Console.Write("\nInput>>>");
                        int[] userInput = AIMath.NumToBin(Convert.ToInt32(Console.ReadLine()), inputNeurons.Length);
                        ForwardPropagation(userInput, inputNeurons, inputWeights, middleNeurons, middleWeights, Mbias, outputNeurons, Obias, outputWeights, disabledDropOut);
                        Console.Write("Output>>>");
                        for (int i = 0; i < outputNeurons.Length; i++)
                            Console.Write(outputNeurons[i] + " ");
                }
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
                        if (AIMath.rand.NextDouble() < Parameters.DropOutPercent / 100.0)
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
            for (int i = 0; i < newNeurons.GetLength(1); i++)
            {
                double temp = 0;
                for (int j = 0; j < oldNeurons.Length; j++)
                    temp += oldweights[j, i] * oldNeurons[j];
                temp += biases[0, i];
                newNeurons[0, i] = AIMath.Tanh(temp);
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
            for (int i = 0; i < newNeurons.GetLength(1); i++)
            {
                double temp = 0;
                for (int j = 0; j < oldNeurons.GetLength(1); j++)
                    temp += oldweights[j, i] * oldNeurons[layer, j];
                temp += biases[layer + 1, i];
                newNeurons[layer + 1, i] = AIMath.Tanh(temp);
                if (Parameters.isDebug)
                    Console.Write(newNeurons[layer + 1, i] + " ");
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
                for (int j = 0; j < oldNeurons.GetLength(1); j++)
                    temp += oldweights[j, i] * oldNeurons[oldNeurons.GetLength(0) - 1, j];
                temp += biases[i];
                newNeurons[i] = AIMath.Tanh(temp);
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

        public static void ForwardPropagation(int[] NNinput,int[] inputNeurons,double[,] inputWeights,double[,] middleNeurons,double[][,] middleWeights,double[,] middleBiases,
            double[] outputNeurons,double[] outputBiases,double[,] outputWeights,float[,]? dropOutMasks)
        {
            WriteToNN(inputNeurons, NNinput);

            SumWeights(inputWeights, inputNeurons, middleNeurons, middleBiases);

            for (int l = 0; l < Parameters.Mlayers - 1; l++)
            {
                SumWeights(middleWeights[l], middleNeurons, middleNeurons, middleBiases, l);

                if (dropOutMasks != null)
                    for (int i = 0; i < middleNeurons.GetLength(1); i++)
                        middleNeurons[l + 1, i] *= dropOutMasks[l, i];
            }

            SumWeights(outputWeights, middleNeurons, outputNeurons, outputBiases);
        }
    }
}
