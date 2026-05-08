namespace Yocto_Roger
{
    public class Training
    {
        public static void SetUpBiases(ref double[] biases) //рандомное заполнение массива сдвигов
        {
            for (int i = 0; i < biases.Length; i++)
            {
                biases[i] = AIMath.rand.NextDouble() * 0.2 - 0.1;
                biases[i] = Math.Clamp(biases[i], -3.0, 3.0);
            }
            if (Parameters.isDebug)
                Console.WriteLine("The biases have been successfully adjusted!");
        }

        public static void SetUpWeights(ref double[,] weights)
        {
            for (int i = 0; i < weights.GetLength(0); i++)
            {
                for (int j = 0; j < weights.GetLength(1); j++)
                {
                    weights[i, j] = AIMath.rand.NextDouble() * 0.2 - 0.1;
                    weights[i, j] = Math.Clamp(weights[i, j], -3.0, 3.0);
                }
            }
            if (Parameters.isDebug)
                Console.WriteLine("The weights have been successfully adjusted!");
        }

        public static void WriteEducationArray(ref int[,] education, string path) //записывание данных из файла в переменную
        {
            var lines = File.ReadAllLines(path);

            for (int row = 0; row < lines.Length; row++)
            {
                string line = lines[row].Replace("-", "").Trim();
                var parts = line.Split(' ');

                for (int col = 0; col < 3; col++)
                    education[row, col] = int.Parse(parts[col]);
            }
        }

        public static void EducationWithTeacher() //обучение с поддержкой DropOut и deadParts
        {
            double[] errorOut = new double[NeuralNetwork.outputNeurons.Length];
            double[] errorMid = new double[NeuralNetwork.middleNeurons.Length];
            double[] deltaMid = new double[NeuralNetwork.middleNeurons.Length];
            double[] deltaOut = new double[NeuralNetwork.outputNeurons.Length];
            //double[,] oldWeights = new double[NeuralNetwork.weights2.GetLength(0), NeuralNetwork.weights2.GetLength(1)];

            for (int z = 0; z < Parameters.passes; z++)
            {
                float[] dropOutMasks = NeuralNetwork.GenerateDropOut();

                for (int i = 0; i < NeuralNetwork.educationArray.GetLength(0); i++)
                {
                    Array.Clear(errorMid, 0, errorMid.Length);
                    Array.Clear(errorOut, 0, errorOut.Length);
                    Array.Clear(deltaMid, 0, deltaMid.Length);
                    Array.Clear(deltaOut, 0, deltaOut.Length);

                    //for (int x = 0; x < NeuralNetwork.weights2.GetLength(0); x++)
                        //for (int y = 0; y < NeuralNetwork.weights2.GetLength(1); y++)
                            //oldWeights[x, y] = NeuralNetwork.weights2[x, y];

                    int[] binary = new int[8];
                    for (int j = 0; j < 8; j++)
                    {
                        int mask = 1 << (7 - j);
                        binary[j] = (NeuralNetwork.educationArray[i, 2] & mask) != 0 ? 1 : 0;
                    }

                    AIMath.WriteInput(ref NeuralNetwork.inputNeurons, NeuralNetwork.educationArray[i, 0], NeuralNetwork.educationArray[i, 1]);
                    //NeuralNetwork.SumWeights(ref NeuralNetwork.weights1, ref NeuralNetwork.inputNeurons, ref NeuralNetwork.middleNeurons, NeuralNetwork.bias1);
                    for (int l = 0; l < NeuralNetwork.middleNeurons.Length; l++) //DropOut 
                    {
                        NeuralNetwork.middleNeurons[l] *= dropOutMasks[l];
                    }
                    //NeuralNetwork.SumWeights(ref NeuralNetwork.weights2, ref NeuralNetwork.middleNeurons, ref NeuralNetwork.outputNeurons, NeuralNetwork.bias2);


                    for (int j = 0; j < NeuralNetwork.outputNeurons.Length; j++)
                    {
                        errorOut[j] = NeuralNetwork.outputNeurons[j] - binary[j]; //ошибка
                        deltaOut[j] = errorOut[j] * NeuralNetwork.outputNeurons[j] * (1 - NeuralNetwork.outputNeurons[j]); //дельта

                        for (int k = 0; k < NeuralNetwork.middleNeurons.Length; k++) { }
                            //NeuralNetwork.weights2[k, j] -= NeuralNetwork.middleNeurons[k] * deltaOut[j] * Parameters.learningRate;

                        //NeuralNetwork.bias2[j] -= deltaOut[j] * Parameters.learningRate;
                    }
                    for (int j = 0; j < NeuralNetwork.middleNeurons.Length; j++)
                    {
                        for (int l = 0; l < NeuralNetwork.outputNeurons.Length; l++)
                        {
                            //errorMid[j] += deltaOut[l] * oldWeights[j, l]; //ошибка

                        }

                        if (NeuralNetwork.middleNeurons[j] == 0)
                            deltaMid[j] = 0;
                        else
                            deltaMid[j] = errorMid[j] * NeuralNetwork.middleNeurons[j] * (1 - NeuralNetwork.middleNeurons[j]); //дельта

                        for (int k = 0; k < NeuralNetwork.inputNeurons.Length; k++)
                        {
                            //NeuralNetwork.weights1[k, j] -= NeuralNetwork.inputNeurons[k] * deltaMid[j] * Parameters.learningRate;
                        }
                        //NeuralNetwork.bias1[j] -= deltaMid[j] * Parameters.learningRate;
                    }
                }
            }

            Array.Clear(errorMid, 0, errorMid.Length);
            Array.Clear(errorOut, 0, errorOut.Length);
            Array.Clear(deltaMid, 0, deltaMid.Length);
            Array.Clear(deltaOut, 0, deltaOut.Length);
        }
    }
}
