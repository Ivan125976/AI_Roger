namespace Yocto_Roger
{
    internal class Weights
    {
        public static void Init(ref double[,] weights) //рандомное заполнение двухмерного массива весов
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
                Console.WriteLine("The weights have been successfully adjusted!");
        }

        public static void Init(ref double[][,] weights) //рандомное заполнение массива двухмерных массивов весов (подойдет для средних слоев)
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
                Console.WriteLine("The weights have been successfully adjusted!");
        }
    }
}
