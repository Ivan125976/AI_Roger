namespace Yocto_Roger
{
    public class Parameters
    {
        public static string version = "2.2.0";
        public static char revision = ' ';
        public static bool isDebug = false;
        public static int passes = 500;
        public static float learningRate = 0.02f;
        public static float DropOutPercent = 3.0f;
        public static string knowledgeFile = "knowledge.know";
        public static string? roger2 = "";
        public static int inputNeuronsCount = 14;
        public static int middleNeuronsCount = 16;
        public static int outputNeuronsCount = 8;
        public static int layers = 3;
        public static int Mlayers = layers - 2; //Mlayers - Count of middle layers.
    }
}
