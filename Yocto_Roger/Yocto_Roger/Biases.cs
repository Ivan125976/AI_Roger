namespace Yocto_Roger.Yocto_Roger
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
    public class Biases(Parameters param)
    {
        private readonly Parameters _param = param;
        /// <summary>
        /// Random filling of the array of biases
        /// </summary>
        /// <param name="biases">Array of biases</param>

        public void Init(ref double[] biases)
        {
            if (_param.isDebug)
                Console.Write($"biases[] = \n");
            for (int i = 0; i < biases.Length; i++)
            {
                biases[i] = RogerMath.rand.NextDouble() * 0.2 - 0.1;
                if (_param.isDebug)
                    Console.Write($"{biases[i]} ");
            }
            if (_param.isDebug)
                UI.UI.Send("\nThe biases have been successfully adjusted!");
        }

        /// <summary>
        /// Random filling of a two-dimensional array of biases
        /// </summary>
        /// <param name="biases">Array of biases</param>

        public void Init(ref double[,] biases)
        {
            if (_param.isDebug)
                Console.Write($"biases[,] = \n");
            for (int i = 0; i < biases.GetLength(0); i++)
            {
                for (int j = 0; j < biases.GetLength(1); j++)
                {
                    biases[i, j] = RogerMath.rand.NextDouble() * 0.2 - 0.1;
                    if (_param.isDebug)
                        Console.Write($"{biases[i, j]} ");
                }
                if (_param.isDebug)
                    Console.WriteLine();
            }
            if (_param.isDebug)
                UI.UI.Send("The biases have been successfully adjusted!");
        }
    }
}
