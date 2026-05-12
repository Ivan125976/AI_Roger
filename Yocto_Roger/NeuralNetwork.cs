namespace Yocto_Roger
{
    internal class NeuralNetwork
    {
        public static int[,]? educationArray;

        public static double[] inputNeurons = new double[Parameters.inputNeuronsCount];
        public static double[] middleNeurons = new double[Parameters.middleNeuronsCount];
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
                    Console.Write("SetUp education array...");
                    educationArray = new int[UI.CountLines(Parameters.knowledgeFile), Parameters.inputNeuronsCount]; //TODO: формат .know2
                    UI.SendMessage("done");
                    Console.Write("Read knowledge...");
                    Training.WriteEducationArray(ref educationArray, Parameters.knowledgeFile);
                    UI.SendMessage("done");
                    Console.Write("Initialization biases...");
                    Biases.Init(ref Mbias);
                    Biases.Init(ref Obias);
                    UI.SendMessage("done");
                    Console.Write("Initialization weights...");
                    Weights.Init(ref inputWeights);
                    Weights.Init(ref outputWeights);
                    Weights.Init(ref middleWeights);
                    UI.SendMessage("done");
                    UI.SendMessage("\nInitialization complete");
                    while (true) { }
                    Console.WriteLine("Education. This make takes few minutes.");
                    //TODO: Обучение
                    UI.DrawLine(ConsoleColor.DarkRed, "Education Roger... This may take a few minutes.");
                    Training.EducationWithTeacher();
                    Array.Clear(educationArray, 0, educationArray.Length);
                    break;

                case 1:
                    Console.Write("Loading your Roger...");
                    Save_Load.LoadRoger();
                    break;
            }
            Console.WriteLine("Hello! I'm Roger, the MLP AI from Emotion!");
            while (true)
            {
                UI.DrawLine(ConsoleColor.DarkGreen, "Ready.   >>> Enter SAVE for saving Roger to .roger2 file");
                AIMath.WriteInput(ref inputNeurons);
                UI.DrawLine(ConsoleColor.DarkRed, "Calculation neurons...");
                //TODO: Складывание весов
                UI.DrawLine(ConsoleColor.DarkRed, "Rounding...");
                AIMath.Rounding(ref outputNeurons);
                UI.DrawLine(ConsoleColor.DarkRed, "Almost ready...");
                Console.WriteLine($"I think it's {AIMath.WriteOutput(outputNeurons)}");
                Console.WriteLine("Press any key to continue...");
                UI.DrawLine(ConsoleColor.Magenta, "Waiting.");
                Console.ReadKey();
                Console.Clear();
            }
        }

        public static float[] GenerateDropOut()
        {
            float[] masks = new float[Parameters.middleNeuronsCount];
            float keepProb = 1.00f - (Parameters.DropOutPercent * 0.01f);

            if (Parameters.DropOutPercent == 0)
            {
                for (int i = 0; i < masks.Length; i++)
                    masks[i] = 1.0f;
                return masks;
            }
            else
            {
                for (int i = 0; i < masks.Length; i++)
                {
                    if (AIMath.rand.Next(0, 100) < Parameters.DropOutPercent)
                        masks[i] = 0;
                    else
                        masks[i] = 1.0f / keepProb;
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
