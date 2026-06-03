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

    /// <summary>
    /// Internal Math lib for Roger
    /// </summary>
    public class RogerMath(Parameters param)
    {
        private readonly Parameters _param = param;

        /// <summary>
        /// System Random
        /// </summary>
        public static readonly Random rand = new();

        /// <summary>
        /// Tanh Activation
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns></returns>
        public static double Tanh(double value)
        {
            return Math.Tanh(value);
        }

        /// <summary>
        /// Splitting a string into parts using a symbol
        /// </summary>
        /// <param name="obj">The string to be splitted</param>
        /// <param name="symbol">The character by which the string will be splitted</param>
        /// <returns></returns>
        public static int[] StringParse(string obj, char symbol)
        {
            string[] strings = obj.Split(symbol);
            int[] parsedArray = new int[strings.Length];
            for (int i = 0; i < parsedArray.Length; i++)
                parsedArray[i] = Convert.ToInt32(strings[i]);
            return parsedArray;
        }

        /// <summary>
        /// TODO: Delete
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public double[] SplitOutputEducation(string obj)
        {
            string[] parsedObj = obj.Split(' ');
            double[] parsedArray = new double[_param.outputNeuronsCount];
            for (int i = 0; i < parsedArray.Length; i++)
                parsedArray[i] = Convert.ToDouble(parsedObj[i]);
            return parsedArray;
        }
    }
}
