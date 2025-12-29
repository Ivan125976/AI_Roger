using System;
using System.Text;

namespace MyNN
{
    /*   ***************
     *Emotion Corp.*
     ***************
     Квекто-нейросеть!
     
     Перцептрон
     2- вход
     0- середина
     1- выход

     Привет, я Аксолотль, новичок в машинном обучении но уже неплохо получается.
     Привет, я Хамелеон, поддерживаю жизнеспособность всего обородувания и сетей.
     Привет, я Семечка, я пишу софт для Роджера. 
     Привет, я Грибочек, помогаю сделать тело для робота.*/
    internal class Program
    {
        static int[,] education = new int[10, 3];
        static double[] inputNeurons = new double[2];
        static double outNeuron = 0;
        static double[] weights1 = new double[2];
        static float learningRate = 0.3f;

        static void Main()
        {
            Console.WriteLine("Quecto-Roger v.1.1");
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;

            weights1 = setUpWeights(weights1.Length);

            Console.WriteLine("Hello! I'm a quecto-AI with 3 neurons! My name is Roger! Let's be friends?");

            while (true)
            {
                Console.Write("First value..........");
                inputNeurons[0] = Convert.ToDouble(Console.ReadLine());
                Console.Write("Second value.........");
                inputNeurons[1] = Convert.ToDouble(Console.ReadLine());

                double sum = calculationWeights(inputNeurons, weights1);
                outNeuron = sigmoida(sum);

                Console.ForegroundColor = ConsoleColor.Green;
                if (outNeuron > 0.5)
                    Console.WriteLine("This is 1");
                else if (outNeuron < 0.5)
                    Console.WriteLine("This is 0");
                else
                    Console.WriteLine("Sorry, i dont know 0_0");

                Console.ForegroundColor = ConsoleColor.White;

                bool foundInEducation = false;
                for (int i = 0; i < education.GetLength(0); i++)
                {
                    if (education[i, 0] == (int)inputNeurons[0] && education[i, 1] == (int)inputNeurons[1])
                    {
                        weights1 = educationWithTeacher(inputNeurons, outNeuron, education[i, 2], weights1);
                        foundInEducation = true;
                        break;
                    }
                }

                if (!foundInEducation)
                {
                    Console.Write("What answer did you expect?(0/1)......");
                    int expected = Convert.ToInt32(Console.ReadLine());
                    weights1 = educationWithTeacher(inputNeurons, outNeuron, expected, weights1);

                    for (int i = 0; i < education.GetLength(0); i++)
                    {
                        if (education[i, 0] == 0 && education[i, 1] == 0 && education[i, 2] == 0)
                        {
                            education[i, 0] = (int)inputNeurons[0];
                            education[i, 1] = (int)inputNeurons[1];
                            education[i, 2] = expected;
                            break;
                        }
                    }
                }
            }
        }

        static double[] setUpWeights(int x)
        {
            Random rand = new Random();
            double[] temp = new double[x];
            for (int i = 0; i < x; i++)
            {
                temp[i] = rand.NextDouble() - 0.5;
            }
            Console.WriteLine();
            return temp;
        }

        static double calculationWeights(double[] neurons, double[] weights)
        {
            double temp = 0;
            for (int i = 0; i < weights.Length; i++)
                temp += neurons[i] * weights[i];
            return temp;
        }

        static double sigmoida(double value)
        {
            return 1.0 / (1.0 + Math.Exp(-value));
        }

        static double[] educationWithTeacher(double[] inputs, double answer, int answerNeed, double[] weights)
        {
            double error = answerNeed - answer;
            double delta = answer * (1 - answer);

            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] += learningRate * delta * inputs[i];
            }
            return weights;
        }
    }
}