using Yocto_Roger.IO;
using Yocto_Roger.UI.Interfaces;
using Yocto_Roger.Yocto_Roger;
using Yocto_Roger.Yocto_Roger.Initialization;
using Yocto_Roger.Yocto_Roger.Training;
using Yocto_Roger.Yocto_Roger.UtilityTools;

namespace Yocto_Roger
{
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
            // We are not idiots.
            // We are idiots++.
            // © Emotion Corp.

            Parameters param = new();
            NeuralNetworkState nNState = new();

            UI.GUI.GUI user = new(null!, null!, param);
            Auxiliary auxiliaryIO = new(param);
            MainIO io = new(param, null!, nNState);
            Weights weights = new(param);
            Biases biases = new(param);
            RogerMath aiMath = new(param);
            Training training = new(param, null!);
            NeuralNetwork nN = new(param, io, weights, biases, training);
            SettingsInterface settingsInterface = new(param, io, auxiliaryIO);

            io._nN = nN;
            user._roger = nN;
            user._settingsInterface = settingsInterface;
            training.roger = nN;

            user.StartEngine();
        }
    }
}
