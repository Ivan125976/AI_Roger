using Yocto_Roger.RogerCore;
using Yocto_Roger.UI.CUI;
using static Yocto_Roger.Configuration.EngineVersion;
using static Yocto_Roger.UI.CUI.CUI;

namespace Yocto_Roger.UI.Interfaces
{

    /// <summary>
    /// MainMenu interface
    /// </summary>
    public class MainMenuInterface(SettingsInterface settings, NeuralNetwork roger, UpdateManagerInterface updateManager) : IUserInterface
    {
        private readonly SettingsInterface _settingsInterface = settings;
        private readonly UpdateManagerInterface _updateManagerInterface = updateManager;

        /// <summary>
        /// object of NeuralNetwork class
        /// </summary>
        public NeuralNetwork _roger = roger;

        /// <summary>
        /// Calling up the main menu
        /// </summary>
        public void StartInterface()
        {
            int i = 0;
            while (true)
            {
                Console.Clear();
#if RELEASE
                DrawLine(ConsoleColor.DarkMagenta, $"Welcome to the RogerHubEngine! v.{majorVersion}.{minorVersion}.{patchVersion}{revision}", DateTime.Now.Date.ToString("dd/MM/yyyy"));
#elif DEBUG
                DrawLine(ConsoleColor.DarkMagenta, $"Welcome to the RogerHubEngine! v.{majorVersion}.{minorVersion}.{patchVersion}{revision} >DEBUG BUILD<", DateTime.Now.Date.ToString("dd/MM/yyyy"));
#endif
                Send("This project is still in the development stage.", MessageType.warning);
                Console.Write("""
                    
                    1. Start Roger in training mode
                    2. Load your roger (neural network) from the file
                    3. Options for training mode...
                    4. RRNNs settings...
                    5. Update manager...
                    6. About...
                    7. Exit from RogerHub 
                    >>> 
                    """);
                if (int.TryParse(Console.ReadLine(), out int value))
                {
                    switch (value)
                    {
                        case 1:
                            Console.WriteLine("Starting Roger...");
                            _roger.StartAI(0);
                            break;

                        case 2:
                            _roger.StartAI(1);
                            break;

                        case 3:
                            _settingsInterface.StartInterface();
                            break;

                        case 4:
                            Send("This version of the engine does not yet support the RRNNs protocol :(", MessageType.error);
                            break;

                        case 5:
                           _updateManagerInterface.StartInterface();
                            break;

                        case 6:
                            ASCIIDraw.Logo(false);
                            Console.WriteLine($" Github: https://github.com/Ivan125976/AI_Roger\n\n Authors: \n Axolotl512 - AI and RogerHubEngine \n d3ath-script - RRNNs, IO and compiling \n\n RogerHubEngine v.{majorVersion}.{minorVersion}.{patchVersion}{revision}\n" +
                                " RogerCore v2.2 \n RRNNs isn't ready \n OpenRB isn't ready \n\n Press any key to continue ");
                            Console.ReadKey();
                            break;

                        case 7:
                            Environment.Exit(0);
                            break;

                        default:
                            switch (i++)
                            {
                                case 0:
                                    Console.WriteLine("What?");
                                    break;
                                case 1:
                                    Console.WriteLine("I dont understand... (0-0)");
                                    break;
                                case 2:
                                    Console.WriteLine("Again?");
                                    break;
                                case 3:
                                    Console.WriteLine("PLEASE STOP!!!");
                                    break;
                                case 4:
                                    Console.WriteLine("I'm going disconnect...");
                                    Thread.Sleep(300);
                                    Console.WriteLine("Bye ;(");
                                    Thread.Sleep(1000);
                                    Environment.Exit(0);
                                    break;
                            }
                            break;
                    }
                }
                else
                    Send("Incorrect input >:(", MessageType.error);
            }
        }
    }
}
