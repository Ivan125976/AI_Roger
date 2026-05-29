namespace Yocto_Roger
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
    
    internal class Biases
    {
        /// <summary>
        /// Random filling of the array of biases
        /// </summary>
        /// <param name="biases">Array of biases</param>

        public static void Init(ref double[] biases)
        {
            if (Parameters.isDebug)
                Console.Write($"biases[] = \n");
            for (int i = 0; i < biases.Length; i++)
            {
                biases[i] = AIMath.rand.NextDouble() * 0.2 - 0.1;
                if (Parameters.isDebug)
                    Console.Write($"{biases[i]} ");
            }
            if (Parameters.isDebug)
                UI.Send("\nThe biases have been successfully adjusted!");
        }

        /// <summary>
        /// Random filling of a two-dimensional array of biases
        /// </summary>
        /// <param name="biases">Array of biases</param>

        public static void Init(ref double[,] biases)
        {
            if (Parameters.isDebug)
                Console.Write($"biases[,] = \n");
            for (int i = 0; i < biases.GetLength(0); i++)
            {
                for (int j = 0; j < biases.GetLength(1); j++)
                {
                    biases[i, j] = AIMath.rand.NextDouble() * 0.2 - 0.1;
                    if (Parameters.isDebug)
                        Console.Write($"{biases[i, j]} ");
                }
                if (Parameters.isDebug)
                    Console.WriteLine();
            }
            if (Parameters.isDebug)
                UI.Send("The biases have been successfully adjusted!");
        }
    }
}
