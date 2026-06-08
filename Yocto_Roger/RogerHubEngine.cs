using Yocto_Roger.IO;
using Yocto_Roger.RogerCore;
using Yocto_Roger.RogerCore.Initialization.Biases;
using Yocto_Roger.RogerCore.Initialization.Weights;
using Yocto_Roger.RogerCore.Training;
using Yocto_Roger.UI.Interfaces;

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

            UI.GUI.GUI user = new(null!, null!);
            Auxiliary auxiliaryIO = new(param);
            MainIO io = new(param, null!, nNState);
            CreateWeights middleWeightsCreator = new(param);
            InitWeights weights = new();
            Training training = new(param, null!);
            NeuralNetwork nN = new(param, io, training, user, middleWeightsCreator);
            SettingsInterface settingsInterface = new(param, io, auxiliaryIO);

            io._nN = nN;
            user._roger = nN;
            user._settingsInterface = settingsInterface;
            training.roger = nN;

            user.StartEngine(true);
        }
    }
}
