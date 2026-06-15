using System.Text;
using Yocto_Roger.IO;
using static Yocto_Roger.Configuration.EngineVersion;
using Yocto_Roger.RogerCore;
using Yocto_Roger.RogerCore.Initialization.Weights;
using Yocto_Roger.RogerCore.Training;
using Yocto_Roger.UI.CUI;
using Yocto_Roger.UI.Interfaces;
using static Yocto_Roger.UI.CUI.CUI;

namespace Yocto_Roger
{
    /* 
         We are not idiots.
         We are idiots++.
         © Emotion Corp.
    */

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
            Console.WriteLine("Configuring console...");

            if (Console.WindowHeight < 20 || Console.WindowWidth < 50)
            {
                Send("The window is too small >:(", MessageType.error);
                Environment.Exit(1);
            }

            Console.Title = $"RogerHubEngine v{majorVersion}.{minorVersion}.{patchVersion}{revision} CharLie";

            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;

            Parameters param = new();
            NeuralNetworkState nNState = new();

            MainIO io = new(param, null!, nNState);
            Auxiliary auxiliaryIO = new(param);
            SettingsInterface settingsInterface = new(param, io, auxiliaryIO);
            CreateWeights middleWeightsCreator = new(param);
            Training training = new(param, null!);
            MainMenuInterface mainMenuInterface = new(settingsInterface, null!);
            NeuralNetworkInterface neuralNetworkInterface = new(param, io, mainMenuInterface, null!);
            NeuralNetwork nN = new(param, io, training, middleWeightsCreator, neuralNetworkInterface, mainMenuInterface);

            io._nN = nN;
            training.roger = nN;
            mainMenuInterface._roger = nN;
            neuralNetworkInterface._neuralNetwork = nN;

            DrawLine(ConsoleColor.Magenta, "Emotion ;) 2026", "Roger :D");
            Thread.Sleep(3000);

            mainMenuInterface.StartInterface();
        }
    }
}
