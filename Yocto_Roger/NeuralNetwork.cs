namespace Yocto_Roger
{
    internal class NeuralNetwork
    {
        public static int[,]? educationArray;

        public static double[] inputNeurons = new double[Parameters.inputNeuronsCount];
        public static double[,] middleNeurons = new double[Parameters.Mlayers, Parameters.middleNeuronsCount];
        public static double[] outputNeurons = new double[Parameters.outputNeuronsCount];

        public static double[,] inputWeights = new double[inputNeurons.Length, middleNeurons.Length];
        public static double[][,] middleWeights = new double[Parameters.layers][,];
        public static double[,] outputWeights = new double[middleNeurons.Length, outputNeurons.Length];

        public static double[,] Mbias = new double[Parameters.Mlayers, middleNeurons.Length];
        public static double[] Obias = new double[outputNeurons.Length];

        public static void StartAI(int mode)
        {
            Console.WriteLine("StartAI in mode " + mode);
            switch (mode)
            {
                case 0:
                    Console.Write("Initialization biases...");
                    Biases.Init(ref Mbias);
                    Biases.Init(ref Obias);
                    UI.SendMessage("done");
                    Console.Write("Initialization weights...");
                    Weights.Init(ref inputWeights);
                    Weights.Init(ref outputWeights);
                    Weights.Init(ref middleWeights);
                    UI.SendMessage("done");
                    UI.SendMessage("Initialization complete");
                    Console.Write("SetUp education array...");
                    //d3ath_script: сделай инициализацию массива обучения, в зависимости от созданной нейросети, тоесть в зависимости от размера входных нейронов и выходных. Придумай свою логику, только шоб работало)
                    UI.SendMessage("done");
                    Console.Write("Read knowledge...");
                    //и еще тут запись в массив обучения из файла, json, .roger2, какой надо. Сделай только чтобы он поддерживал .roger2, надо обратную поддержку оставить с версии 2.1 на 2.2
                    UI.SendMessage("done");
                    Console.Write("Education...");
                    UI.DrawLine(ConsoleColor.DarkRed, "Creating your Roger, please wait :D");
                    //Training.Education();
                    UI.SendMessage("done");
                    Console.Write("Cleaning...");
                    //и тут после этого еще чистку массива до нуля, или если можно вообще его удаление. Давай, полагаюсь на тебя)))
                    UI.SendMessage("done");
                    break;

                case 1:
                    Console.Write("Loading your Roger...");
                    Save_Load.LoadRoger();
                    break;
            }
            Console.WriteLine("Hello! I'm Roger, the MLP AI from Emotion!");
            while (true)
            {
                UI.DrawLine(ConsoleColor.DarkGreen, "Not-ready AI Interface v2.2");
                AIMath.numToBin(ref inputNeurons);
                //TODO: Складывание весов
            }
        }

        public static float[,] GenerateDropOut()
        {
            if (Parameters.isDebug)
                Console.WriteLine("DropOut Matrix = ");
            float[,] masks = new float[Parameters.Mlayers, Parameters.middleNeuronsCount];
            float keepProb = 1.00f - (Parameters.DropOutPercent * 0.01f);

            if (Parameters.DropOutPercent == 0)
            {
                for (int i = 0; i < masks.GetLength(0); i++)
                {
                    for (int j = 0; j < masks.GetLength(1); j++)
                    {
                        masks[i, j] = 1.0f;
                        if (Parameters.isDebug)
                            Console.Write(masks[i, j] + " ");
                    }
                    if (Parameters.isDebug)
                        Console.WriteLine();
                }
                return masks;
            }
            else
            {
                for (int i = 0; i < masks.GetLength(0); i++)
                {
                    for (int j = 0; j < masks.GetLength(1); j++)
                    {
                        if (AIMath.rand.Next(0, 101) < Parameters.DropOutPercent)
                            masks[i, j] = 0;
                        else
                            masks[i, j] = 1.0f / keepProb;
                        if (Parameters.isDebug)
                            Console.Write(masks[i, j] + " ");
                    }
                    if (Parameters.isDebug)
                        Console.WriteLine();
                }
            }
            return masks;
        }

        public static void SumWeights(ref double[,] oldweights, ref double[] oldNeurons, ref double[] newNeurons, double[] biases) //нахождение новых нейронов
        {
            if (Parameters.isDebug)
                Console.Write("Sum of weights - ");
            for (int i = 0; i < newNeurons.Length; i++)
            {
                double temp = 0;
                for (int j = 0; j < oldNeurons.Length; j++)
                    temp += oldweights[j, i] * oldNeurons[j];
                temp += biases[i];
                newNeurons[i] = AIMath.Sigmoida(temp);
                if (Parameters.isDebug)
                    Console.Write(newNeurons[i] + " ");
            }
            if (Parameters.isDebug)
                Console.WriteLine();
        }
    }
}
