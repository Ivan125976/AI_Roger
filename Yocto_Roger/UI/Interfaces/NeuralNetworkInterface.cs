using System.Globalization;
using Yocto_Roger.IO;
using Yocto_Roger.RogerCore;
using Yocto_Roger.UI.CUI;
using MemoryPack;
#if DEBUG
using Newtonsoft.Json;
#endif
using static Yocto_Roger.UI.CUI.CUI;

namespace Yocto_Roger.UI.Interfaces
{
    /// <summary>
    /// NeuralNetwork manual interface
    /// </summary>
    public class NeuralNetworkInterface(Parameters param, MainIO io, MainMenuInterface mainMenuInterface, NeuralNetwork neuralNetwork) : IUserInterface
    {
        private readonly Parameters _param = param;
        private readonly MainIO _io = io;
        private readonly MainMenuInterface _mainMenuInterface = mainMenuInterface;

        /// <summary>
        /// object of NeuralNetwork class
        /// </summary>
        public NeuralNetwork _neuralNetwork = neuralNetwork;

        /// <summary>
        /// Calling up the NeuralNetwork manual interface
        /// </summary>
        public void StartInterface()
        {
            Console.CursorVisible = true;
            while (true)
            {
                Console.Clear();
                Send("Enter \"save\"  to fix the state of neural network in the file, for load at this point later. Or \"exit\" to exit to main menu", MessageType.warning);
                Console.WriteLine($"Roger have {_param.inputNeuronsCount} input neurons, and {_param.outputNeuronsCount} output neurons." +
                    $"Write input format: <datain1>,<datain2>,<datain3>...");
                DrawLine(ConsoleColor.DarkGreen, "Welcome to Yocto Roger v2.2! Manual interface", DateTime.Now.Date.ToString("dd/MM/yyyy"));
                Console.Write("\nInput>>>");
                string? userInputString = Console.ReadLine();
                if (!string.IsNullOrEmpty(userInputString))
                {

                    string[] userInputChecked = userInputString.Split(',');
                    if (userInputString == "exit")
                    {
                        _mainMenuInterface.StartInterface();
                    }
                    else if (userInputString == "save")
                    {
                        Console.Write("Please, enter the path to the directory, where we going to save the file (to this directory, simple press the enter): ");
                        string input = Console.ReadLine() ?? string.Empty;

                        try
                        {
                            if (input is string path && !string.IsNullOrEmpty(path) && Directory.Exists(path))
                            {
                                MainIO.SaveNeuralNetworkStateToBin(_io.FixTheStateOfNeuralNetwork(), path);
#if DEBUG
                                    string data = JsonConvert.SerializeObject(
                                        MemoryPackSerializer.Deserialize<NeuralNetworkState>(File.ReadAllBytes(path)),
                                        Formatting.Indented);

                                    Console.WriteLine($"Saved data is: {data}");
                                    Console.WriteLine("Enter any button to continue");
                                    Console.ReadLine();
#endif
                            }

                            else if (input == string.Empty)
                            {
                                MainIO.SaveNeuralNetworkStateToBin(_io.FixTheStateOfNeuralNetwork(), Directory.GetCurrentDirectory());
#if DEBUG
                                    NeuralNetworkState data = MemoryPackSerializer.Deserialize<NeuralNetworkState>(File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "NeuralNetworkState.bin")))!;
                                    Console.WriteLine($"Saved data (in json) is: \n{JsonConvert.SerializeObject(data, Formatting.Indented)});");
                                    Console.WriteLine("Enter any button to continue");
                                    Console.ReadLine();
#endif
                            }
                            else
                                Send("Incorrect input (-_0)", MessageType.error);
                        }
                        catch (Exception e)
                        {
                            Send("Somethin' wrong with me, here's my exception: ", MessageType.error);
                            Console.WriteLine($"Error: {e}", ConsoleColor.Red);
                            Thread.Sleep(5000);
                        }
                    }
                    else if (userInputChecked.Length == _param.inputNeuronsCount)
                    {
                        int[] userInput = new int[_param.inputNeuronsCount];
                        for (int i = 0; i < userInput.Length; i++)
                            userInput[i] = Convert.ToInt32(userInputChecked[i], CultureInfo.InvariantCulture);
                        _neuralNetwork.ForwardPropagation(userInput, _neuralNetwork.inputNeurons!, _neuralNetwork.inputWeights!, _neuralNetwork.middleNeurons!,
                            _neuralNetwork.middleWeights!, _neuralNetwork.Mbias!, _neuralNetwork.outputNeurons!, _neuralNetwork.Obias!, _neuralNetwork.outputWeights!);
                        Console.Write("Output>>>");
                        for (int i = 0; i < _neuralNetwork.outputNeurons!.Length; i++)
                            Console.Write(_neuralNetwork.outputNeurons[i] + " ");
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                        Console.Clear();
                    }
                    else
                        Send("It looks like you entered the wrong amount of information for the neurons or made a mistake with the command. No worries — it happens.", MessageType.error);
                }
                else
                {
                    Send("Incorrect input (-_0)", MessageType.error);
                }
            }
        }
    }
}
