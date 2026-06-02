using IniParser;
using IniParser.Model;
using System.Globalization;
using System.Text.Json;
using Yocto_Roger.Yocto_Roger;
using static Yocto_Roger.UI.UI;

namespace Yocto_Roger.IO
{
    /* 
Yocto Roger ;)
*****************
*Emotion Corp ;)*
*****************
Copyright 2025-2026 Emotion Corp.
Internal I/O lib
*/
    internal static class MainIO
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
                Send("Roger file not found", "error");
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
            public string? AIversion { get; set; }

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

            Roger roger = JsonSerializer.Deserialize<Roger>(File.ReadAllText(Parameters.roger2));

            //Roger roger = new()
            //{
            //    AIversion = root.GetProperty("AIversion").GetString(),
            //    Passes = root.GetProperty("Passes").GetInt32(),

            //    LearingRate = float.Parse(root.GetProperty("LearingRate").GetString()),
            //    DropOutPercent = float.Parse(root.GetProperty("DropOutPercent").GetString()),

            //    InputNeuronsCount = root.GetProperty("InputNeuronsCount").GetInt32(),
            //    MiddleNeuronsCount = root.GetProperty("MiddleNeuronsCount").GetInt32(),
            //    OutputNeuronsCount = root.GetProperty("OutputNeuronsCount").GetInt32(),

            //    Layers = root.GetProperty("Layers").GetInt32(),
            //    MLayers = root.GetProperty("MLayers").GetInt32()
            //};

