using System.Text;
using System.Globalization;

namespace Yocto_Roger
/* 
Yocto Roger ;)
*****************
*Emotion Corp ;)*
*****************
Copyright 2025-2026 Emotion Corp.
    RogerHub UI part
*/

{
    public class UI
    {
        static void Main()
        {
            Console.WriteLine("Configuring console...");
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;
            DrawLine(ConsoleColor.Magenta, "Emotion ;) 2026", "Yocto Roger :D");
            Thread.Sleep(3000);
            int i = 0;
            while (true)
            {
                Console.Clear();
                if (Parameters.isDebug == false)
                    DrawLine(ConsoleColor.DarkMagenta, $"Welcome to the RogerHub! v.{Parameters.version}{Parameters.revision} DEV2", DateTime.Now.Date.ToString("dd/MM/yyyy"));
                else
                    DrawLine(ConsoleColor.DarkMagenta, $"Welcome to the RogerHub! v.{Parameters.version}{Parameters.revision} DEV2 DEBUG MODE", DateTime.Now.Date.ToString("dd/MM/yyyy"));
                Send("This project is still in the development stage.", "warning");
                Send("This is a BETA build. Some functionality may not work. Have fun testing :D", "warning");
                Console.Write("""
                    
                    1. Start Roger in training mode
                    2. Start Roger from the .roger or .json file
                    3. Options for training mode
                    4. RRNNs settings
                    5. Exit from RogerHub 
                    >>> 
                    """);
                if (int.TryParse(Console.ReadLine(), out int value))
                {
                    switch (value)
                    {
                        case 1:
                            Console.WriteLine("Starting Roger...");
                            NeuralNetwork.StartAI(0);
                            break;

                        case 2:
                            Console.Clear();
                            Console.Write("Enter the name of your .roger or .json file. If it's not in this directory, please write absolute path to it\n>>> ");
                            Parameters.roger2 = Console.ReadLine();

                            Auxiliary.InitRogersData(IO.LoadRoger());

                            NeuralNetwork.StartAI(1);
                            break;

                        case 3:
                            SetUp();
                            break;

                        case 4:
                            Send("This function isn't ready :(", "error");
                            break;

                        case 5:
                            Environment.Exit(0);
                            break;

                        default:
                            switch (i++)
                            {
                                case 0:
                                    Console.WriteLine("What?");
                                    break;
                                case 1:
                                    Console.WriteLine("I dont understand... (0-0)");
                                    break;
                                case 2:
                                    Console.WriteLine("Again?");
                                    break;
                                case 3:
                                    Console.WriteLine("PLEASE STOP!!!");
                                    break;
                                case 4:
                                    Console.WriteLine("I'm going disconnect...");
                                    Thread.Sleep(300);
                                    Console.WriteLine("Bye ;(");
                                    Thread.Sleep(1000);
                                    Environment.Exit(0);
                                    break;
                            }
                            break;
                    }
                }
                else
                    Send("Incorrect input >:(", "error");
            }
        }

        /// <summary>
        /// Считает кол-во строк в переданном файле
        /// </summary>
        /// <param name="path">Абсолютный путь к файлу</param>
        /// <returns>Целое число, значение которого является количеством прочитанных строк</returns>
        public static int CountLines(string path)
        {
            int count = 0;
            using var reader = new StreamReader(path);
            while (reader.ReadLine() != null)
                count++;
            return count;
        }

        /// <summary>
        /// Рисует полоску определенного цвета внизу окна консоли
        /// </summary>
        /// <param name="color">Цвет текста</param>
        /// <param name="leftText">Текст слева</param>
        /// <param name="rightText">Текст справа</param>
        public static void DrawLine(ConsoleColor color, string leftText = "", string rightText = "")
        {
            int cursorX = Console.CursorLeft;
            int cursorY = Console.CursorTop;

            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.BackgroundColor = color;

            Console.Write(new string(' ', Console.WindowWidth));

            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.Write(leftText);

            Console.SetCursorPosition((Console.WindowWidth - rightText.Length - 1), Console.WindowHeight - 1);
            Console.Write(rightText);

            Console.SetCursorPosition(cursorX, cursorY);
            Console.BackgroundColor = ConsoleColor.Black;
        }

        /// <summary>
        /// Вызов меню настройки значений а также сохранения файла
        /// </summary>
        public static void SetUp()
        {
            int i = 0;
            while (i == 0)
            {
                Console.Clear();
                Console.Write($"""
                                        RogerHub Training Options
                                            
                                        0. Save your roger settings in the file 

                                        1. Count of input neurons...{Parameters.inputNeuronsCount}
                                        2. Count of middle neurons (all middle layers)...{Parameters.middleNeuronsCount}
                                        3. Count of output neurons...{Parameters.outputNeuronsCount}
                                        4. Count of Layers...{Parameters.layers}
                                        5. Knowledge file...{Parameters.knowledgeFile}
                                        6. DropOut sys percent...{Parameters.DropOutPercent}% (0% - disable DropOut)
                                        7. Learning Rate...{Parameters.learningRate}
                                        8. Passes...{Parameters.passes}
                                        9. Exit 
                                        >>> 
                                        """);
                string choice = Console.ReadLine();
                switch (choice)
                {

                    case "0":
                        Console.Write("""                           
                            How do you want to save roger?

                            1. INI
                            2. Json (recommended)

                            >>>
                            """);
                        if (int.TryParse(Console.ReadLine(), out int userInputChecked))
                        {
                            switch (userInputChecked)
                            {
                                case 1:
                                    IO.SaveRoger();
                                    break;

                                case 2:
                                    IO.SaveRogerToJson();
                                    break;

                                default:
                                    Send("What?", "error");
                                    break;
                            }

                            Console.WriteLine("Your roger saved in this directory, let's go, check it!\n If file was not created, write it in issues on our GitHub please ;)");
                        }

                        break;
                    case "1":
                        Console.Clear();
                        Console.WriteLine("*INPUT NEURONS PARAMETER*");
                        Console.Write("INT32> Enter new count of input neurons (> 0)...");
                        if (int.TryParse(Console.ReadLine(), out int userInputChecked1))
                        {
                            if (userInputChecked1 > 0)
                                Parameters.inputNeuronsCount = userInputChecked1;
                            else
                                Send("Value out of range.", "error");
                        }
                        break;

                    case "2":
                        Console.Clear();
                        Console.WriteLine("*MIDDLE NEURONS PARAMETER*");
                        Console.Write("INT32> Enter new count of middle neurons (> 0)...");
                        if (int.TryParse(Console.ReadLine(), out int userInputChecked2))
                        {
                            if (userInputChecked2 > 0)
                                Parameters.middleNeuronsCount = userInputChecked2;
                            else
                                Send("Value out of range.", "error");
                        }
                        break;

                    case "3":
                        Console.Clear();
                        Console.WriteLine("*OUTPUT NEURONS PARAMETER*");
                        Console.Write("INT32> Enter new count of output neurons (> 0)...");
                        if (int.TryParse(Console.ReadLine(), out int userInputChecked3))
                        {
                            if (userInputChecked3 > 0)
                                Parameters.outputNeuronsCount = userInputChecked3;
                            else
                                Send("Value out of range.", "error");
                        }
                        break;

                    case "4":
                        Console.Clear();
                        Console.WriteLine("*LAYERS PARAMETER*");
                        Console.Write("INT32> Enter new count of layers (> 2)...");
                        if (int.TryParse(Console.ReadLine(), out int layersCount))
                        {
                            if (layersCount > 2)
                            {
                                Parameters.layers = layersCount;
                                Parameters.Mlayers = layersCount - 2;
                            }
                            else
                                Send("Value out of range.", "error");
                        }
                        break;

                    case "5":
                        Console.Clear();
                        Console.WriteLine("*KNOWLEDGE PARAMETER*");
                        Console.Write("STRING> Enter new knowledge file...");
                        string file = Console.ReadLine();
                        if (File.Exists(file))
                            Parameters.knowledgeFile = file;
                        else
                            Send("Knowledge file doesn't exists", "error");
                        break;

                    case "6":
                        Console.Clear();
                        Console.WriteLine("*DROPOUT PERCENT PARAMETER*");
                        Console.Write("FLOAT> Enter new DropOut percent (0–70)... ");
                        if (int.TryParse(Console.ReadLine(), out int newDrop))
                        {
                            if (newDrop >= 0 && newDrop <= 70)
                                Parameters.DropOutPercent = newDrop;
                            else
                                Send("Value out of range.", "error");
                        }
                        else
                            Send("Invalid input.", "error");
                        break;

                    case "7":
                        Console.Clear();
                        Console.WriteLine("*LEARNING RATE PARAMETER*");
                        Console.Write("FLOAT> Enter new learning rate (0,0 – 1,0)... ");
                        if (float.TryParse(Console.ReadLine(), out float newLR))
                        {
                            if (newLR > 0 && newLR <= 1.0)
                                Parameters.learningRate = newLR;
                            else
                                Send("Learning rate out of range.", "error");
                        }
                        else
                            Send("Invalid input.", "error");
                        break;

                    case "8":
                        Console.Clear();
                        Console.WriteLine("*PASSES PARAMETER*");
                        Console.Write("INT32> Enter count of passes (> 0)... ");
                        if (int.TryParse(Console.ReadLine(), out int newPasses))
                        {
                            if (newPasses > 0)
                                Parameters.passes = newPasses;
                            else
                                Send("Passes must be greater than zero.", "error");
                        }
                        else
                            Send("Invalid input.", "error");
                        break;

                    case "9":
                        i++;
                        break;
                }
            }
        }

        /// <summary>
        /// Рисует красивое сообщение пользователю о чём либо
        /// 
        /// Если ввести неправильный мод, пользователь получит ошибку о некорректно введённом моде. Замена исключениям
        /// </summary>
        /// <param name="message">Текст сообщения</param>
        /// <param name="mode">Мод в зависимости от которого будет зависеть цвет и смысл сообщения. Доступные моды: "error", "warning", "message". По умолчанию мод является message</param>
        public static void Send(string message, string mode = "message")
        {
            switch (mode.ToLower())
            {
                case "error":
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERROR>" + message);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                    break;

                case "warning":
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("WARNING>" + message);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;

                case "message":
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(message + "\n");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;

                default:
                    Send("UI.Send>Incorrect mode! Check the UI.Send method call", "error");
                    break;
            }
        }
    }
}
