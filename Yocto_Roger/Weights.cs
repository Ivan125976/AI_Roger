namespace Yocto_Roger
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

    internal class Weights
    {
        /// <summary>
        /// Random filling of a two-dimensional array of weights
        /// </summary>
        /// <param name="weights">Array of weights</param>

        public static void Init(ref double[,] weights)
        {
            if (Parameters.isDebug)
                Console.Write($"weights[,] = \n");
            for (int i = 0; i < weights.GetLength(0); i++)
            {
                for (int j = 0; j < weights.GetLength(1); j++)
                {
                    weights[i, j] = AIMath.rand.NextDouble() * 0.2 - 0.1;
                    if (Parameters.isDebug)
                        Console.Write($"{weights[i, j]} ");
                }
                if (Parameters.isDebug)
                    Console.WriteLine();
            }
            if (Parameters.isDebug)
                UI.Send("The weights have been successfully adjusted!");
        }

        /// <summary>
        /// Random filling an array of two-dimensional weight arrays (suitable for middle layers)
        /// </summary>
        /// <param name="weights">Array of weights</param>

        public static void Init(ref double[][,] weights)
        {
            if (Parameters.isDebug)
                Console.Write($"weights[][,] = \n");
            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] = new double[Parameters.middleNeuronsCount, Parameters.middleNeuronsCount];
                for (int j = 0; j < weights[i].GetLength(0); j++)
                {
                    for (int k = 0; k < weights[i].GetLength(1); k++)
                    {
                        weights[i][j, k] = AIMath.rand.NextDouble() * 0.2 - 0.1;
                        if (Parameters.isDebug)
                            Console.Write($"{weights[i][j, k]} ");
                    }
                    if (Parameters.isDebug)
                        Console.WriteLine();
                }
                if (Parameters.isDebug)
                    Console.WriteLine(new string('=', Console.WindowWidth));
            }
            if (Parameters.isDebug)
                UI.Send("The weights have been successfully adjusted!");
        }
    }
}
