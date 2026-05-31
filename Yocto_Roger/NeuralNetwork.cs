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
        public static bool rogerIsCreated = false;

        public static double[,] educationArray;

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
                        UI.Send("I can't find the training file! Maybe you didn't specify it in the settings, please check it before write an issue on our github ", "error");
                        break;
                    }
                    Console.Write("SetUp education array and reading knowledge...");
                    string[] allLines = File.ReadAllLines(Parameters.knowledgeFile);

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
                        UI.Send("NeuralNetwork.StartAI.InputNeurons>The training file doesn't match your neural network! (need value " + input.Length + " for Count of Input neurons)", "error");
                        Console.WriteLine("Do you want to change this parameter <Parameters.inputNeuronsCount> and restart NeuralNetwork? (y/n)");
                        ConsoleKeyInfo answer = Console.ReadKey();
                        switch (answer.KeyChar)
                        {
                            case 'y':
                                Parameters.inputNeuronsCount = input.Length;
                                Console.Clear();
                                StartAI(0);
                                break;
                        }
                        break;
                    }
                    else if (output.Length != Parameters.outputNeuronsCount)
                    {
                        Console.WriteLine();
                        UI.Send("NeuralNetwork.StartAI.OutputNeurons>The training file doesn't match your neural network! (need value " + output.Length + " for Count of Output neurons)", "error");
                        Console.WriteLine("Do you want to change this parameter <Parameters.outputNeuronsCount> and restart NeuralNetwork? (y/n)");
                        ConsoleKeyInfo answer = Console.ReadKey();
                        switch (answer.KeyChar)
                        {
                            case 'y':
                                Parameters.outputNeuronsCount = output.Length;
                                Console.Clear();
                                StartAI(0);
                                break;
                        }
                        break;
                    }

                    Console.CursorVisible = false;
                    UI.Send("Everything is ready to create Roger!");

                    educationArray = new double[allLines.Length, length];

                    for (int i = 0; i < allLines.Length; i++)
                    {
                        parsedString = allLines[i].Split(' ');
                        input = AIMath.StringParse(parsedString[0], ',');
                        splitingSecond = parsedString[1].Split(';');
                        for (int j = 0; j < input.Length; j++)
                            educationArray[i, j] = input[j];
                        for (int j = 0; j < splitingSecond.Length; j++)
                            output[j] = Convert.ToDouble(splitingSecond[j], CultureInfo.InvariantCulture);
                        for (int j = 0; j < splitingSecond.Length; j++)
                            educationArray[i, j + input.Length] = output[j];
                    }

                    for (int i = 0; i < educationArray.GetLength(0); i++)
                    {
                        for (int j = 0; j < educationArray.GetLength(1); j++)
                            Console.Write(educationArray[i, j] + " ");
                        Console.WriteLine();
                    }

                    UI.Send("done");
                    Console.Write("Initialization RogerHubEngine...");
                    inputNeurons = new int[Parameters.inputNeuronsCount];
                    middleNeurons = new double[Parameters.Mlayers, Parameters.middleNeuronsCount];
                    outputNeurons = new double[Parameters.outputNeuronsCount];
                    inputWeights = new double[Parameters.inputNeuronsCount, Parameters.middleNeuronsCount];
                    middleWeights = new double[Parameters.Mlayers - 1][,];
                    outputWeights = new double[Parameters.middleNeuronsCount, Parameters.outputNeuronsCount];
                    Mbias = new double[Parameters.Mlayers, Parameters.middleNeuronsCount];
                    Obias = new double[Parameters.outputNeuronsCount];
                    UI.Send("done");
                    Console.Write("Initialization biases...");
                    Biases.Init(ref Mbias);
                    Biases.Init(ref Obias);
                    UI.Send("done");
                    Console.Write("Initialization weights...");
                    Weights.Init(ref inputWeights);
                    Weights.Init(ref outputWeights);
                    Weights.Init(ref middleWeights);
                    UI.Send("done");
                    UI.Send("Initialization complete", "message");
                    Console.Write("Education...");
                    UI.DrawLine(ConsoleColor.DarkRed, "Creating your Roger, please wait :D", DateTime.Now.Date.ToString("dd/MM/yyyy"));
                    Console.WriteLine();
                    Progressbar educationStatus = new(ConsoleColor.DarkGreen, 20, Console.CursorLeft, Console.CursorTop);

                    Training.Education(ref inputNeurons, ref middleNeurons, ref outputNeurons, ref inputWeights, ref middleWeights, ref outputWeights, ref Mbias, ref Obias, educationArray, educationStatus);

                    educationStatus.Draw(100);
                    UI.Send("\nEducation Complete");

                    Console.Write("Finishing...");
                    rogerIsCreated = true;
                    UI.Send("done");
                    Console.WriteLine("Hello! I'm Roger, the neuron network from Emotion!");
                    Thread.Sleep(3000);
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
                Console.CursorVisible = true;
                while (true)
                {
                    Console.Clear();
                    UI.Send("Enter \"save\" to fix the state of neural network in the file, for load at this point later", "warning");
                    Console.WriteLine($"Roger have {Parameters.inputNeuronsCount} input neurons, and {Parameters.outputNeuronsCount} output neurons." +
                        $"Write input format: <datain1>,<datain2>,<datain3>...");
                    UI.DrawLine(ConsoleColor.DarkGreen, "Welcome to Yocto Roger v2.2! Manual interface", DateTime.Now.Date.ToString("dd/MM/yyyy"));
                    Console.Write("\nInput>>>");
                    string? userInputString = Console.ReadLine();
                    if (!string.IsNullOrEmpty(userInputString))
                    {
                        if (userInputString != "save")
                        {
                            string[] userInputChecked = userInputString.Split(',');
                            if (userInputChecked.Length == Parameters.inputNeuronsCount)
                            {
                                int[] userInput = new int[Parameters.inputNeuronsCount];
                                for (int i = 0; i < userInput.Length; i++)
                                    userInput[i] = Convert.ToInt32(userInputChecked[i]);
                                ForwardPropagation(userInput, inputNeurons, inputWeights, middleNeurons, middleWeights, Mbias, outputNeurons, Obias, outputWeights, disabledDropOut);
                                Console.Write("Output>>>");
                                for (int i = 0; i < outputNeurons.Length; i++)
                                    Console.Write(outputNeurons[i] + " ");
                                Console.WriteLine("Press any key to continue");
                                Console.ReadKey();
                            }
                        }
                        else
                        {
                            Console.Write("Please, enter the path, where we going to save the file (to this directory, simple press the enter): ");
                            string input = Console.ReadLine() ?? string.Empty;

                            if (input is string path && !string.IsNullOrEmpty(path))
                                IO.SaveNeuralNetworkStateToJson(IO.FixTheStateOfNeuralNetwork(false), path);

                            else if (input == string.Empty)
                                IO.SaveNeuralNetworkStateToJson(IO.FixTheStateOfNeuralNetwork(false), Directory.GetCurrentDirectory());
                            else
                                UI.Send("Incorrect input (-_0)", "error");
                        }
                    }
                    else
                    {
                        UI.Send("Incorrect input (-_0)", "error");
                    }
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

        public static void ForwardPropagation(int[] NNinput, int[] inputNeurons, double[,] inputWeights, double[,] middleNeurons, double[][,] middleWeights, double[,] middleBiases,
            double[] outputNeurons, double[] outputBiases, double[,] outputWeights, float[,]? dropOutMasks)
        {
            WriteToNN(inputNeurons, NNinput);

            SumWeights(inputWeights, inputNeurons, middleNeurons, middleBiases);

            if (dropOutMasks != null)
                for (int i = 0; i < middleNeurons.GetLength(1); i++)
                    middleNeurons[0, i] *= dropOutMasks[0, i];

            for (int l = 0; l < Parameters.Mlayers - 1; l++)
            {
                SumWeights(middleWeights[l], middleNeurons, middleNeurons, middleBiases, l);

                if (dropOutMasks != null)
                    for (int i = 0; i < middleNeurons.GetLength(1); i++)
                        middleNeurons[l + 1, i] *= dropOutMasks[l + 1, i];
            }

            SumWeights(outputWeights, middleNeurons, outputNeurons, outputBiases);
        }
    }
}
