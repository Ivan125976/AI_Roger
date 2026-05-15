namespace Yocto_Roger
{
    public class Training
    {
        public static void Education() //обучение с поддержкой DropOut
        {
            double[] errorOut = new double[NeuralNetwork.outputNeurons.Length];
            double[,] errorMid = new double[NeuralNetwork.middleNeurons.GetLength(0), NeuralNetwork.middleNeurons.GetLength(1)];
            double[,] deltaMid = new double[NeuralNetwork.middleNeurons.GetLength(0), NeuralNetwork.middleNeurons.GetLength(1)];
            double[] deltaOut = new double[NeuralNetwork.outputNeurons.Length];
            //double[,] oldWeights = new double[NeuralNetwork.middleNeurons.GetLength(1), NeuralNetwork.outputNeurons.Length];

            for (int z = 0; z < Parameters.passes; z++)
            {
                float[,] dropOutMasks = NeuralNetwork.GenerateDropOut();

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

                    AIMath.numToBin(ref NeuralNetwork.inputNeurons, NeuralNetwork.educationArray[i, 0], NeuralNetwork.educationArray[i, 1]);
                    //NeuralNetwork.SumWeights(ref NeuralNetwork.weights1, ref NeuralNetwork.inputNeurons, ref NeuralNetwork.middleNeurons, NeuralNetwork.bias1);
                    for (int l = 0; l < NeuralNetwork.middleNeurons.Length; l++) //DropOut 
                    {
                        //NeuralNetwork.middleNeurons[l] *= dropOutMasks[l];
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

                        /*if (NeuralNetwork.middleNeurons[j] == 0)
                            deltaMid[j] = 0;
                        else
                            deltaMid[j] = errorMid[j] * NeuralNetwork.middleNeurons[j] * (1 - NeuralNetwork.middleNeurons[j]); //дельта */

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
