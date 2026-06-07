using System.Globalization;
using System.Text.Json;
using Yocto_Roger.Yocto_Roger;
using Yocto_Roger.UI.GUI;
using static Yocto_Roger.IO.Auxiliary;
using static Yocto_Roger.UI.GUI.GUI;

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
    /// <summary>
    /// Main IO class, where contains all main function for work with salve/load
    /// </summary>
    public class MainIO(Parameters param, NeuralNetwork nN, NeuralNetworkState nNState)
    {
        private static readonly JsonSerializerOptions options =
            new()
            {
                WriteIndented = true
            };

        private readonly Parameters _param = param;
        /// <summary>
        /// object of NeuralNetwork class
        /// </summary>
        public NeuralNetwork _nN = nN;
        private readonly NeuralNetworkState _nNState = nNState;

        /// <summary>
        /// Saving the current Roger settings in the json file, which creating automatedly
        /// </summary>
        public void SaveRogerToJson(string? fileName = "roger")
        {

            Roger roger = new()
            {
                AIversion = _param.version,
                Passes = _param.passes,

                LearingRate = _param.learningRate,
                DropOutPercent = _param.DropOutPercent,

                InputNeuronsCount = _param.inputNeuronsCount,
                MiddleNeuronsCount = _param.middleNeuronsCount,
                OutputNeuronsCount = _param.outputNeuronsCount,

                Layers = _param.layers,
            };

            string jsonData = JsonSerializer.Serialize(roger, options);

            using StreamWriter writer = new(MakeFileSplitOnIndexIfExists("params", fileName));
            writer.Write(jsonData);
        }

        /// <summary>
        /// This method determines the type and parses Roger's file and provides an object in which all the data is in the form of strings, they will have to be converted to the required data types using the appropriate functions that seem to be in this class
        /// </summary>
        /// <returns>Roger class object. If happened any error, for example something with null, so it's returning an empty object of class Roger</returns>
        public Roger? LoadRoger()
        {

            if (!File.Exists(_param.roger2))
            {
                Send($"Roger file [{_param.roger2}] not found", MessageType.error);
                return null;
            }
            else // I made an else clause so that if the file does not exist, the code will not be executed further.
            {
                try
                {
                    if (LoadRogerFromJson() is Roger roger)
                        return roger;
                    else
                        return null;
                }
                catch (JsonException e)
                {
                    Send($"Failed to parse the json data: \n{e}", MessageType.error);
                    return null;
                }
            }
        }

        /// <summary>
        /// A class that will store data for loading/saving the neural network.
        /// The data is stored as strings, so the data from it must be initialized using a special function InitRogersData, written specifically to avoid initializing everything manually.
        /// Please note, both when working with this class and simply when viewing it, that the absence of data is indicated by String.Empty instead of null!!!
        /// </summary>
        public class Roger
        {
            /// <summary>
            /// Ai version
            /// </summary>
            public string? AIversion { get; set; }

            /// <summary>
            /// Passes
            /// </summary>
            public int Passes { get; set; }

            /// <summary>
            /// Learning rate
            /// </summary>
            public float LearingRate { get; set; }
            /// <summary>
            /// Drop out percent
            /// </summary>
            public float DropOutPercent { get; set; }

            /// <summary>
            /// Input neurons count
            /// </summary>
            public int InputNeuronsCount { get; set; }
            /// <summary>
            /// Middle neurons count
            /// </summary>
            public int MiddleNeuronsCount { get; set; }
            /// <summary>
            /// Output neurons count
            /// </summary>
            public int OutputNeuronsCount { get; set; }

            /// <summary>
            /// Layers
            /// </summary>
            public int Layers { get; set; }
            /// <summary>
            /// MLayers
            /// </summary>
            public int MLayers { get; set; }
        }

        /// <summary>
        /// Returns an object of the Roger class with all the necessary data to load the neural network.
        /// </summary>
        private Roger? LoadRogerFromJson()
        {
            using JsonDocument document = JsonDocument.Parse(File.ReadAllText(_param.roger2));
            JsonElement root = document.RootElement;

            Roger? roger = JsonSerializer.Deserialize<Roger>(File.ReadAllText(_param.roger2));


            return roger ?? null;
        }

        /// <summary>
        /// Attempts to create a file in the same directory; if such a file already exists, it adds an index of attempts until it reaches the index where there is no file with that name.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="extension">File extension (without period)</param>
        public static string MakeFileSplitOnIndexIfExists(string extension, string? fileName = "roger")
        {
            int index = 0;

            string filenameWithIndex;
            do
            {
                if (index == 0)
                    filenameWithIndex = $"{fileName}.{extension}";
                else
                    filenameWithIndex = $"{fileName}{index}.{extension}";
                index++;
            }
            while (File.Exists(filenameWithIndex));

            FileStream fs = File.Create(filenameWithIndex);
            fs.Close();

            return filenameWithIndex;
        }

        //*************NEURAL NETWORK SECTION********************

        /// <summary>
        /// Transforming the values from class NeuralNetworkState to needed types, and initializing it where it needs
        /// </summary>
        /// <param name="nN"></param>
        /// <param name="isNeededToInitEducationArray"></param>
        public void InitNeuralNetwork(NeuralNetworkState? nN, bool isNeededToInitEducationArray = false)
        {
            if (isNeededToInitEducationArray)
                if (nN?.EducationArray is not null)
                    _nN.educationArray = ReadMatrixFromArray([.. nN.EducationArray.Split(';').Select(s => int.Parse(s, CultureInfo.InvariantCulture!))]);

            _nN.inputNeurons = nN?.InputNeurons?.Split(';').Select(s => int.Parse(s, CultureInfo.InvariantCulture)).ToArray();
            _nN.middleNeurons = ReadMatrixFromDoublesArray((nN?.MiddleNeurons is not null) ? [.. nN.MiddleNeurons.Split(';').Select(s => double.Parse(s, CultureInfo.InvariantCulture))] : null);
            _nN.outputNeurons = nN?.OutputNeurons?.Split(';').Select(s => double.Parse(s, CultureInfo.InvariantCulture)).ToArray();

            // If null - Values by default
            _param.inputNeuronsCount = nN?.InputNeuronsCount ?? 14;
            _param.middleNeuronsCount = nN?.MiddleNeuronsCount ?? 16;
            _param.outputNeuronsCount = nN?.OutputNeuronsCount ?? 8;

            _nN.inputWeights = ReadMatrixFromDoublesArray((nN?.InputWeights is not null) ? [.. nN.InputWeights!.Split(';').Select(s => double.Parse(s, CultureInfo.InvariantCulture))] : null);
            _nN.middleWeights = ReadJaggedMatrixFromArray((nN?.MiddleWeights is not null) ? [.. nN.MiddleWeights!.Split(';', StringSplitOptions.RemoveEmptyEntries).Select(s => double.Parse(s, CultureInfo.InvariantCulture))] : null);
            _nN.outputWeights = ReadMatrixFromDoublesArray((nN?.OutputWeights is not null) ? [.. nN.OutputWeights!.Split(';').Select(s => double.Parse(s, CultureInfo.InvariantCulture))] : null);

            // If null - values by default
            _param.layers = nN?.Layers ?? 3;

            _nN.Mbias = ReadMatrixFromDoublesArray((nN?.Mbias != null) ? [.. nN.Mbias!.Split(';').Select(s => double.Parse(s, CultureInfo.InvariantCulture))] : null);
            _nN.Obias = nN?.Obias?.Split(';').Select(s => double.Parse(s, CultureInfo.InvariantCulture)).ToArray();

        }
        /// <summary>
        /// Saving neural network state to json file.
        /// </summary>
        /// <param name="nN"></param>
        /// <param name="pathToDirectoryToSave"></param>
        public static void SaveNeuralNetworkStateToJson(NeuralNetworkState nN, string pathToDirectoryToSave)
        {
            string json = JsonSerializer.Serialize(nN, options);

            string path = MakeFileSplitOnIndexIfExists("json", Path.Combine(pathToDirectoryToSave, "NeuralNetworkState"));

            File.WriteAllText(path, json);
        }
        /// <summary>
        /// Fixing the neural network state
        /// </summary>
        /// <param name="isNeedToFixTheEducationArray">recommended to set it to false</param>
        /// <returns></returns>
        public NeuralNetworkState FixTheStateOfNeuralNetwork(bool isNeedToFixTheEducationArray)
        {
            NeuralNetworkState nN = new()
            {
                EducationArray = (isNeedToFixTheEducationArray) ? BuildStringMatrix(_nN.educationArray) ?? null : String.Empty,
                InputNeurons = BuildStringArray(_nN.inputNeurons) ?? null,
                MiddleNeurons = BuildStringMatrix(_nN.middleNeurons) ?? null,
                OutputNeurons = BuildStringArray(_nN.outputNeurons) ?? null,

                // It can't be null (i hope...), so i don't paste "?? null" here
                InputNeuronsCount = _param.inputNeuronsCount,
                MiddleNeuronsCount = _param.middleNeuronsCount,
                OutputNeuronsCount = _param.outputNeuronsCount,

                InputWeights = BuildStringArray(_nN.inputWeights) ?? null,
                MiddleWeights = BuildStringJaggedMatrix(_nN.middleWeights) ?? null,
                OutputWeights = BuildStringArray(_nN.outputWeights) ?? null,

                // And here too
                Layers = _param.layers,
                
                Obias = BuildStringMatrix(_nN.Mbias) ?? null,
                Mbias = BuildStringArray(_nN.Obias) ?? null,
            };

            return nN;
        }

        /// <summary>
        /// Loading values from the file 
        /// </summary>
        /// <param name="absolute_path">Absolute path to file</param>
        /// <returns></returns>
        public NeuralNetworkState? LoadNeuralNetworkStateFromJson(string absolute_path)
        {
            NeuralNetworkState? nNState = JsonSerializer.Deserialize<NeuralNetworkState>(File.ReadAllText(absolute_path));

            return nNState;
        }
    }
}
