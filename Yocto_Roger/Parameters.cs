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
        /// Debug flag. If enabled, logs will be written to the console. This is for developers.
        /// </summary>
        public readonly bool isDebug = false;

        /// <summary>
        /// Version of RogerHubEngine
        /// </summary>
        public readonly string version = "2.2.0";

        /// <summary>
        /// Revision of version RogerHubEngine
        /// </summary>
        public readonly string revision = "";
        /// <summary>
        /// Number of passes during training on training data
        /// </summary>
        public int passes = 500;

        /// <summary>
        /// The coefficient of change of weights and biases of the neural network.
        /// </summary>
        public float learningRate = 0.02f;

        /// <summary>
        /// The percentage of response of the built-in DropOut subsystem during neural network training
        /// </summary>
        public float DropOutPercent = 3.0f;

        /// <summary>
        /// The path to the neural network's knowledge base on which it will need to be trained
        /// </summary>
        public string knowledgeFile = string.Empty;

        /// <summary>
        /// The path to the finished neural network, sealed in a file
        /// </summary>
        public string roger2 = string.Empty;

        /// <summary>
        /// Number of input neurons
        /// </summary>
        public int inputNeuronsCount = 14;

        /// <summary>
        /// Number of middle neurons
        /// </summary>
        public int middleNeuronsCount = 16;

        /// <summary>
        /// Number of output neurons
        /// </summary>
        public int outputNeuronsCount = 8;

        /// <summary>
        /// Number of layers in a neural network
        /// </summary>
        public int layers = 3;

    }
}
