using MemoryPack;

namespace Yocto_Roger.RogerCore
{
    /// <summary>
    /// Keeping values in string's
    /// </summary>
    [MemoryPackable]
    public partial class NeuralNetworkState
    {
        /// <summary>
        /// Input neurons count
        /// </summary>
        public int InputNeuronsCount { get; set; } = 14;
        /// <summary>
        /// middle neurons count
        /// </summary>
        public int MiddleNeuronsCount { get; set; } = 16;
        /// <summary>
        /// output neurons count
        /// </summary>
        public int OutputNeuronsCount { get; set; } = 8;
        /// <summary>
        /// input weights
        /// </summary>
        public double[,]? InputWeights { get; set; }
        /// <summary>
        /// middle weights
        /// </summary>
        public double[][,]? MiddleWeights { get; set; }
        /// <summary>
        /// output weights
        /// </summary>
        public double[,]? OutputWeights { get; set; }
        /// <summary>
        /// layers
        /// </summary>
        public int Layers { get; set; }
        /// <summary>
        /// mbias
        /// </summary>
        public double[,]? Mbias { get; set; }
        /// <summary>
        /// obias
        /// </summary>
        public double[]? Obias { get; set; }
    }
}
