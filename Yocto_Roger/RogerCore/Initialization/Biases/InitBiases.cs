namespace Yocto_Roger.RogerCore.Initialization.Biases
{
    /* 
Yocto Roger ;)
*****************
*Emotion Corp ;)*
*****************
Copyright 2025-2026 Emotion Corp.
    Internal Biases lib
*/

    /// <summary> 
    /// Class for initializing arrays of biases
    /// </summary>
    public class InitBiases()
    {
        /// <summary>
        /// Random filling of the array of biases
        /// </summary>
        /// <param name="biases">Array of biases</param>

        public static void Init(double[] biases)
        {
            for (int i = 0; i < biases.Length; i++)
                biases[i] = 0;
        }

        /// <summary>
        /// Random filling of a two-dimensional array of biases
        /// </summary>
        /// <param name="biases">Array of biases</param>

        public static void Init(double[,] biases)
        {
            for (int i = 0; i < biases.GetLength(0); i++)
                for (int j = 0; j < biases.GetLength(1); j++)
                    biases[i, j] = 0;
        }
    }
}
