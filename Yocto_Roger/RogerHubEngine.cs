using System.Diagnostics;
using System.Management;
using System.Runtime.Versioning;
using System.Text;
using Yocto_Roger.IO;
using Yocto_Roger.RogerCore;
using Yocto_Roger.RogerCore.Training;
using Yocto_Roger.UI.CUI;
using Yocto_Roger.UI.Interfaces;
using static Yocto_Roger.Configuration.EngineVersion;
using static Yocto_Roger.UI.CUI.CUI;


namespace Yocto_Roger
{

    /* 
     We are not idiots.
     We are idiots++.
     © Emotion Corp.
    */

    /// <summary>
    /// Keeps minimal size of the console
    /// </summary>
    public struct ConsoleSize(ushort height, ushort width) // By default, in Windows, the console size is 25 in height and 80 in width.
    {
        /// <summary>
        /// Height
        /// </summary>
        public ushort Height = height;
        /// <summary>
        /// Width
        /// </summary>
        public ushort Width = width;
    }

    /// <summary>
    /// Main class
    /// </summary>
    public class RogerHubEngine
    {
        /// <summary>
        /// A class that creates the environment and starts RogerHubEngine
        /// </summary>
        static public void Main()
        {
            ConsoleSize minSize = new(40, 90);

            while (true)
            {
                Console.Clear();
                Console.WriteLine($"""
                     _____                       _    _       _     ______             _            
                    |  __ \                     | |  | |     | |   |  ____|           (_)           
                    | |__) |___   __ _  ___ _ __| |__| |_   _| |__ | |__   _ __   __ _ _ _ __   ___ 
                    |  _  // _ \ / _` |/ _ \ '__|  __  | | | | '_ \|  __| | '_ \ / _` | | '_ \ / _ \
                    | | \ \ (_) | (_| |  __/ |  | |  | | |_| | |_) | |____| | | | (_| | | | | |  __/
                    |_|  \_\___/ \__, |\___|_|  |_|  |_|\__,_|_.__/|______|_| |_|\__, |_|_| |_|\___| V{majorVersion}.{minorVersion}
                                  __/ |                                           __/ |             
                                 |___/                                           |___/        
                    """);
                Console.WriteLine("Configuring console...");

                if (Console.WindowWidth < minSize.Width || Console.WindowHeight < minSize.Height)
                {
                    Send($"The window is too small (min - {minSize.Width}x{minSize.Height}) >:(", MessageType.warning);

                    bool isWinTerm = false;
                    bool isWindows = OperatingSystem.IsWindows();

                    if (isWindows) isWinTerm = IsWindowsTerminal();

                    bool isModernTerminal = isWinTerm || !isWindows;

                    if (isModernTerminal == false) // Checking if running in Windows11 Terminal, so "Windows Terminal" doesn't allow changing window size
                    {
                        Console.Write("""
                        Can i change the window size?
                        (y/n) >>> 
                        """);
                        if (char.TryParse(Console.ReadLine()?.ToLower(), out char input))
                        {
                            switch (input)
                            {
                                case 'y' or 'Y':
                                    try
                                    {
                                        if (isWindows)
                                            Console.SetWindowSize(width: minSize.Width, height: minSize.Height);
                                    }
                                    catch (PlatformNotSupportedException)
                                    {
                                        Console.WriteLine($"Your terminal doesn't allow changing the window size, please change it yourself. Size need to be: Width: {minSize.Width} | Height: {minSize.Height}");
                                        Console.WriteLine($"But wait... I can try to change it with escape-code.\n\nPress Enter to try to change the size with escape code...");
                                        Console.ReadLine();
                                        Console.Write($"\x1b[8;{minSize.Height};{minSize.Width}t");
                                        switch (Console.WindowHeight > minSize.Height && Console.WindowWidth > minSize.Width)
                                        {
                                            case true:
                                                Send("Success! Everything's okay, you're ready to use this program", MessageType.message);
                                                Console.WriteLine("Press Enter to enter to the main menu");
                                                Console.ReadLine();
                                                break;

                                            case false:
                                                Send($"I didn't succeed... So please change it yourself. Let me remind you: need Height: {minSize.Height}, Need Width {minSize.Width}", MessageType.warning);
                                                Console.WriteLine("Press enter to close the app");
                                                Console.ReadLine();
                                                Environment.Exit(0);
                                                break;
                                        }
                                    }
                                    continue;

                                case 'n' or 'N':
                                    Console.WriteLine("Whatever you want... then bye... :/");
                                    Thread.Sleep(2000);
                                    Environment.Exit(0);
                                    break;

                                default:
                                    Send("BRUH", MessageType.error);
                                    Main();
                                    break;
                            }
                        }
                        else
                        {
                            Send("Bruh... Incorrect input, are you serious?", MessageType.error);
                            Thread.Sleep(500);
                            Send("Please, press Enter and don't screw up again", MessageType.note);
                            Console.ReadKey();
                            continue;
                        }
                    }
                    else
                    {
                        Send("You running it on Windows 11 (or just using Windows Terminal on windows 10) - it's using Windows Terminal, and he doesn't allow changing window size with default tools, so i try to change it with experimental function, with escape-code.\n\nPress Enter when you'll be ready", MessageType.warning);
                        Console.ReadLine();
                        Console.Write($"\x1b[8;{minSize.Height};{minSize.Width}t");
                        Thread.Sleep(200);

                        if (Console.WindowWidth < minSize.Width || Console.WindowHeight < minSize.Height)
                        {
                            Console.WriteLine("Window size still didn't changed, it means escape-code doesn't work, so please change it yourself.\nPress Enter to exit");
                            Console.ReadLine();
                            Environment.Exit(0);
                        }
                        else
                        {
                            Console.WriteLine("I've successfully changed it, yappy.");
                            Thread.Sleep(2000);
                        }
                    }
                }

                break;
            }

            try { Console.Title = $"RogerHubEngine v{majorVersion}.{minorVersion}.{patchVersion}{revision} CharLie"; } catch { }

            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;

            Parameters param = new();
            NeuralNetworkState nNState = new();

            MainIO io = new(param, null!, nNState);
            Auxiliary auxiliaryIO = new(param);
            SettingsInterface settingsInterface = new(param, io, auxiliaryIO);
            Training training = new(param, null!);
            MainMenuInterface mainMenuInterface = new(settingsInterface, null!);
            NeuralNetworkInterface neuralNetworkInterface = new(param, io, mainMenuInterface, null!);
            NeuralNetwork nN = new(param, io, training, neuralNetworkInterface, mainMenuInterface);

            io._nN = nN;
            training.roger = nN;
            mainMenuInterface._roger = nN;
            neuralNetworkInterface._neuralNetwork = nN;

            DrawLine(ConsoleColor.Magenta, "Emotion ;) 2026", "Roger :D");
            Thread.Sleep(3000);

            mainMenuInterface.StartInterface();
        }

        [SupportedOSPlatform("windows")]
        static bool IsWindowsTerminal() // This Method Work Only On Windows
        {
            try
            {
                int currentPid = Environment.ProcessId;

                string query = $"SELECT ParentProcessId FROM Win32_Process WHERE ProcessId = {currentPid}";
                using var searcher = new ManagementObjectSearcher(query);
                using var results = searcher.Get();
                foreach (ManagementObject mO in results.Cast<ManagementObject>())
                {
                    int parentPid = Convert.ToInt32(mO["ParentProcessId"]);
                    if (parentPid == 0) return false;

                    using Process parentProcess = Process.GetProcessById(parentPid);
                    return parentProcess.ProcessName.Equals("WindowsTerminal", StringComparison.OrdinalIgnoreCase);
                }
            }
            catch
            {
                return false;
            }
            return false;
        }
    }
}
