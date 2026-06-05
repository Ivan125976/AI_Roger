using Yocto_Roger.IO;
using Yocto_Roger.UI;
using Yocto_Roger.Yocto_Roger;

namespace Yocto_Roger
{
    /// <summary>
    /// Main class
    /// </summary>
    public class RogerHubEngine
    {
        //public RogerHubEngine(UI.UI user, Parameters param, NeuralNetwork nN)
        //{
        //    _user = user;
        //    _param = param;
        //    _nN = nN;
        //}

        /// <summary>
        /// A class that creates the environment and starts RogerHubEngine
        /// </summary>
        static public void Main()
        {
            Parameters param = new();
            NeuralNetworkState nNState = new();

            UI.UI user = new(null!, null!, param);
            Auxiliary auxiliaryIO = new(param);
            MainIO io = new(param, null!, nNState);
            Weights weights = new(param);
            Biases biases = new(param);
            RogerMath aiMath = new(param);
            Training training = new(param, null!, aiMath);
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
