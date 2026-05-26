namespace Yocto_Roger
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

        public static double Tanh(double value) //активация
        {
            return Math.Tanh(value);
        }
        public static int BinToNum(double[] obj) //двиучное в десятичное
        {
            int result = 0;
            for (int i = 0; i < obj.Length; i++)
                if (obj[i] != 0.0)
                    result += 1 << (7 - i);
            if (Parameters.isDebug)
                Console.WriteLine("binToNum -> " + result);
            return result;
        }
        
        /// <summary>
        /// Конвертация десятичного числа в двоичное
        /// </summary>
        /// <param name="obj">Десятичное число которое нужно конвертировать</param>
        /// <param name="bits">Количество бит бинарного массива</param>
        /// <returns></returns>
        public static int[] NumToBin(int obj, int bits)
        {
            if (Parameters.isDebug)
                Console.WriteLine("numToBin (" + bits + " bits) -> ");
            int[] bin = new int[bits];
            for (int i = 0; i < bits; i++)
            {
                bin[bits - 1 - i] = (obj >> i) & 1;
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
