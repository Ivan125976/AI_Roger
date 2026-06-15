using MemoryPack;
using System.Globalization;
using Yocto_Roger.IO;
using Yocto_Roger.RogerCore.Initialization.Weights;
using Yocto_Roger.RogerCore.UtilityTools;
using Yocto_Roger.UI.CUI;
using Yocto_Roger.UI.Interfaces;
using static Yocto_Roger.IO.Splitter;
using static Yocto_Roger.RogerCore.UtilityTools.RogerMath;
using static Yocto_Roger.UI.CUI.CUI;

// При компиляции в Release дллка NewtonsoftJson.dll всё равно почему-то линкуется в папку с бинарником, вероятно изза того что он добавлен в проект как nuget пакет. Так вот, в release когда компилируешь, дллку эту можно удалить, ибо она не нужна и весит 700кб целых
#if DEBUG
using Newtonsoft.Json; // For middleWeights
#endif

namespace Yocto_Roger.RogerCore
{
    /* 
Yocto Roger ;)
*****************
*Emotion Corp ;)*
*****************
Copyright 2025-2026 Emotion Corp.
    Yocto Roger 
*/

    /// <summary>
    /// Yocto Roger Neural Network. Hello! :D
    /// </summary>

    public class NeuralNetwork(Parameters param, MainIO io, Training.Training training, NeuralNetworkInterface neuralNetworkInterface, MainMenuInterface mainMenu)
    {
        private readonly Parameters _param = param;
        private readonly MainIO _io = io;
        private readonly Training.Training _training = training;
        private readonly NeuralNetworkInterface _neuralNetworkInterface = neuralNetworkInterface;
        private readonly MainMenuInterface _mainMenu = mainMenu;
        /// <summary>
        /// Flag indicating whether Roger has been created
        /// </summary>
        public bool rogerIsCreated = false;

        /// <summary>
        /// An array containing training data
        /// </summary>
        public double[,]? educationArray;

        /// <summary>
        /// Array of input neurons
        /// </summary>
        public int[]? inputNeurons;

        /// <summary>
        /// Array of middle neurons
        /// </summary>
        public double[,]? middleNeurons;

        /// <summary>
        /// Array of output neurons
        /// </summary>
        public double[]? outputNeurons;

        /// <summary>
        /// Array of input weights (I->M)
        /// </summary>
        public double[,]? inputWeights;

        /// <summary>
        /// Array of middle weights (M->M)
        /// </summary>
        public double[][,]? middleWeights;

        /// <summary>
        /// Array of output weights (M->O)
        /// </summary>
        public double[,]? outputWeights;

        /// <summary>
        /// Array of middle biases
        /// </summary>
        public double[,]? Mbias;

        /// <summary>
        /// Array of output biases
        /// </summary>
        public double[]? Obias;

