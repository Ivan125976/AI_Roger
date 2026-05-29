namespace Yocto_Roger
{
    /* 
Yocto Roger ;)
*****************
*Emotion Corp ;)*
*****************
Copyright 2025-2026 Emotion Corp.
RogerHub configuration
*/

    /// <summary>
    /// contains all RogerHubEngine parameters
    /// </summary>
    public class Parameters
    {
        /// <summary>
        /// RogerHubEngine version
        /// </summary>
        public const string version = "2.2.0";

        /// <summary>
        /// RogerHubEngine revision
        /// </summary>
        public const char revision = ' ';

        /// <summary>
        /// Debug flag. If enabled, logs will be written to the console. This is for developers.
        /// </summary>
        public static readonly bool isDebug = false;

        /// <summary>
        /// Number of passes during training on training data
        /// </summary>
        public static int passes = 500;

        /// <summary>
        /// The coefficient of change of weights and biases of the neural network.
        /// </summary>
        public static float learningRate = 0.02f;

        /// <summary>
        /// The percentage of response of the built-in DropOut subsystem during neural network training
        /// </summary>
        public static float DropOutPercent = 3.0f;

        /// <summary>
        /// The path to the neural network's knowledge base on which it will need to be trained
        /// </summary>
        public static string knowledgeFile = string.Empty;

        /// <summary>
        /// The path to the finished neural network, sealed in a file
        /// </summary>
        public static string roger2 = string.Empty;

        /// <summary>
        /// Number of input neurons
        /// </summary>
        public static int inputNeuronsCount = 14;

        /// <summary>
        /// Number of middle neurons
        /// </summary>
        public static int middleNeuronsCount = 16;

        /// <summary>
        /// Number of output neurons
        /// </summary>
        public static int outputNeuronsCount = 8;

        /// <summary>
        /// Number of layers in a neural network
        /// </summary>
        public static int layers = 3;

        /// <summary>
        /// Number of middle layers in a neural network
        /// </summary>
        public static int Mlayers = layers - 2; //Mlayers - Count of middle layers.
    }
}
