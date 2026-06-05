using System.Text;
using Yocto_Roger.Yocto_Roger;

namespace Yocto_Roger.UI
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
    /// Internal library for a beautiful command line
    /// </summary>
    public class UI(NeuralNetwork nN, SettingsInterface settingsInterface, Parameters param)
    {
        /// <summary>
        /// Link to neural network file
        /// </summary>
        public NeuralNetwork _roger = nN;

        /// <summary>
        /// Link to SettingsInterface
        /// </summary>
        public SettingsInterface _settingsInterface = settingsInterface;

        private readonly Parameters _param = param;

        /// <summary>
        /// Launches the console UI
        /// </summary>
        public void StartEngine()
        {
            Console.WriteLine("Configuring console...");

            if (Console.WindowHeight < 20 || Console.WindowWidth < 50)
            {
                Send("The window is too small >:(", "error");
                Environment.Exit(1);
            }

            Console.Title = "RogerHubEngine v2.2.0";

            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;

            DrawLine(ConsoleColor.Magenta, "Emotion ;) 2026", "Roger :D");
            Thread.Sleep(3000);
            int i = 0;
            while (true)
            {
                Console.Clear();
                if (_param.isDebug == false)
                    DrawLine(ConsoleColor.DarkMagenta, $"Welcome to the RogerHubEngine! v.{_param.version} BETA", DateTime.Now.Date.ToString("dd/MM/yyyy"));
                else
                    DrawLine(ConsoleColor.DarkMagenta, $"Welcome to the RogerHubEngine! v.{_param.version}.{_param.revision} BETA DEBUG MODE", DateTime.Now.Date.ToString("dd/MM/yyyy"));
                Send("This project is still in the development stage.", "warning");
                Send("This is a BETA build. Some functionality may not work. Have fun testing :D", "warning");
                Console.Write("""
                    
                    1. Start Roger in training mode
                    2. Load your roger (neural network) from the file
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
                            _roger.StartAI(0);
                            break;

                        case 2:
                            _roger.StartAI(1);
                            break;

                        case 3:
                            _settingsInterface.SetUpMenu();
                            break;

                        case 4:
                            Send("RRNNs.RRNNs>This page isn't ready", "error");
                            break;

                        case 5:
                            Console.WriteLine($" Github: https://github.com/Ivan125976/AI_Roger\n\n Authors: \n Axolotl512 - AI and RogerHubEngine \n d3ath-script - RRNNs, IO and compiling \n\n RogerHubEngine v{_param.version}.{_param.revision} build:BETA \n" +
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
        /// Draws a stripe of a specified color at the bottom of the console window with auto-text color.
        /// </summary>
        /// <param name="color">Background text color</param>
        /// <param name="leftText">Left text</param>
        /// <param name="rightText">Right text</param>
        public static void DrawLine(ConsoleColor color, string leftText = "", string rightText = "")
        {
            Console.ForegroundColor = color switch
            {
                ConsoleColor.Gray or ConsoleColor.White or ConsoleColor.Yellow or ConsoleColor.DarkYellow or ConsoleColor.Cyan or ConsoleColor.Green or ConsoleColor.DarkGreen => ConsoleColor.Black,
                _ => ConsoleColor.White,
            };
            int cursorX = Console.CursorLeft;
            int cursorY = Console.CursorTop;

            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.BackgroundColor = color;

            Console.Write(new string(' ', Console.WindowWidth));

            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.Write(leftText);

            Console.SetCursorPosition(Console.WindowWidth - rightText.Length - 1, Console.WindowHeight - 1);
            Console.Write(rightText);

            Console.SetCursorPosition(cursorX, cursorY);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
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
                    Console.Beep();
                    Console.ResetColor();
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                    break;

                case "warning":
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("WARNING>" + message);
                    Console.ResetColor();
                    break;

                case "message":
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(message + "\n");
                    Console.ResetColor();
                    break;

                default:
                    Send("UI.Send>Incorrect mode! Check the UI.Send method call", "error");
                    break;
            }
        }
    }

    /// <summary>
    /// Draws a beautiful progress bar and can erase it automatically.
    /// </summary>
    /// <param name="color">Color of the filled progress bar</param>
    /// <param name="parts">How many parts is the progress bar divided into</param>
    /// <param name="x">Abscissa</param>
    /// <param name="y">Ordinate</param>

    public class Progressbar(ConsoleColor color, int parts, int x, int y)
    {
        private readonly int _parts = parts;
        private readonly ConsoleColor _color = color;
        private readonly int _x = x;
        private readonly int _y = y;

        /// <summary>
        /// Redraws the progressbar
        /// </summary>
        /// <param name="percent">Fill percentage</param>
        public void Draw(int percent)
        {
            int cursorX = Console.CursorLeft;
            int cursorY = Console.CursorTop;
            Console.SetCursorPosition(_x + _parts + 2, _y);
            Console.Write(new string('\b', _parts + 2));
            Console.Write("[");
            Console.ForegroundColor = _color;
            int filled = Math.Clamp(percent * _parts / 100, 0, _parts);
            Console.Write(new string('█', filled));
            Console.Write(new string('░', _parts - filled));
            Console.ResetColor();
            Console.Write("]");
            Console.SetCursorPosition(cursorX, cursorY);
        }

        /// <summary>
        /// Clears the progress bar
        /// </summary>
        public void Remove()
        {
            int cursorX = Console.CursorLeft;
            int cursorY = Console.CursorTop;
            Console.SetCursorPosition(_x + _parts + 2, _y);
            Console.Write(new string('\b', _parts + 2));
            Console.SetCursorPosition(cursorX, cursorY);
        }
    }
}
