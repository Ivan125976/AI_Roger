using IniParser;
using IniParser.Model;
using System.Globalization;
using System.Text.Json;
using Yocto_Roger;

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
            using (StreamWriter writer = new(MakeFileSplitOnIndexIfExists("roger", "roger")))
            {
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

                 [layers]
                 Layers = {Parameters.layers}
                 Mlayers = {Parameters.Mlayers}
                 """);
            }
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

            using (StreamWriter writer = new(MakeFileSplitOnIndexIfExists("roger", "json")))
                writer.Write(jsonData);
        }

        /// <summary>
        /// This method determines the type and parses Roger's file and provides an object in which all the data is in the form of strings, they will have to be converted to the required data types using the appropriate functions that seem to be in this class
        /// </summary>
        /// <returns>Roger class object. If happened any error, for example something with null, so it's returning an empty object of class Roger</returns>
        public static Roger? LoadRoger()
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

                    Layers = Convert.ToInt32(data["layers"]["layers"]),
                    MLayers = Convert.ToInt32(data["layers"]["mLayers"])
                };

            return roger;
        }

        /// <summary>
        /// Returns an object of the Roger class with all the necessary data to load the neural network.
        /// </summary>
        private static Roger? LoadRogerFromJson()
        {
            using JsonDocument document = JsonDocument.Parse(File.ReadAllText(Parameters.roger2));
            JsonElement root = document.RootElement;

            Roger roger = JsonSerializer.Deserialize<Roger>(File.ReadAllText(Parameters.roger2));

            return roger ?? null;
        }

        /// <summary>
        /// Attempts to create a file in the same directory; if such a file already exists, it adds an index of attempts until it reaches the index where there is no file with that name.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="extension">File extension (without period)</param>
        public static string MakeFileSplitOnIndexIfExists(string filename, string extension)
        {
            int index = 0;

            string filenameWithIndex;
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
            public string? educationArray { get; set; }

            public string? inputNeurons { get; set; }
            public string? middleNeurons { get; set; }
            public string? outputNeurons { get; set; }

            public int inputNeuronsCount { get; set; }
            public int middleNeuronsCount { get; set; }
            public int outputNeuronsCount { get; set; }

            public string? inputWeights { get; set; }
            public string? middleWeights { get; set; }
            public string? outputWeights { get; set; }

            public int Layers { get; set; }
            public int MLayers { get; set; }

            public string? Mbias { get; set; }
            public string? Obias { get; set; }
        }

        public static void InitNeuralNetwork(NeuralNetworkState nN, bool isNeededToInitEducationArray = false)
        {
            if (isNeededToInitEducationArray)
                if (nN?.educationArray is not null)
                    NeuralNetwork.educationArray = Auxiliary.ReadMatrixFromArray([.. nN.educationArray.Split(';').Select(s => int.Parse(s, CultureInfo.InvariantCulture!))]);

            NeuralNetwork.inputNeurons = nN?.inputNeurons?.Split(';').Select(s => int.Parse(s, CultureInfo.InvariantCulture)).ToArray();
            NeuralNetwork.middleNeurons = Auxiliary.ReadMatrixFromDoublesArray((nN?.middleNeurons is not null) ? [.. nN.middleNeurons.Split(';').Select(s => double.Parse(s, CultureInfo.InvariantCulture))] : null);
            NeuralNetwork.outputNeurons = nN?.outputNeurons?.Split(';').Select(s => double.Parse(s, CultureInfo.InvariantCulture)).ToArray();

            // Если null - значения по умолчанию
            Parameters.inputNeuronsCount = nN?.inputNeuronsCount ?? 14;
            Parameters.middleNeuronsCount = nN?.middleNeuronsCount ?? 16;
            Parameters.outputNeuronsCount = nN?.outputNeuronsCount ?? 8;

            NeuralNetwork.inputWeights = Auxiliary.ReadMatrixFromDoublesArray((nN?.inputWeights is not null) ? [.. nN.inputWeights!.Split(';').Select(s => double.Parse(s, CultureInfo.InvariantCulture))] : null);
            NeuralNetwork.middleWeights = Auxiliary.ReadJaggedMatrixFromArray((nN?.middleWeights is not null) ? [.. nN.middleWeights!.Split(';').Select(s => double.Parse(s, CultureInfo.InvariantCulture))] : null );
            NeuralNetwork.outputWeights = Auxiliary.ReadMatrixFromDoublesArray((nN?.outputWeights is not null) ? [.. nN.outputWeights!.Split(';').Select(s => double.Parse(s, CultureInfo.InvariantCulture))] : null);

            // Значения по умолчанию в случае null или пустого элемента
            Parameters.layers = nN?.Layers ?? 3;
            Parameters.Mlayers = nN?.MLayers ?? 1; // Layers -1

            NeuralNetwork.Mbias = Auxiliary.ReadMatrixFromDoublesArray((nN?.Mbias != null) ? [..nN.Mbias!.Split(';').Select(s => double.Parse(s, CultureInfo.InvariantCulture))] : null);
            NeuralNetwork.Obias = nN?.Obias?.Split(';').Select(s => double.Parse(s, CultureInfo.InvariantCulture)).ToArray();
                 
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
                educationArray = (isNeedToFixTheEducationArray) ? Auxiliary.BuildStringMatrix(NeuralNetwork.educationArray) ?? null : String.Empty,
                inputNeurons = Auxiliary.BuildStringArray(NeuralNetwork.inputNeurons) ?? null,
                middleNeurons = Auxiliary.BuildStringMatrix(NeuralNetwork.middleNeurons) ?? null,
                outputNeurons = Auxiliary.BuildStringArray(NeuralNetwork.outputNeurons) ?? null,

                inputNeuronsCount = Parameters.inputNeuronsCount,
                middleNeuronsCount = Parameters.middleNeuronsCount,
                outputNeuronsCount = Parameters.outputNeuronsCount,

                inputWeights = Auxiliary.BuildStringArray(NeuralNetwork.inputWeights) ?? null,
                middleWeights = Auxiliary.BuildStringJaggedMatrix(NeuralNetwork.middleWeights) ?? null,
                outputWeights = Auxiliary.BuildStringArray(NeuralNetwork.outputWeights) ?? null,

                Layers = Parameters.layers,
                MLayers = Parameters.Mlayers,

                Obias = Auxiliary.BuildStringMatrix(NeuralNetwork.Mbias) ?? null,
                Mbias = Auxiliary.BuildStringArray(NeuralNetwork.Obias) ?? null,
            };

            return nN;
        }
        public static NeuralNetworkState LoadNeuralNetworkStateFromJson(string absolute_path)
        {
            NeuralNetworkState nN = new();

            using (JsonDocument doc = JsonDocument.Parse(File.ReadAllText(absolute_path)))
            {
                var root = doc.RootElement;

                nN.educationArray = root.GetProperty("educationArray").GetString() ?? null;

                nN.inputNeurons = root.GetProperty("inputNeurons").GetString() ?? null;
                nN.middleNeurons = root.GetProperty("middleNeurons").GetString() ?? null;
                nN.outputNeurons = root.GetProperty("outputNeurons").GetString() ?? null;

                nN.inputNeuronsCount = root.GetProperty("inputNeuronsCount").GetInt32();
                nN.middleNeuronsCount = root.GetProperty("middleNeuronsCount").GetInt32();
                nN.outputNeuronsCount = root.GetProperty("outputNeuronsCount").GetInt32();

                nN.inputWeights = root.GetProperty("inputWeights").GetString() ?? null;
                nN.middleWeights = root.GetProperty("middleWeights").GetString() ?? null;
                nN.outputWeights = root.GetProperty("outputWeights").GetString() ?? null;

                nN.Obias = root.GetProperty("Obias").GetString() ?? null;
                nN.Mbias = root.GetProperty("Mbias").GetString() ?? null;
            }

            return nN;
        }
    }
}
