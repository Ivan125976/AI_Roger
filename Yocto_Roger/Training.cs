namespace Yocto_Roger
{
    public class Training
    {
        public static void Education(ref int[] inputNeurons, ref double[,] middleNeurons, ref double[] outputNeurons, ref double[,] inputWeights,
            ref double[][,] middleWeights, ref double[,] outputWeights, ref double[,] middleBiases, ref double[] outputBiases, string[,] educationArray) //обучение с поддержкой DropOut
        {
            double[,] errorMid = new double[middleNeurons.GetLength(0), middleNeurons.GetLength(1)];
            double[,] deltaMid = new double[middleNeurons.GetLength(0), middleNeurons.GetLength(1)];
            double[] errorOut = new double[outputNeurons.Length];
            double[] deltaOut = new double[outputNeurons.Length];
            double[,] oldOutputWeights = new double[middleNeurons.GetLength(1), outputNeurons.Length];
            double[][,] oldMiddleWeights = new double[Parameters.Mlayers - 1][,];

            for (int passes = 0; passes < Parameters.passes; passes++)
            {
                for (int i = 0; i < educationArray.GetLength(0); i++)
                {
                    Array.Clear(errorMid, 0, errorMid.Length);
                    Array.Clear(errorOut, 0, errorOut.Length);
                    Array.Clear(deltaMid, 0, deltaMid.Length);
                    Array.Clear(deltaOut, 0, deltaOut.Length);

                    for (int x = 0; x < outputWeights.GetLength(0); x++)
                        for (int y = 0; y < outputWeights.GetLength(1); y++)
                            oldOutputWeights[x, y] = outputWeights[x, y];

                    for (int x = 0; x < Parameters.Mlayers - 1; x++)
                    {
                        oldMiddleWeights[x] = new double[middleNeurons.GetLength(1), middleNeurons.GetLength(1)];
                        for (int y = 0; y < middleWeights.GetLength(1); y++)
                            for (int z = 0; z < middleWeights.GetLength(1); z++)
                                oldMiddleWeights[x][y, z] = middleWeights[x][y, z];

                    }

                    int[] binary = AIMath.NumToBin(Convert.ToInt32(educationArray[i, 0]), inputNeurons.Length);

                    //forward propagation
                    NeuralNetwork.ForwardPropagation(binary, inputNeurons, inputWeights, middleNeurons, middleWeights, middleBiases, outputNeurons, outputBiases, outputWeights, NeuralNetwork.GenerateDropOut());

                    int[] correctOutput = AIMath.StringParse(educationArray[i, 1]);

                    for (int j = 0; j < outputNeurons.Length; j++)
                    {
                        errorOut[j] = outputNeurons[j] - correctOutput[j]; //ошибка
                        deltaOut[j] = errorOut[j] * outputNeurons[j] * (1 - outputNeurons[j]); //дельта

                        for (int k = 0; k < middleNeurons.GetLength(1); k++)
                            outputWeights[k, j] -= middleNeurons[Parameters.Mlayers - 1, k] * deltaOut[j] * Parameters.learningRate;

                        outputBiases[j] -= deltaOut[j] * Parameters.learningRate;
                    }

                    for (int j = 0; j < middleNeurons.GetLength(1); j++)
                    {
                        for (int l = 0; l < outputNeurons.Length; l++)
                        {
                            errorMid[Parameters.Mlayers - 1, j] += deltaOut[l] * oldOutputWeights[j, l]; //ошибка
                        }

                        if (middleNeurons[Parameters.Mlayers - 1, j] == 0)
                            deltaMid[Parameters.Mlayers - 1, j] = 0;
                        else
                            deltaMid[Parameters.Mlayers - 1, j] = errorMid[Parameters.Mlayers - 1, j] * middleNeurons[Parameters.Mlayers - 1, j] * (1 - middleNeurons[Parameters.Mlayers - 1, j]); //дельта

                        for (int k = 0; k < outputNeurons.Length; k++)
                            middleWeights[Parameters.Mlayers - 2][k, j] -= middleNeurons[Parameters.Mlayers - 2, k] * deltaMid[Parameters.Mlayers - 1, j] * Parameters.learningRate;

                        middleBiases[Parameters.Mlayers - 1, j] -= deltaMid[Parameters.Mlayers - 1, j] * Parameters.learningRate;
                    }

                    for (int layer = Parameters.Mlayers - 2; layer >= 0; layer--)
                    {
                        int oldLayer = layer + 1;
                        for (int j = 0; j < middleNeurons.GetLength(1); j++)
                        {
                            for (int l = 0; l < middleNeurons.GetLength(1); l++)
                            {
                                errorMid[layer, j] += deltaMid[oldLayer, l] * oldMiddleWeights[oldLayer][j, l]; //ошибка
                            }

                            if (middleNeurons[layer, j] == 0)
                                deltaMid[layer, j] = 0;
                            else
                                deltaMid[layer, j] = errorMid[layer, j] * middleNeurons[layer, j] * (1 - middleNeurons[layer, j]); //дельта

                            for (int k = 0; k < outputNeurons.Length; k++)
                                middleWeights[Parameters.Mlayers - 2][k, j] -= middleNeurons[oldLayer, k] * deltaMid[layer, j] * Parameters.learningRate;

                            middleBiases[layer, j] -= deltaMid[layer, j] * Parameters.learningRate;
                        }
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
