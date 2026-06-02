namespace Yocto_Roger.Yocto_Roger
{
    /* 
Yocto Roger ;)
*****************
*Emotion Corp ;)*
*****************
Copyright 2025-2026 Emotion Corp.
Internal AIMath lib
*/
    internal class AIMath
    {
        static public Random rand = new();

        public static double Tanh(double value) //activation
        {
            return Math.Tanh(value);
        }
        public static int BinToNum(double[] obj) //binary to int
        {
            int result = 0;
            for (int i = 0; i < obj.Length; i++)
                if (obj[i] != 0.0)
                    result += 1 << 7 - i;
            if (Parameters.isDebug)
                Console.WriteLine("binToNum -> " + result);
            return result;
        }

        /// <summary>
        /// Converting int to binary
        /// </summary>
        /// <param name="obj">The int32 number to be converted</param>
        /// <param name="bits">Number of bits in a binary array</param>
        /// <returns></returns>
        public static int[] NumToBin(int obj, int bits)
        {
            if (Parameters.isDebug)
                Console.WriteLine("numToBin (" + bits + " bits) -> ");
            int[] bin = new int[bits];
            for (int i = 0; i < bits; i++)
            {
                bin[bits - 1 - i] = obj >> i & 1;
                if (Parameters.isDebug)
                    Console.Write(bin[bits - 1 - i] + " ");
            }
            return bin;
        }

        public static int[] StringParse(string obj, char symbol)
        {
            string[] strings = obj.Split(symbol);
            int[] parsedArray = new int[strings.Length];
            for (int i = 0; i < parsedArray.Length; i++)
                parsedArray[i] = Convert.ToInt32(strings[i]);
            return parsedArray;
        }
        public static double[] SplitOutputEducation(string obj)
        {
            string[] parsedObj = obj.Split(' ');
            double[] parsedArray = new double[Parameters.outputNeuronsCount];
            for (int i = 0; i < parsedArray.Length; i++)
                parsedArray[i] = Convert.ToDouble(parsedObj[i]);
            return parsedArray;
        }
    }
}
