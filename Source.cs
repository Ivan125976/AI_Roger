using System;
using System.ComponentModel.Design;
using System.Data;
using System.IO;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Transactions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Quecto_Roger_v._2._0
    // Quecto Roger 2.0 ;)
    //
    //***************
     //*Emotion Corp.*
    // ***************
    // Квекто-нейросеть!

    // Многослойный Перцептрон
    // 14 - вход
    // 16 - середина
    // 8 - выход
    //тут был Аксолотль
    //и Грибочек
    //и Хамелеон
    //и Семечка

{
    internal class Program
    {
        static bool isDebug = false;
        static int passes = 500;
        static double learningRate = 0.02;
        static int DropOutPercent = 20;
        static string knowledgeFile = "knowledge.know";
        static int[,] educationArray;
        static int middleNeuronsCount = 16;

        static double[] inputNeurons = new double[14];
        static double[] middleNeurons = new double[middleNeuronsCount];
        static double[] outputNeurons = new double[8];

        static double[,] weights1 = new double[inputNeurons.Length, middleNeurons.Length];
        static double[,] weights2 = new double[middleNeurons.Length, outputNeurons.Length];

        static double[] bias1 = new double[middleNeurons.Length];
        static double[] bias2 = new double[outputNeurons.Length];

        static Random rand = new Random();

        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            sendMessage(ConsoleColor.Magenta, "Emotion :) 2025    Quecto Roger v.2.0");
            Thread.Sleep(3000);
            sendMessage(ConsoleColor.DarkRed, "Configuring console...");
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
            if (isDebug == false)
                sendMessage(ConsoleColor.DarkMagenta, "Welcome to the RogerHub! v.2.0.7");
            else
                sendMessage(ConsoleColor.DarkYellow, "Welcome to the RogerHub! v.2.0.7 DEBUG MODE");
            Console.Write(" 1. Start Roger \n 2. Set Up Roger \n 3. Cake\n >>>");
            int userInput = Convert.ToInt32(Console.ReadLine());
            switch (userInput)
            {
                case 1:
                    Console.WriteLine("Starting Roger...");
                    break;

                case 2:
                    setUp();
                    break;

                case 3:
                    Console.WriteLine("The cake is lie! Starting Roger...");
                    break;

                default:
                    Console.WriteLine("I'm not understand... and started Roger!");
                    break;
            }
            Console.WriteLine("SetUp education array...");
            educationArray = new int[CountLines(knowledgeFile), 3];
            Console.WriteLine("Read knowledge...");
            writeEducationArray(ref educationArray, knowledgeFile);
            Console.WriteLine("Set up Weights... (1/2)");
            setUpWeights(ref weights1);
            Console.WriteLine("Set up Weights... (2/2)");
            setUpWeights(ref weights2);
            Console.WriteLine("Set up biases... (1/2)");
            setUpBiases(ref bias1);
            Console.WriteLine("Set up biases... (2/2)");
            setUpBiases(ref bias2);
            Console.WriteLine("Education. This make takes few minutes.");
            sendMessage(ConsoleColor.DarkRed, "Education Roger... This may take a few minutes.");
            educationWithTeacher();
            Console.WriteLine("Hello! I'm Roger, the AI from Emotion!");
            while (true)
            {
                sendMessage(ConsoleColor.DarkGreen, "Ready.");
                WriteInput(ref inputNeurons);
                sendMessage(ConsoleColor.DarkRed, "Calculation neurons... (1/2)");
                sumWeights(ref weights1, ref inputNeurons, ref middleNeurons, bias1);
                sendMessage(ConsoleColor.DarkRed, "Calculation neurons... (2/2)");
                sumWeights(ref weights2, ref middleNeurons, ref outputNeurons, bias2);
                sendMessage(ConsoleColor.DarkRed, "Rounding...");
                Rounding(ref outputNeurons);
                sendMessage(ConsoleColor.DarkRed, "Almost ready...");
                Console.WriteLine($"I think it's {writeOutput(outputNeurons)}");
                Console.WriteLine("Press any key to continue...");
                sendMessage(ConsoleColor.Magenta, "Waiting.");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void writeEducationArray(ref int[,] education, string path) //записывание данных из файла в переменную
        {
            var lines = File.ReadAllLines(path);

            for (int row = 0; row < lines.Length; row++)
            {
                string line = lines[row].Replace("-", "").Trim();
                var parts = line.Split(' ');

                for (int col = 0; col < 3; col++)
                    education[row, col] = int.Parse(parts[col]);
            }
        }

        static int CountLines(string path)
        {
            int count = 0;
            using var reader = new StreamReader(path);
            while (reader.ReadLine() != null)
                count++;
            return count;
        }

        static void educationWithTeacher() //обучение с поддержкой DropOut и deadParts
        {
            double[] errorOut = new double[outputNeurons.Length];
            double[] errorMid = new double[middleNeurons.Length];
            double[] deltaMid = new double[middleNeurons.Length];
            double[] deltaOut = new double[outputNeurons.Length];
            double[,] oldWeights = new double[weights2.GetLength(0),weights2.GetLength(1)]; 

            for (int z = 0; z < passes; z++)
            { 
                for (int i = 0; i < educationArray.GetLength(0); i++)
                {

                    Array.Clear(errorMid, 0, errorMid.Length);
                    Array.Clear(errorOut, 0, errorOut.Length);
                    Array.Clear(deltaMid, 0, deltaMid.Length);
                    Array.Clear(deltaOut, 0, deltaOut.Length);

                    for (int x = 0; x < weights2.GetLength(0); x++)
                        for (int y = 0; y < weights2.GetLength(1); y++)
                            oldWeights[x,y] = weights2[x, y];

                    int[] binary = new int[8];
                    for (int j = 0; j < 8; j++)
                    {
                        int mask = 1 << (7 - j);
                        binary[j] = (educationArray[i,2] & mask) != 0 ? 1 : 0;
                    }

                    WriteInput(ref inputNeurons, educationArray[i, 0], educationArray[i, 1]);
                    sumWeights(ref weights1, ref inputNeurons, ref middleNeurons, bias1);
                    sumWeights(ref weights2, ref middleNeurons, ref outputNeurons, bias2);

                    for (int j = 0; j < outputNeurons.Length; j++)
                    {
                        errorOut[j] = outputNeurons[j] - binary[j]; //ошибка
                        deltaOut[j] = errorOut[j] * outputNeurons[j] * (1 - outputNeurons[j]); //дельта

                        for (int k = 0; k < middleNeurons.Length; k++)
                            weights2[k, j] -= middleNeurons[k] * deltaOut[j] * learningRate;

                        bias2[j] -= deltaOut[j] * learningRate;
                    }
                    for (int j = 0; j < middleNeurons.Length; j++)
                    {
                        for (int l = 0; l < outputNeurons.Length; l++)
                        {
                            errorMid[j] += deltaOut[l] * oldWeights[j, l]; //ошибка

                        }
                        deltaMid[j] = errorMid[j] * middleNeurons[j] * (1 - middleNeurons[j]); //дельта
                        for (int k = 0; k < inputNeurons.Length; k++)
                        {
                            weights1[k, j] -= inputNeurons[k] * deltaMid[j] * learningRate;
                        }
                        bias1[j] -= deltaMid[j] * learningRate;
                    }
                }
            }
        }

        static void Rounding(ref double[] neurons)
        {
            for (int i = 0; i < neurons.Length; i++)
                neurons[i] = (int)Math.Round(neurons[i]);
        }

        static int[] WriteInput(ref double[] inNeurons, int? v1 = null, int? v2 = null) //конвертация десятичного числа в двиучное
        {
            int[] values = new int[2];

            if (v1.HasValue && v2.HasValue)
            {
                values[0] = v1.Value;
                values[1] = v2.Value;
            }
            else
            {
                Console.Write("Enter first value -> ");
                string input = Console.ReadLine();

                if(int.TryParse(input, out int correctInput))
                    values[0] = correctInput;
                else
                    Console.WriteLine("Incorrect input!");

                Console.Write("Enter second value -> ");
                input = Console.ReadLine();
                if (int.TryParse(input, out int correctInput2))
                    values[1] = correctInput2;
                else
                    Console.WriteLine("Incorrect input!");
            }
            if (isDebug)
                Console.Write("Recorded in the initial neurons - " );
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    int mask = 1 << (6 - j);
                    inNeurons[i * 7 + j] = (values[i] & mask) != 0 ? 1.0 : 0.0;
                    if (isDebug)
                        Console.Write(inNeurons[i * 7 + j]);
                }
            }
            
            return values;
        }



        static int writeOutput(double[] binary) //двиучное в десятичное
        {
            int result = 0;
            for (int i = 0; i < binary.Length; i++)
                if (binary[i] != 0.0)
                    result += 1 << (7 - i);
            return result;
        }


        static void sumWeights(ref double[,] oldweights, ref double[] oldNeurons, ref double[] newNeurons, double[] biases) //нахождение новых нейронов
        {
            if (isDebug)
                Console.Write("Sum of weights - ");
            for (int i = 0; i < newNeurons.Length; i++)
            {
                double temp = 0;
                for (int j = 0; j < oldNeurons.Length; j++)
                {
                    temp += oldweights[j, i] * oldNeurons[j];
                }
                temp += biases[i];
                newNeurons[i] = sigmoida(temp);
                if (isDebug)
                    Console.Write(newNeurons[i] + " ");
            }
            if (isDebug)
                Console.WriteLine();
        }

        static double sigmoida(double value) //активация
        {
            double answer = 1.0 / (1.0 + Math.Exp(-value));
            if (isDebug)
                Console.WriteLine("Sigmoida> " + answer);
            return answer;
        }

        static void setUpBiases(ref double[] biases) //рандомное заполнение массива сдвигов
        {
            for (int i = 0; i < biases.Length;i++)
            {
                biases[i] = rand.NextDouble() * 0.2 - 0.1;
                biases[i] = Math.Clamp(biases[i], -3.0, 3.0);
            }
            if (isDebug)
                Console.WriteLine("The biases have been successfully adjusted!");
        }

        static void setUpWeights(ref double[,] weights)
        {
            for (int  i = 0; i < weights.GetLength(0); i++)
            {
                for (int j = 0; j < weights.GetLength(1); j++)
                {
                    weights[i,j] = rand.NextDouble() * 0.2 - 0.1;
                    weights[i, j] = Math.Clamp(weights[i, j], -3.0, 3.0);
                }
            }
            if (isDebug)
                Console.WriteLine("The weights have been successfully adjusted!");
        }

        static void sendMessage(ConsoleColor color, string message)
        {
            int cursorX = Console.CursorLeft;
            int cursorY = Console.CursorTop;

            Console.SetCursorPosition(0,Console.WindowHeight - 1);
            Console.BackgroundColor = color;

            for(int i = 0; i < Console.WindowWidth;  i++)
                Console.Write(" ");

            Console.SetCursorPosition(0,Console.WindowHeight - 1);
            Console.Write(message);
            Console.SetCursorPosition(cursorX, cursorY);
            Console.BackgroundColor = ConsoleColor.Black;
        }

        static void setUp()
        {
            Console.Clear();

            string userInput = "";
            int i = 0;
            while (i == 0)
            {
                Console.Write($"RogerHub Options \n 1. Number of middle neurons...{middleNeuronsCount} \n 2. Knowledge file...{knowledgeFile} \n 3. DropOut sys percent...{DropOutPercent}% \n 4. Learning Rate...{learningRate} \n 5. Passes...{passes} \n 6. Exit \n >>>");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        Console.WriteLine("*MIDDLE NEURONS PARAMETER*");
                        Console.Write("INT16> Enter new middle neurons number (> 0)...");
                        userInput = Console.ReadLine();
                        if (int.TryParse(userInput, out int userInputChecked))
                        {
                            if (userInputChecked > 0)
                                middleNeuronsCount = userInputChecked;
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Value out of range.");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                        }
                        break;

                    case "2":
                        Console.Clear();
                        Console.WriteLine("*KNOWLEDGE PARAMETER*");
                        Console.Write("STRING> Enter new knowledge file...");
                        knowledgeFile = Console.ReadLine();
                        break;

                    case "3":
                        Console.Clear();
                        Console.WriteLine("*DROPOUT PERCENT PARAMETER*");
                        Console.Write("INT16> Enter new DropOut percent (0–70)... ");
                        userInput = Console.ReadLine();
                        if (int.TryParse(userInput, out int newDrop))
                        {
                            if (newDrop >= 0 && newDrop <= 70)
                                DropOutPercent = newDrop;
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine("Value out of range.");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Invalid input.");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        break;

                    case "4":
                        Console.Clear();
                        Console.WriteLine("*LEARNING RATE PARAMETER*");
                        Console.Write("INT32> Enter new learning rate (0.001 – 1.0)... ");
                        userInput = Console.ReadLine();
                        if (double.TryParse(userInput, out double newLR))
                        {
                            if (newLR > 0 && newLR <= 1.0)
                                learningRate = newLR;
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine("Learning rate out of range.");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Invalid input.");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        break;

                    case "5":
                        Console.Clear();
                        Console.WriteLine("*PASSES PARAMETER*");
                        Console.Write("INT16> Enter passes count (> 0)... ");
                        userInput = Console.ReadLine();
                        if (int.TryParse(userInput, out int newPasses))
                        {
                            if (newPasses > 0)
                                passes = newPasses;
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine("Passes must be greater than zero.");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Invalid input.");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        break;

                    case "6":
                        i++;
                        break;
                }
            }
        }
    }
}
