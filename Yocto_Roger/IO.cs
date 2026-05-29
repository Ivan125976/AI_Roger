using IniParser;
using IniParser.Model;
using System.Text.Json;

namespace Yocto_Roger
{
    /* 
Yocto Roger ;)
*****************
*Emotion Corp ;)*
*****************
Copyright 2025-2026 Emotion Corp.
Internal I/O lib
*/
    internal static class IO
    {
        public static void SaveRoger()
        {

            using StreamWriter writer = new(MakeFileSplitOnIndexIfExists("roger", "roger"));

            writer.Write(
                $"""
                 [roger]
                 AIversion = {Parameters.version}
                 passes = {Parameters.passes}
                 learningRate = {Parameters.learningRate}
                 DropOutPercent = {Parameters.DropOutPercent}

                 [neurons]
                 inputNeuronsCount = {Parameters.inputNeuronsCount}
                 middleNeuronsCount = {Parameters.middleNeuronsCount}
                 outputNeuronsCount = {Parameters.outputNeuronsCount}

                 [biases]
                 Layers = {Parameters.layers}
                 Mlayers = {Parameters.Mlayers}
                 """);
        }

        public static void SaveRogerToJson()
        {
            Roger roger = new()
            {
                AIversion = Parameters.version,
                IsDebug = Parameters.isDebug,
                Passes = Parameters.passes,

                LearingRate = Parameters.learningRate,
                DropOutPercent = Parameters.DropOutPercent,

                InputNeuronsCount = Parameters.inputNeuronsCount,
                MiddleNeuronsCount = Parameters.outputNeuronsCount,
                OutputNeuronsCount = Parameters.outputNeuronsCount,

                Layers = Parameters.layers,
                MLayers = Parameters.Mlayers,
            };

            JsonSerializerOptions options = new() { WriteIndented = true };
            string jsonData = JsonSerializer.Serialize(roger, options);

            using StreamWriter writer = new(MakeFileSplitOnIndexIfExists("roger", "json"));
            writer.Write(jsonData);
        }

        /// <summary>
        /// This method determines the type and parses Roger's file and provides an object in which all the data is in the form of strings, they will have to be converted to the required data types using the appropriate functions that seem to be in this class
        /// </summary>
        /// <returns>Roger class object</returns>
        public static Roger LoadRoger()
        {
            if (!File.Exists(Parameters.roger2))
                UI.Send("Roger file not found", "error");
            else // I made an else clause so that if the file does not exist, the code will not be executed further.
            {
                switch (CheckFormat())
                {
                    case true: // Json
                        return LoadRogerFromJson();

                    case false: // Roger 
                        return LoadRogerFromRoger();
                }
            }
            return null; //It's a stub to keep the compiler from complaining. I have no idea how to fix it. I can fix it with GOTO, but damn... In theory, it's basically "unreachable code."
        }

        /// <summary>
        /// Checking the recording format
        /// </summary>
        /// <returns>true - json format, false - roger format</returns>
        private static bool? CheckFormat()
        {
            if (Parameters.roger2.EndsWith(".json"))
                return true;
            else if (Parameters.roger2.EndsWith(".roger") || Parameters.roger2.EndsWith(".roger2"))
                return false;

            else return null;
        }

        /// <summary>
        /// A class that will store data for loading/saving the neural network.
        /// The data is stored as strings, so the data from it must be initialized using a special function InitRogersData, written specifically to avoid initializing everything manually.
        /// Please note, both when working with this class and simply when viewing it, that the absence of data is indicated by String.Empty instead of null!!!
        /// </summary>
        public class Roger
        {
            public string AIversion { get; set; }

            public bool IsDebug { get; set; }
            public int Passes { get; set; }

            public float LearingRate { get; set; }
            public float DropOutPercent { get; set; }

            public int InputNeuronsCount { get; set; }
            public int MiddleNeuronsCount { get; set; }
            public int OutputNeuronsCount { get; set; }

            public int Layers { get; set; }
            public int MLayers { get; set; }
        }

        /// <summary>
        /// A function that returns a Roger class object with data extracted from a .roger file.
        /// </summary>
        private static Roger LoadRogerFromRoger()
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile(Parameters.roger2);

            Roger roger = new()
            {
                AIversion = data["roger"]["AIversion"],
                Passes = Convert.ToInt32(data["roger"]["passes"]),
                LearingRate = float.Parse(data["roger"]["learningRate"]),
                DropOutPercent = float.Parse(data["roger"]["DropOutPercent"]),

                InputNeuronsCount = Convert.ToInt32(data["neurons"]["inputNeuronsCount"]),
                MiddleNeuronsCount = Convert.ToInt32(data["neurons"]["middleNeuronsCount"]),
                OutputNeuronsCount = Convert.ToInt32(data["neurons"]["outputNeuronsCount"]),

                Layers = Convert.ToInt32(data["biases"]["layers"]),
                MLayers = Convert.ToInt32(data["biases"]["mLayers"])
            };

            return roger;
        }

        /// <summary>
        /// Returns an object of the Roger class with all the necessary data to load the neural network.
        /// </summary>
        private static Roger LoadRogerFromJson()
        {
            using JsonDocument document = JsonDocument.Parse(File.ReadAllText(Parameters.roger2));
            JsonElement root = document.RootElement;

            Roger roger = new()
            {
                AIversion = root.GetProperty("AIversion").GetString(),
                IsDebug = root.GetProperty("isDebug").GetBoolean(),
                Passes = root.GetProperty("Passes").GetInt32(),

                LearingRate = float.Parse(root.GetProperty("learningRate").GetString()), // Переделаю это говно
                DropOutPercent = float.Parse(root.GetProperty("learningRate").GetString()), // И вот это тоже

                InputNeuronsCount = root.GetProperty("inputNeuronsCount").GetInt32(),
                MiddleNeuronsCount = root.GetProperty("middleNeuronsCount").GetInt32(),
                OutputNeuronsCount = root.GetProperty("outputNeuronsCount").GetInt32(),

                Layers = root.GetProperty("Layers").GetInt32(),
                MLayers = root.GetProperty("Mlayers").GetInt32()
            };

            return roger;
        }

        /// <summary>
        /// Attempts to create a file in the same directory; if such a file already exists, it adds an index of attempts until it reaches the index where there is no file with that name.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="extension">File extension (without period)</param>
        public static string MakeFileSplitOnIndexIfExists(string filename, string extension)
        {
            string filenameWithIndex = filename;
            int index = 0;

            do
            {
                if (index == 0)
                    filenameWithIndex = $"{filename}.{extension}";
                else
                    filenameWithIndex = $"{filename}{index}.{extension}";
                index++;
            }
            while (File.Exists(filenameWithIndex));

            FileStream fs = File.Create(filenameWithIndex);
            fs.Close();

            return filenameWithIndex;
        }
    }
}
