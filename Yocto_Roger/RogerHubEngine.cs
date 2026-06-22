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
            ConsoleSize minSize = new(40, 120);

            while (true)
            {
                Console.Clear();
                ASCIIDraw.Logo(true);
                Console.WriteLine("Creating a software environment...");

                if (!CheckMinWindowSize(minSize))
                {
                    if (OperatingSystem.IsWindows())
                    {
                        try
                        {
                            Console.SetWindowSize(width: minSize.Width, height: minSize.Height);
                        }
                        catch (PlatformNotSupportedException)
                        {
                            Console.Write($"\x1b[8;{minSize.Height};{minSize.Width}t");

                            Thread.Sleep(25); // Delay to allow time for the size to change

                            if (!CheckMinWindowSize(minSize)) // If escape-code didn't work
                            {
                                Send($"Unable to resize the console. You'll have to do it yourself :( \nneed: \nWidth: {minSize.Width} \nHeight: {minSize.Height}", MessageType.error);
                                Send("Resize the window until i say \"Done\"", MessageType.note);
                                while (!CheckMinWindowSize(minSize))
                                {
                                    bool check = CheckMinWindowSize(minSize);

                                    if (check) { Send("Done"); }
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.Write("\n");
                        Console.Write($"\x1b[8;{minSize.Height};{minSize.Width}t");

                        Thread.Sleep(25); // Delay to allow time for the size to change

                        if (!CheckMinWindowSize(minSize)) // If escape-code didn't work
                        {
                            Send($"Unable to resize the console. You'll have to do it yourself :( \nneed: \nWidth: {minSize.Width} \nHeight: {minSize.Height}", MessageType.error);
                            Send("Resize the window until i say \"Done\"", MessageType.note);
                            while (!CheckMinWindowSize(minSize))
                            {
                                bool check = CheckMinWindowSize(minSize);

                                if (check) { Send("Done"); }
                            }
                        }

                    }
                }


                try { Console.Title = $"RogerHubEngine v{majorVersion}.{minorVersion}.{patchVersion}{revision}"; } catch { Send("Couldn't change the title", MessageType.warning); }

                // Some terminals (mostly on GNU/Linux) don't support Unicode, and throwing exception, but supporting UTF-8
                try { Console.InputEncoding = Encoding.Unicode; } catch { 
                    Console.InputEncoding = Encoding.UTF8;
                    Send("RogerHubEngine.InputEncoding> Yout system doesn't support Unicode! I'm setting up UTF-8", MessageType.warning);
                }
                try { Console.OutputEncoding = Encoding.Unicode; } catch { 
                    Console.OutputEncoding = Encoding.UTF8;
                    Send("RogerHubEngine.OutputEncoding> Your system doesn't support Unicode! I'm setting up UTF-8", MessageType.warning);
                }

                Parameters param = new();
                NeuralNetworkState nNState = new();
                UpdateManagerInterface updateManager = new();
                MainIO io = new(param, null!, nNState);
                Auxiliary auxiliaryIO = new(param);
                SettingsInterface settingsInterface = new(param, io, auxiliaryIO);
                Training training = new(param, null!);
                MainMenuInterface mainMenuInterface = new(settingsInterface, null!, updateManager);
                NeuralNetworkInterface neuralNetworkInterface = new(param, io, mainMenuInterface, null!);
                NeuralNetwork nN = new(param, io, training, neuralNetworkInterface, mainMenuInterface);

                io._nN = nN;
                training.roger = nN;
                mainMenuInterface._roger = nN;
                neuralNetworkInterface._neuralNetwork = nN;

                DrawLine(ConsoleColor.Magenta, "Emotion ;) 2025-2026", "Roger :D");
                Thread.Sleep(3000);

                mainMenuInterface.StartInterface();
            }
        }

        /// <summary>
        /// If Console Size > ConsoleSize - true. Else - false
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static bool CheckMinWindowSize(ConsoleSize size) => (Console.WindowWidth > size.Width && Console.WindowHeight > size.Height);
    }
}
