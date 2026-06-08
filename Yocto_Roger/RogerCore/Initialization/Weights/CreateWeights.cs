namespace Yocto_Roger.RogerCore.Initialization.Weights
{
    /// <summary>
    /// Creates an array of middle weights
    /// </summary>
    public class CreateWeights(Parameters param)
    {
        private readonly Parameters _param = param;

        /// <summary>
        /// Creates an array of middle weights.
        /// </summary>
        /// <param name="weights">Array of middle weights</param>
        public void CreateMiddleWeights(double[][,] weights)
        {
            for (int i = 0; i < weights.Length; i++)
                weights[i] = new double[_param.middleNeuronsCount, _param.middleNeuronsCount];
        }
    }
}
