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
    /// <summary>
    /// Internal library for a beautiful command-line interface
    /// </summary>
    public class UI
    {
        static void Main()
        {
            Console.WriteLine("Configuring console...");
            Console.Title = "Welcome to Beta!";
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;
            DrawLine(ConsoleColor.Magenta, "Emotion ;) 2026", "Roger :D");
            Thread.Sleep(3000);
            int i = 0;
            while (true)
            {
                Console.Clear();
                if (Parameters.isDebug == false)
                    DrawLine(ConsoleColor.DarkMagenta, $"Welcome to the RogerHubEngine! v.{Parameters.version}{Parameters.revision} BETA", DateTime.Now.Date.ToString("dd/MM/yyyy"));
                else
                    DrawLine(ConsoleColor.DarkMagenta, $"Welcome to the RogerHubEngine! v.{Parameters.version}{Parameters.revision} BETA DEBUG MODE", DateTime.Now.Date.ToString("dd/MM/yyyy"));
                Send("This project is still in the development stage.", "warning");
                Send("This is a BETA build. Some functionality may not work. Have fun testing :D", "warning");
                Console.Write("""
                    
                    1. Start Roger in training mode
                    2. Load your roger setting from the file
                    3. Options for training mode...
                    4. RRNNs settings...
                    5. About...
                    6. Exit from RogerHub 
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
                            Console.Write("Write a path to your .roger or .json file\nSTRING> ");
                            if (Console.ReadLine() is string inputChecked && !string.IsNullOrEmpty(Console.ReadLine()))
                            {
                                Parameters.roger2 = inputChecked;
                                Auxiliary.InitRogersData(IO.LoadRoger());
                            }
                            else
                                Send("Incorrect input (-_0)", "error");
                            break;

                        case 3:
                            SetUp();
                            break;

                        case 4:
                            Send("This page isn't ready RRNNs.RRNNs", "error");
                            break;

                        case 5:
                            Console.WriteLine($" Authors: \n Axolotl512 - AI and RogerHubEngine \n d3ath_script - RRNNs, IO and compiling \n\n RogerHubEngine v{Parameters.version}{Parameters.revision} build:DEV2 \n" +
                                " RogerCore v2.2 \n RRNNs isn't ready \n OpenRB isn't ready \n\n Press any key to continue ");
                            Console.ReadKey();
                            break;

                        case 6:
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
        /// Draws a stripe of a specified color at the bottom of the console window.
        /// </summary>
        /// <param name="color">Background text color</param>
        /// <param name="leftText">Left text</param>
        /// <param name="rightText">Right text</param>
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
        /// Calling up the menu for setting values ​​and saving the file
        /// </summary>
        public static void SetUp()
        {
            int i = 0;
            while (i == 0)
            {
                Console.Clear();
                Console.Write($"""
                                        RogerHubEngine Training Options
                                            
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
                string? choice = Console.ReadLine();
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

                            Console.WriteLine("Your roger saved in this directory, let's go, check it!\n If file was not created, write it in issues on our GitHub please ;)" +
                                "\n Press any key to continue");
                            Console.ReadKey();
                        }
                        else
                        {
                            Send("Unknown input", "error");
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
                        string? file = Console.ReadLine();
                            if (File.Exists(file))
                                Parameters.knowledgeFile = file;
                            else
                                Send("I couldn't find such a file :(", "error");
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
        /// Draws a beautiful message to the user about something
        /// </summary>
        /// <param name="message">Message text</param>
        /// <param name="mode">The color and meaning of the message will depend on the mode. Available modes are "error," "warning," and "message." The default mode is "message."</param>
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

    class Progressbar(ConsoleColor color, int parts) : UI
    {
        private readonly int _parts = parts;
        private readonly ConsoleColor _color = color;

        public void Draw(int percent)
        {
            Console.Write("[");
            Console.ForegroundColor = _color;
            Console.Write(new string('█', percent * _parts / 100));
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(new string('█', _parts - (percent * _parts / 100)));
            Console.Write("]");
        }
    }
}
