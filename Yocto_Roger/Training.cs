namespace Yocto_Roger
{
    public class Training
    {
        public static void Education(ref int[] inputNeurons, ref double[,] middleNeurons, ref double[] outputNeurons,ref double[,] inputWeights, 
            ref double[][,] middleWeights, ref double[,] outputWeights, ref double[,] middleBiases, ref double[] outputBiases, int[,] educationArray) //обучение с поддержкой DropOut
        {
            double[,] errorMid = new double[middleNeurons.GetLength(0), middleNeurons.GetLength(1)];
            double[,] deltaMid = new double[middleNeurons.GetLength(0), middleNeurons.GetLength(1)];
            double[] errorOut = new double[outputNeurons.Length];
            double[] deltaOut = new double[outputNeurons.Length];
            //double[,] oldWeights = new double[NeuralNetwork.middleNeurons.GetLength(1), NeuralNetwork.outputNeurons.Length];
            
            for (int z = 0; z < Parameters.passes; z++)
            {
                for (int i = 0; i < educationArray.GetLength(0); i++)
                {
                    Array.Clear(errorMid, 0, errorMid.Length);
                    Array.Clear(errorOut, 0, errorOut.Length);
                    Array.Clear(deltaMid, 0, deltaMid.Length);
                    Array.Clear(deltaOut, 0, deltaOut.Length);

                    //for (int x = 0; x < NeuralNetwork.weights2.GetLength(0); x++)
                    //for (int y = 0; y < NeuralNetwork.weights2.GetLength(1); y++)
                    //oldWeights[x, y] = NeuralNetwork.weights2[x, y];

                    int[] binary = AIMath.NumToBin(educationArray[i,0], inputNeurons.Length);

                    //forward propagation
                    NeuralNetwork.ForwardPropagation(binary, inputNeurons, inputWeights, middleNeurons, middleWeights, middleBiases, outputNeurons, outputBiases, outputWeights, NeuralNetwork.GenerateDropOut());

                    for (int j = 0; j < outputNeurons.Length; j++)
                    {
                        errorOut[j] = outputNeurons[j] - binary[j]; //ошибка
                        deltaOut[j] = errorOut[j] * outputNeurons[j] * (1 - outputNeurons[j]); //дельта

                        for (int k = 0; k < middleNeurons.Length; k++) { }
                        //NeuralNetwork.weights2[k, j] -= NeuralNetwork.middleNeurons[k] * deltaOut[j] * Parameters.learningRate;

                        //NeuralNetwork.bias2[j] -= deltaOut[j] * Parameters.learningRate;
                    }
                    for (int j = 0; j < middleNeurons.Length; j++)
                    {
                        for (int l = 0; l < outputNeurons.Length; l++)
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
