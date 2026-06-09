using System.Runtime.CompilerServices;
using System.Text;
using Yocto_Roger.RogerCore;
using Yocto_Roger.UI.Interfaces;
using Velopack;
using Velopack.Sources;
using static Yocto_Roger.Configuration.EngineVersion;

namespace Yocto_Roger.UI.GUI
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
    public class GUI(NeuralNetwork nN, SettingsInterface settingsInterface)
    {
        /// <summary>
        /// Link to neural network file
        /// </summary>
        public NeuralNetwork _roger = nN;

        /// <summary>
        /// Link to SettingsInterface
        /// </summary>
        public SettingsInterface _settingsInterface = settingsInterface;

        /// <summary>
        /// Launches the console UI
        /// </summary>
        public void StartEngine(bool needToConfigure)
        {
            if (needToConfigure)
            {
                Console.WriteLine("Configuring console...");

                if (Console.WindowHeight < 20 || Console.WindowWidth < 50)
                {
                    Send("The window is too small >:(", MessageType.error);
                    Environment.Exit(1);
                }

                Console.Title = "RogerHubEngine v2.2.0 CharLie";

                Console.InputEncoding = Encoding.Unicode;
                Console.OutputEncoding = Encoding.Unicode;

                DrawLine(ConsoleColor.Magenta, "Emotion ;) 2026", "Roger :D");
                Thread.Sleep(3000);
            }
            int i = 0;
            while (true)
            {
                Console.Clear();
#if RELEASE
                DrawLine(ConsoleColor.DarkMagenta, $"Welcome to the RogerHubEngine! v.{version} CHARLIE", DateTime.Now.Date.ToString("dd/MM/yyyy"));
#elif DEBUG
                DrawLine(ConsoleColor.DarkMagenta, $"Welcome to the RogerHubEngine! v.{version}.{revision} CHARLIE >DEBUG BUILD<", DateTime.Now.Date.ToString("dd/MM/yyyy"));
#endif
                Send("This project is still in the development stage.", MessageType.warning);
                Send("This is a BETA build. Some functionality may not work. Have fun testing :D", MessageType.warning);
                Console.Write("""
                    
                    1. Start Roger in training mode
                    2. Load your roger (neural network) from the file
                    3. Options for training mode...
                    4. RRNNs settings...
                    5. Update manager...
                    6. About...
                    7. Exit from RogerHub 
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
                            Send("RRNNs.RRNNs>This page isn't ready", MessageType.error);
                            break;

                        case 5:
                            VelopackApp.Build().Run();
                            GithubSource githubSource = new("https://github.com/Ivan125976/AI_Roger", null, false);
                            var mgr = new UpdateManager(githubSource);

                            if (mgr.IsInstalled)
                            {

                                Console.WriteLine(
                                    $"""
                                1. Check for updates and update
                                2. Get outta here to main menu
                                """);
                                if (int.TryParse(Console.ReadLine(), out int choice))
                                {
                                    switch (choice)
                                    {
                                        case 1:
                                            UpdateInfo info = mgr.CheckForUpdates();

                                            if (info != null)
                                            {
                                                Console.WriteLine("Updates found! Downloading...");
                                                try
                                                {
                                                    mgr.DownloadUpdates(info);
                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.WriteLine($"Failed to download the update: {ex}");
                                                    break;
                                                }
                                                Console.WriteLine("Updates was downloaded successful!\nTrying to apply it, the app will be restarted in new version...");
                                                Thread.Sleep(5000); // For user can to read the message
                                                try
                                                {
                                                    mgr.ApplyUpdatesAndRestart(info);
                                                }
                                                catch (Exception ex)
                                                {
                                                    Send("Failed to apply updates, here's my error: ", MessageType.error);
                                                    Console.WriteLine(ex.ToString(), ConsoleColor.Red);
                                                }
                                            }
                                            else
                                            {
                                                Send("Hey, hey, calm down, you have the latest version");
                                                Thread.Sleep(5000);
                                            }
                                            break;

                                        case 2:
                                            break;

                                    }

                                }
                                else
                                {
                                    Send("Incorrect input (-_0)", MessageType.error);
                                    Thread.Sleep(1000);
                                }
                                break;
                            }
                            else
                            {
                                Send("I can't find installed tools, maybe you run it in ide? If you run it in visual studio for example, then the library which response for updates, won't working", MessageType.error);
                            }
                            break;

                        case 6:
                            Console.WriteLine($" Github: https://github.com/Ivan125976/AI_Roger\n\n Authors: \n Axolotl512 - AI and RogerHubEngine \n d3ath-script - RRNNs, IO and compiling \n\n RogerHubEngine v{version}.{revision} build:CHARLIE \n" +
                                " RogerCore v2.2 \n RRNNs isn't ready \n OpenRB isn't ready \n\n Press any key to continue ");
                            Console.ReadKey();
                            break;

                        case 7:
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
                    Send("Incorrect input >:(", MessageType.error);
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
        public static void Send(string message, MessageType mode = MessageType.message)
        {
            switch (mode)
            {
                case MessageType.error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERROR>" + message);
                    Console.ResetColor();
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                    break;

                case MessageType.warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("WARNING>" + message);
                    Console.ResetColor();
                    break;

                case MessageType.message:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(message + "\n");
                    Console.ResetColor();
                    break;

                default:
                    Send("UI.Send>Incorrect mode! Check the UI.Send method call", MessageType.error);
                    break;
            }
        }
    }
}