        /// <summary>
        /// Method that launches a roger
        /// </summary>
        /// <param name="mode">When 1 -> the neural network tries to load, when 0 -> a new neural network is created</param>
        public void StartAI(int mode)
        {
            Console.WriteLine("StartAI in mode " + mode);
            switch (mode)
            {
                case 0:
                    if (!File.Exists(_param.knowledgeFile))
                    {
                        Send("I can't find the training file!", MessageType.error);
                        break;
                    }
                    Console.Write("SetUp education array and reading knowledge...");

                    // In the future, these variables can't be null, because if they don't receive a value, the while(true) method is called, which eliminates the use of null values.
                    string[] allLines = null!;
                    string[] parsedString = null!;
                    int[] input = null!;
                    string[] splitingSecond = null!;
                    double[] output = null!;
                    int length = 0!;

                    try
                    {
                        allLines = File.ReadAllLines(_param.knowledgeFile);

                        parsedString = allLines[0].Split(' ');
                        input = StringParse(parsedString[0], ',');
                        splitingSecond = parsedString[1].Split(';');
                        output = new double[splitingSecond.Length];
                        for (int j = 0; j < splitingSecond.Length; j++)
                            output[j] = Convert.ToDouble(splitingSecond[j], CultureInfo.InvariantCulture);
                        length = input.Length + output.Length;
                    }
                    catch (Exception ex)
                    {
                        Send("Invalid knowledge file. Check if this is it, or check if its contents are correct", MessageType.error);
                        Console.WriteLine("Here's exception, if you are programmer, you can make a pull request on our github, with fix this exception, or write Issue with this problem, and add to it this exception please\nGithub: https://github.com/Ivan125976/AI_Roger/tree/v2.2-beta\n Exception:\n" + ex.Message);
                        Console.Write("Press enter to continue", ConsoleColor.Blue);
                        Console.ReadLine();
                        _mainMenu.StartInterface();
                    }


                    if (input!.Length != _param.inputNeuronsCount)
                    {
                        Console.WriteLine();
                        Send("NeuralNetwork.StartAI.InputNeurons>The training file doesn't match your neural network! (need value " + input.Length + " for Count of Input neurons)", MessageType.error);
                        Console.WriteLine("Do you want to change this parameter <Parameters.inputNeuronsCount> and restart NeuralNetwork? (y/n)");
                        ConsoleKeyInfo answer = Console.ReadKey();
                        switch (answer.KeyChar)
                        {
                            case 'y':
                                _param.inputNeuronsCount = input.Length;
                                Console.Clear();
                                StartAI(0);
                                break;
                        }
                        break;
                    }
                    else if (output!.Length != _param.outputNeuronsCount)
                    {
                        Console.WriteLine();
                        Send("NeuralNetwork.StartAI.OutputNeurons>The training file doesn't match your neural network! (need value " + output.Length + " for Count of Output neurons)", MessageType.error);
                        Console.WriteLine("Do you want to change this parameter <Parameters.outputNeuronsCount> and restart NeuralNetwork? (y/n)");
                        ConsoleKeyInfo answer = Console.ReadKey();
                        switch (answer.KeyChar)
                        {
                            case 'y':
                                _param.outputNeuronsCount = output.Length;
                                Console.Clear();
                                StartAI(0);
                                break;
                        }
                        break;
                    }

                    Console.CursorVisible = false;
                    Send("Everything is ready to create Roger!");

                    educationArray = new double[allLines!.Length, length];

                    for (int i = 0; i < allLines.Length; i++)
                    {
                        parsedString = allLines[i].Split(' ');
                        input = StringParse(parsedString[0], ',');
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

                    Send("done");
                    Console.Write("Initialization RogerHubEngine...");
                    inputNeurons = new int[_param.inputNeuronsCount];
                    middleNeurons = new double[_param.layers - 2, _param.middleNeuronsCount];
                    outputNeurons = new double[_param.outputNeuronsCount];
                    inputWeights = new double[_param.inputNeuronsCount, _param.middleNeuronsCount];
                    middleWeights = new double[_param.layers - 3][,];
                    outputWeights = new double[_param.middleNeuronsCount, _param.outputNeuronsCount];
                    Mbias = new double[_param.layers - 2, _param.middleNeuronsCount];
                    Obias = new double[_param.outputNeuronsCount];
                    Send("done");
                    Console.Write("Initialization weights...");
                    InitWeights.Init(inputWeights);
                    CreateWeights.CreateMiddleWeights(middleWeights, _param.middleNeuronsCount);
                    InitWeights.Init(middleWeights);
                    InitWeights.Init(outputWeights);
                    Send("done");
                    Send("Initialization complete", MessageType.message);
                    Console.Write("Education...");
                    DrawLine(ConsoleColor.DarkRed, "Creating your Roger, please wait :D", DateTime.Now.Date.ToString("dd/MM/yyyy"));
                    Console.WriteLine();
                    Progressbar educationStatus = new(ConsoleColor.DarkGreen, 20, Console.CursorLeft, Console.CursorTop);

                    _training.Education(inputNeurons, middleNeurons, outputNeurons, inputWeights, middleWeights, outputWeights, Mbias, Obias, educationArray, educationStatus);

                    educationStatus.Draw(100);
                    Send("\nEducation Complete");

                    Console.Write("Finishing...");
                    rogerIsCreated = true;
                    Send("done");
                    Console.WriteLine("Hello! I'm Roger, the neuron network from Emotion!");
                    Thread.Sleep(3000);
                    break;

                case 1:
                    Console.Write("Write an absolute path to your .bin file\nSTRING> ");
                    string? userInput = Console.ReadLine();
                    if (userInput is string inputChecked && !string.IsNullOrEmpty(userInput) && Path.Exists(userInput))
                    {
                        Console.WriteLine("Loading your Roger... please wait :D");
                        try
                        {
                            _io.InitNeuralNetwork(MainIO.LoadNeuralNetworkStateFromBin(inputChecked));
                            rogerIsCreated = true;
                        }
                        catch (MemoryPackSerializationException e)
                        {
                            Send("I can't to serialize the data, here's my error: \n", MessageType.error);
                            Console.WriteLine(e.Message, ConsoleColor.Red);
                            Console.WriteLine("This could mean that the developers screwed up somewhere. If you have a time, then please write an issue about this error on our Github (0v0). Here's url:\n" +
                                "https://github.com/Ivan125976/AI_Roger/issues/new", ConsoleColor.Blue);
                            Console.Write("Press enter to continue");
                            Console.ReadLine();
                        }
                    }
                    else
                    {
                        Send("Incorrect input (-_0)", MessageType.error);
                        Send("Maybe file which you entered, doesn't exists, please check it and retry");
                    }
                    break;

                case 3:
                    return;
            }

            if (rogerIsCreated)
                _neuralNetworkInterface.StartInterface();
        }

        /// <summary>
        /// Generates a DropOut subsystem table
        /// </summary>
        /// <returns></returns>
        public float[,] GenerateDropOut()
        {
#if DEBUG
            Console.WriteLine("DropOut Matrix = ");
#endif
            float[,] masks = new float[_param.layers - 2, _param.middleNeuronsCount];
            float keepProb = 1.00f - _param.DropOutPercent * 0.01f;

            if (_param.DropOutPercent == 0)
            {
                for (int i = 0; i < masks.GetLength(0); i++)
                {
                    for (int j = 0; j < masks.GetLength(1); j++)
                    {
                        masks[i, j] = 1.0f;
#if DEBUG
                        Console.Write(masks[i, j] + " ");
#endif
                    }
#if DEBUG
                    Console.WriteLine();
#endif
                }
                return masks;
            }
            else
            {
                for (int i = 0; i < masks.GetLength(0); i++)
                {
                    for (int j = 0; j < masks.GetLength(1); j++)
                    {
                        if (RogerMath.rand.NextDouble() < _param.DropOutPercent / 100.0)
                            masks[i, j] = 0;
                        else
                            masks[i, j] = 1.0f / keepProb;
#if DEBUG
                        Console.Write(masks[i, j] + " ");
#endif
                    }
#if DEBUG
                    Console.WriteLine();
#endif
                }
            }
            return masks;
        }

        /// <summary>
        /// Sum weights (I->M)
        /// </summary>
        /// <param name="oldweights">Input weights</param>
        /// <param name="oldNeurons">Input neurons</param>
        /// <param name="newNeurons">Middle neurons</param>
        /// <param name="biases">Middle biases</param>
        public static void SumWeights(double[,] oldweights, int[] oldNeurons, double[,] newNeurons, double[,] biases)
        {

#if DEBUG
            Console.Write("Sum of weights ([]->[,]) - ");
#endif
            for (int i = 0; i < newNeurons.GetLength(1); i++)
            {
                double temp = 0;
                for (int j = 0; j < oldNeurons.Length; j++)
                    temp += oldweights[j, i] * oldNeurons[j];
                temp += biases[0, i];
                newNeurons[0, i] = Tanh(temp);
#if DEBUG
                Console.Write(newNeurons[0, i] + " ");
#endif
            }
#if DEBUG
            Console.WriteLine();
#endif
        }

        /// <summary>
        /// Sum weights (M->M)
        /// </summary>
        /// <param name="oldweights">Middle weights</param>
        /// <param name="neurons">Middle neurons</param>
        /// <param name="biases">Middle biases</param>
        /// <param name="layer">Layer</param>
        public static void SumWeights(double[,] oldweights, double[,] neurons, double[,] biases, int layer) //нахождение новых нейронов (middle -> middle)
        {

#if DEBUG
            Console.Write("Sum of weights ([,]->[,]) - ");
#endif
            for (int i = 0; i < neurons.GetLength(1); i++)
            {
                double temp = 0;
                for (int j = 0; j < neurons.GetLength(1); j++)
                    temp += oldweights[j, i] * neurons[layer, j];
                temp += biases[layer + 1, i];
                neurons[layer + 1, i] = Tanh(temp);
#if DEBUG
                Console.Write(neurons[layer + 1, i] + " ");
#endif
            }
#if DEBUG
            Console.WriteLine();
#endif
        }

        /// <summary>
        /// Sum weights (M->O)
        /// </summary>
        /// <param name="oldweights">Middle weights</param>
        /// <param name="oldNeurons">Middle neurons</param>
        /// <param name="newNeurons">Output neurons</param>
        /// <param name="biases">Output biases</param>
        public static void SumWeights(double[,] oldweights, double[,] oldNeurons, double[] newNeurons, double[] biases) //нахождение новых нейронов (middle -> output)
        {
#if DEBUG
            Console.Write("Sum of weights ([,]->[]) - ");
#endif
            for (int i = 0; i < newNeurons.GetLength(0); i++)
            {
                double temp = 0;
                for (int j = 0; j < oldNeurons.GetLength(1); j++)
                    temp += oldweights[j, i] * oldNeurons[oldNeurons.GetLength(0) - 1, j];
                temp += biases[i];
                newNeurons[i] = Tanh(temp);
#if DEBUG
                Console.Write(newNeurons[i] + " ");
#endif
            }
#if DEBUG
            Console.WriteLine();
#endif
        }

        /// <summary>
        /// Write array to input neurons
        /// </summary>
        /// <param name="neurons"></param>
        /// <param name="writeArray"></param>
        public static void WriteToNN(int[] neurons, int[] writeArray)
        {
            if (neurons.Length == writeArray.Length)
                for (int i = 0; i < neurons.Length; i++)
                    neurons[i] = writeArray[i];
            else
                Send("NeuralNetwork.WriteToNN>The size of the neuron array and the data array do not match, it is impossible to write data", MessageType.error);
        }

        /// <summary>
        /// Forward propogation algorithm
        /// </summary>
        /// <param name="NNinput">Current input to the neural network</param>
        /// <param name="inputNeurons">Input neurons array</param>
        /// <param name="inputWeights">Input weights array</param>
        /// <param name="middleNeurons">Middle neurons array</param>
        /// <param name="middleWeights">Middle weights array</param>
        /// <param name="middleBiases">Middle biases array</param>
        /// <param name="outputNeurons">Output neurons array</param>
        /// <param name="outputBiases">Output biases array</param>
        /// <param name="outputWeights">Output weights array</param>
        public void ForwardPropagation(int[] NNinput, int[] inputNeurons, double[,] inputWeights, double[,] middleNeurons, double[][,] middleWeights, double[,] middleBiases,
            double[] outputNeurons, double[] outputBiases, double[,] outputWeights)
        {
            WriteToNN(inputNeurons, NNinput);

            SumWeights(inputWeights, inputNeurons, middleNeurons, middleBiases);

            for (int l = 0; l < _param.layers - 3; l++)
                SumWeights(middleWeights[l], middleNeurons, middleBiases, l);

            SumWeights(outputWeights, middleNeurons, outputNeurons, outputBiases);
        }

        /// <summary>
        /// Forward propogation algorithm for Training Mode
        /// </summary>
        /// <param name="NNinput">Current input to the neural network</param>
        /// <param name="inputNeurons">Input neurons array</param>
        /// <param name="inputWeights">Input weights array</param>
        /// <param name="middleNeurons">Middle neurons array</param>
        /// <param name="middleWeights">Middle weights array</param>
        /// <param name="middleBiases">Middle biases array</param>
        /// <param name="outputNeurons">Output neurons array</param>
        /// <param name="outputBiases">Output biases array</param>
        /// <param name="outputWeights">Output weights array</param>
        /// <param name="dropOutMatrix">DropOut Matrix</param>
        public void ForwardPropagation(int[] NNinput, int[] inputNeurons, double[,] inputWeights, double[,] middleNeurons, double[][,] middleWeights, double[,] middleBiases,
            double[] outputNeurons, double[] outputBiases, double[,] outputWeights, float[,] dropOutMatrix)
        {
            WriteToNN(inputNeurons, NNinput);

            SumWeights(inputWeights, inputNeurons, middleNeurons, middleBiases);
            for (int i = 0; i < middleNeurons.GetLength(1); i++)
                middleNeurons[0, i] *= dropOutMatrix[0, i];

            for (int l = 0; l < _param.layers - 3; l++)
            {
                SumWeights(middleWeights[l], middleNeurons, middleBiases, l);
                for (int j = 0; j < middleNeurons.GetLength(1); j++)
                    middleNeurons[l + 1, j] *= dropOutMatrix[l + 1, j];
            }

            SumWeights(outputWeights, middleNeurons, outputNeurons, outputBiases);
        }
    }
}