            return roger;
        }

        /// <summary>
        /// Attempts to create a file in the same directory; if such a file already exists, it adds an index of attempts until it reaches the index where there is no file with that name.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="extension">File extension (without period)</param>
        public static string MakeFileSplitOnIndexIfExists(string filename, string extension)
        {
            string filenameWithIndex;
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

        //*************NEURAL NETWORK SECTION********************
        public class NeuralNetworkState
        {
            public string? EducationArray { get; set; }

            public string InputNeurons { get; set; }
            public string MiddleNeurons { get; set; }
            public string OutputNeurons { get; set; }

            public int InputNeuronsCount { get; set; }
            public int MiddleNeuronsCount { get; set; }
            public int OutputNeuronsCount { get; set; }

            public string InputWeights { get; set; }
            public string MiddleWeights { get; set; }
            public string OutputWeights { get; set; }

            public int Layers { get; set; }
            public int MLayers { get; set; }

            public string Mbias { get; set; }
            public string Obias { get; set; }
        }

        public static void InitNeuralNetwork(NeuralNetworkState nN, bool isNeededToInitEducationArray = false)
        {
            if (isNeededToInitEducationArray)
                NeuralNetwork.educationArray = Auxiliary.ReadMatrixFromArray([.. nN.EducationArray.Split(';').Select(s => int.Parse(s, CultureInfo.InvariantCulture))]);

            NeuralNetwork.inputNeurons = [.. nN.InputNeurons.Split(';').Select(s => int.Parse(s, CultureInfo.InvariantCulture))];
            NeuralNetwork.middleNeurons = Auxiliary.ReadMatrixFromDoublesArray([.. nN.MiddleNeurons.Split(';').Select(s => double.Parse(s, CultureInfo.InvariantCulture))]);
            NeuralNetwork.outputNeurons = [.. nN.OutputNeurons.Split(';').Select(s => double.Parse(s, CultureInfo.InvariantCulture))];

            Parameters.inputNeuronsCount = nN.InputNeuronsCount;
            Parameters.middleNeuronsCount = nN.MiddleNeuronsCount;
            Parameters.outputNeuronsCount = nN.OutputNeuronsCount;

            NeuralNetwork.inputWeights = Auxiliary.ReadMatrixFromDoublesArray([.. nN.InputWeights.Split(';').Select(s => double.Parse(s, CultureInfo.InvariantCulture))]);
            NeuralNetwork.middleWeights = Auxiliary.ReadJaggedMatrixFromArray([.. nN.MiddleNeurons.Split(';').Select(s => double.Parse(s, CultureInfo.InvariantCulture))]);
            NeuralNetwork.outputWeights = Auxiliary.ReadMatrixFromDoublesArray([.. nN.OutputWeights.Split(';').Select(s => double.Parse(s, CultureInfo.InvariantCulture))]);

            Parameters.layers = nN.Layers;
            Parameters.Mlayers = nN.Layers;

            NeuralNetwork.Mbias = Auxiliary.ReadMatrixFromDoublesArray([.. nN.Mbias.Split(';').Select(s => double.Parse(s, CultureInfo.InvariantCulture))]);
            NeuralNetwork.Obias = [.. nN.Obias.Split(';').Select(s => double.Parse(s, CultureInfo.InvariantCulture))];
                 
        }

        public static void SaveNeuralNetworkStateToJson(NeuralNetworkState nN, string pathToDirectoryToSave)
        {
            string json = JsonSerializer.Serialize(nN, new JsonSerializerOptions { WriteIndented = true} );

            string path = MakeFileSplitOnIndexIfExists(Path.Combine(pathToDirectoryToSave, "NeuralNetworkState"), "json");

            File.WriteAllText(path, json);
        }

        public static NeuralNetworkState FixTheStateOfNeuralNetwork(bool isNeedToFixTheEducationArray)
        {
            NeuralNetworkState nN = new()
            {
                EducationArray = isNeedToFixTheEducationArray ? Auxiliary.BuildStringMatrix(NeuralNetwork.educationArray) ?? string.Empty : string.Empty,
                InputNeurons = Auxiliary.BuildStringArray(NeuralNetwork.inputNeurons) ?? string.Empty,
                MiddleNeurons = Auxiliary.BuildStringMatrix(NeuralNetwork.middleNeurons) ?? string.Empty,
                OutputNeurons = Auxiliary.BuildStringArray(NeuralNetwork.outputNeurons) ?? string.Empty,

                InputNeuronsCount = Parameters.inputNeuronsCount,
                MiddleNeuronsCount = Parameters.middleNeuronsCount,
                OutputNeuronsCount = Parameters.outputNeuronsCount,

                InputWeights = Auxiliary.BuildStringArray(NeuralNetwork.inputWeights) ?? string.Empty,
                MiddleWeights = Auxiliary.BuildStringJaggedMatrix(NeuralNetwork.middleWeights) ?? string.Empty,
                OutputWeights = Auxiliary.BuildStringArray(NeuralNetwork.outputWeights) ?? string.Empty,

                Layers = Parameters.layers,
                MLayers = Parameters.Mlayers,

                Obias = Auxiliary.BuildStringMatrix(NeuralNetwork.Mbias) ?? string.Empty,
                Mbias = Auxiliary.BuildStringArray(NeuralNetwork.Obias) ?? string.Empty,
            };

            return nN;
        }
        public static NeuralNetworkState LoadNeuralNetworkStateFromJson(string absolute_path)
        {
            NeuralNetworkState nN = new();

            using (JsonDocument doc = JsonDocument.Parse(File.ReadAllText(absolute_path)))
            {
                var root = doc.RootElement;

                nN.EducationArray = root.GetProperty("educationArray").GetString() ?? string.Empty;

                nN.InputNeurons = root.GetProperty("inputNeurons").GetString() ?? string.Empty;
                nN.MiddleNeurons = root.GetProperty("middleNeurons").GetString() ?? string.Empty;
                nN.OutputNeurons = root.GetProperty("outputNeurons").GetString() ?? string.Empty;

                nN.InputNeuronsCount = root.GetProperty("inputNeuronsCount").GetInt32();
                nN.MiddleNeuronsCount = root.GetProperty("middleNeuronsCount").GetInt32();
                nN.OutputNeuronsCount = root.GetProperty("outputNeuronsCount").GetInt32();

                nN.InputWeights = root.GetProperty("inputWeights").GetString() ?? string.Empty;
                nN.MiddleWeights = root.GetProperty("middleWeights").GetString() ?? string.Empty;
                nN.OutputWeights = root.GetProperty("outputWeights").GetString() ?? string.Empty;

                nN.Obias = root.GetProperty("Obias").GetString() ?? string.Empty;
                nN.Mbias = root.GetProperty("Mbias").GetString() ?? string.Empty;
            }

            return nN;
        }
    }
}
