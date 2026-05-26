using System.Data;

namespace Yocto_Roger
{
    /* 
Yocto Roger ;)
*****************
*Emotion Corp ;)*
*****************
Copyright 2025-2026 Emotion Corp.
    Education With Teacher v1.1

    EducationWithTeacher, DropOut, multilayer
*/
    /// <summary>
    /// Алгоритм обучения
    /// </summary>
    /// <returns>Обученная нейросеть</returns>
    public class Training
    {
        public static void Education(ref int[] inputNeurons, ref double[,] middleNeurons, ref double[] outputNeurons, ref double[,] inputWeights,
            ref double[][,] middleWeights, ref double[,] outputWeights, ref double[,] middleBiases, ref double[] outputBiases, string[,] educationArray)
        {
            string input, correctOutput;
            int[] parsedInput;
            double[] output;
            double[,] errorMid = new double[middleNeurons.GetLength(0), middleNeurons.GetLength(1)];
            double[,] deltaMid = new double[middleNeurons.GetLength(0), middleNeurons.GetLength(1)];
            double[] errorOut = new double[outputNeurons.Length];
            double[] deltaOut = new double[outputNeurons.Length];
            double[,] oldOutputWeights = new double[outputWeights.GetLength(0), outputWeights.GetLength(1)];
            double[][,] oldMiddleWeights = new double[Parameters.Mlayers - 1][,];
            for (int x = 0; x < Parameters.Mlayers - 1; x++)
                oldMiddleWeights[x] = new double[middleNeurons.GetLength(1), middleNeurons.GetLength(1)];

            for (int passes = 0; passes < Parameters.passes; passes++)
            {
                for (int i = 0; i < educationArray.GetLength(0); i++)
                {
                    input = "";

                    for (int l = 0; l < inputNeurons.Length; l++)
                        input += educationArray[i, l];

                    parsedInput = AIMath.StringParse(input, ',');

                    Array.Clear(errorMid, 0, errorMid.Length);
                    Array.Clear(errorOut, 0, errorOut.Length);
                    Array.Clear(deltaMid, 0, deltaMid.Length);
                    Array.Clear(deltaOut, 0, deltaOut.Length);

                    for (int x = 0; x < outputWeights.GetLength(0); x++)
                        for (int y = 0; y < outputWeights.GetLength(1); y++)
                            oldOutputWeights[x, y] = outputWeights[x, y];

                    for (int x = 0; x < Parameters.Mlayers - 1; x++)
                    {
                        for (int y = 0; y < middleWeights[x].GetLength(0); y++)
                            for (int z = 0; z < middleWeights[x].GetLength(1); z++)
                                oldMiddleWeights[x][y, z] = middleWeights[x][y, z];
                    }
                    
                    float[,] dropOut = NeuralNetwork.GenerateDropOut();

                    //forward propagation
                    NeuralNetwork.ForwardPropagation(parsedInput, inputNeurons, inputWeights, middleNeurons, middleWeights, middleBiases, outputNeurons, outputBiases, outputWeights, dropOut);

                    correctOutput = "";
                    for (int l = inputNeurons.Length; l < outputNeurons.Length + inputNeurons.Length; l++)
                        correctOutput += educationArray[i, l] + " ";

                    output = AIMath.SplitOutputEducation(correctOutput);

                    for (int j = 0; j < outputNeurons.Length; j++) //update output weights
                    {
                        errorOut[j] = outputNeurons[j] - output[j]; //ошибка
                        deltaOut[j] = errorOut[j] * (1 - outputNeurons[j] * outputNeurons[j]); //дельта

                        for (int k = 0; k < middleNeurons.GetLength(1); k++)
                            outputWeights[k, j] -= middleNeurons[Parameters.Mlayers - 1, k] * deltaOut[j] * Parameters.learningRate;

                        outputBiases[j] -= deltaOut[j] * Parameters.learningRate;
                    }

                    for (int j = 0; j < middleNeurons.GetLength(1); j++) //update output->middle weights
                    {
                        for (int l = 0; l < outputNeurons.Length; l++)
                            errorMid[Parameters.Mlayers - 1, j] += deltaOut[l] * oldOutputWeights[j, l]; //ошибка

                            deltaMid[Parameters.Mlayers - 1, j] = errorMid[Parameters.Mlayers - 1, j] * (1 - middleNeurons[Parameters.Mlayers - 1, j] * middleNeurons[Parameters.Mlayers - 1, j]); //дельта
                        deltaMid[Parameters.Mlayers - 1, j] *= dropOut[Parameters.Mlayers - 1, j];

                        if (Parameters.Mlayers > 1)
                            for (int k = 0; k < middleNeurons.GetLength(1); k++)
                                middleWeights[Parameters.Mlayers - 2][k, j] -=
                                    middleNeurons[Parameters.Mlayers - 2, k] *
                                    deltaMid[Parameters.Mlayers - 1, j] *
                                    Parameters.learningRate;
                        else
                            for (int k = 0; k < inputNeurons.Length; k++)
                                inputWeights[k, j] -=
                                    parsedInput[k] *
                                    deltaMid[0, j] *
                                    Parameters.learningRate;

                        middleBiases[Parameters.Mlayers - 1, j] -= deltaMid[Parameters.Mlayers - 1, j] * Parameters.learningRate;
                    }

                    if (Parameters.Mlayers > 1)
                        for (int layer = Parameters.Mlayers - 2; layer >= 0; layer--) //update middle->middle weights
                        {
                            int oldLayer = layer + 1;
                            for (int j = 0; j < middleNeurons.GetLength(1); j++)
                            {
                                for (int l = 0; l < middleNeurons.GetLength(1); l++)
                                    errorMid[layer, j] += deltaMid[oldLayer, l] * oldMiddleWeights[layer][j, l]; //ошибка

                                    deltaMid[layer, j] = errorMid[layer, j] * (1 - middleNeurons[layer, j] * middleNeurons[layer, j]); //дельта
                                    deltaMid[layer, j] *= dropOut[layer, j];
                                if (layer > 0)
                                {
                                    for (int k = 0; k < middleNeurons.GetLength(1); k++)
                                        middleWeights[layer - 1][k, j] -= middleNeurons[layer - 1, k] * deltaMid[layer, j] * Parameters.learningRate;
                                }

                                middleBiases[layer, j] -= deltaMid[layer, j] * Parameters.learningRate;
                            }
                        }

                    for (int j = 0; j < inputNeurons.Length; j++) //update middle->input weights
                    {
                        for (int k = 0; k < middleNeurons.GetLength(1); k++)
                            inputWeights[j, k] -= parsedInput[j] * deltaMid[0, k] * Parameters.learningRate;
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
