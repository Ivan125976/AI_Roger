namespace Yocto_Roger
{
    internal class Biases
    {
        public static void Init(ref double[] biases) //рандомное заполнение массива сдвигов
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
                Console.WriteLine("\nThe biases have been successfully adjusted!");
        }

        public static void Init(ref double[,] biases) //рандомное заполнение двухмерного массива сдвигов
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
                Console.WriteLine("The biases have been successfully adjusted!");
        }
    }
}
