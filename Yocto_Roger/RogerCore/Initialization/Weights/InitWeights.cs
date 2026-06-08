using Yocto_Roger.RogerCore.UtilityTools;

namespace Yocto_Roger.RogerCore.Initialization.Weights
{
    /* 
Yocto Roger ;)
*****************
*Emotion Corp ;)*
*****************
Copyright 2025-2026 Emotion Corp.
    Internal weights lib
*/

    /// <summary>
    /// Class for initializing arrays of weights
    /// </summary>

    public class InitWeights()
    {
        /// <summary>
        /// Xavier Uniform method for two-dimensional weight arrays
        /// </summary>
        /// <param name="weights">Array of weights</param>

        public static void Init(double[,] weights)
        {
            double limit = (double)Math.Sqrt(6 / (weights.GetLength(0) * 1.0 + weights.GetLength(1) * 1.0));
#if DEBUG
            Console.Write($"weights[,] = \n");
            Send($"Xaiver Uniform Initialization; limit = {limit}", MessageType.warning);
#endif
            for (int i = 0; i < weights.GetLength(0); i++)
            {
                for (int j = 0; j < weights.GetLength(1); j++)
                {
                    weights[i, j] = RogerMath.rand.NextDouble() * limit * 2 - limit;
#if DEBUG
                    Console.Write($"{weights[i, j]} ");
#endif
                }
#if DEBUG
                Console.WriteLine();
#endif
            }
#if DEBUG
            Send("The weights have been successfully adjusted!");
#endif
        }

        /// <summary>
        /// Xavier Uniform method an array of two-dimensional weight arrays (suitable for middle layers)
        /// </summary>
        /// <param name="weights">Array of weights</param>

        public static void Init(double[][,] weights)
        {
            if (weights.Length > 0)
            {
#if DEBUG
                Console.Write($"weights[][,] = \n");
                Send($"Xaiver Uniform Initialization; limit = {limit}", MessageType.warning);
#endif
                for (int i = 0; i < weights.Length; i++)
                {
                    double limit = (double)Math.Sqrt(6 / (weights[i].GetLength(0) * 1.0 + weights[i].GetLength(1) * 1.0));
                    for (int j = 0; j < weights[i].GetLength(0); j++)
                    {
                        for (int k = 0; k < weights[i].GetLength(1); k++)
                        {
                            weights[i][j, k] = RogerMath.rand.NextDouble() * limit * 2 - limit;
#if DEBUG
                            Console.Write($"{weights[i][j, k]} ");
#endif
                        }
#if DEBUG
                        Console.WriteLine();
#endif
                    }
#if DEBUG
                    Console.WriteLine(new string('=', Console.WindowWidth));
#endif
                }
#if DEBUG
                Send("The weights have been successfully adjusted!");
#endif
            }
        }
    }
}
