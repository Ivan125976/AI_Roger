using Yocto_Roger.Yocto_Roger;
using static Yocto_Roger.IO.MainIO;
using static Yocto_Roger.IO.Auxiliary;

namespace Yocto_Roger.UI
{
    internal class SetUp
    {

        /// <summary>
        /// Calling up the menu for setting values ​​and saving the file
        /// </summary>
        public static void SetUpMenu()
        {
            int i = 0;
            while (i == 0)
            {
                Console.Clear();
                Console.Write($"""
                                        RogerHubEngine Training Options
                                            
                                        0. Save your roger settings in the file 
                                        1. Load your roger setting from the file

                                        2. Count of input neurons...{Parameters.inputNeuronsCount}
                                        3. Count of middle neurons (all middle layers)...{Parameters.middleNeuronsCount}
                                        4. Count of output neurons...{Parameters.outputNeuronsCount}
                                        5. Count of Layers...{Parameters.layers}
                                        6. Knowledge file...{Parameters.knowledgeFile}
                                        7. DropOut sys percent...{Parameters.DropOutPercent}% (0% - disable DropOut)
                                        8. Learning Rate...{Parameters.learningRate}
                                        9. Passes...{Parameters.passes}
                                        10. Exit 
                                        >>> 
                                        """);
                string? choice = Console.ReadLine();
                switch (choice)
                {
                    case "0":
                        Console.Write("""                           
                            How do you want to save roger?

                            1. INI
                            2. Json (recommended)

                            >>>
                            """);
                        if (int.TryParse(Console.ReadLine(), out int userInputChecked))
                        {
                            switch (userInputChecked)
                            {
                                case 1:
                                    SaveRoger();
                                    break;

                                case 2:
                                    SaveRogerToJson();
                                    break;

                                default:
                                    UI.Send("What?", "error");
                                    break;
                            }

                            Console.WriteLine("Your roger saved in this directory, let's go, check it!\n If file was not created, write it in issues on our GitHub please ;)" +
                                "\n Press any key to continue");
                            Console.ReadKey();
                        }
                        else
                            UI.Send("Unknown input", "error");

                        break;

                    case "1":
                        Console.Write("Write an absolute path to the .roger of .json file please: ");

                        if (Console.ReadLine() is string input && !string.IsNullOrEmpty(input) && Path.Exists(input))
                        {
                            Parameters.roger2 = input;

                            InitRogersData(LoadRoger());
                        }
                        else
                            UI.Send("Incorrect input (-_0)", "error");
                        UI.Send("Maybe file which you typed, doesn't exists or you typed not string, please recheck this 2 factors");
                        break;

                    case "2":
                        Console.Clear();
                        Console.WriteLine("*INPUT NEURONS PARAMETER*");
                        Console.Write("INT32> Enter new count of input neurons (> 0)...");
                        if (int.TryParse(Console.ReadLine(), out int userInputChecked1))
                        {
                            if (userInputChecked1 > 0)
                                Parameters.inputNeuronsCount = userInputChecked1;
                            else
                                UI.Send("Value out of range.", "error");
                        }
                        break;

                    case "3":
                        Console.Clear();
                        Console.WriteLine("*MIDDLE NEURONS PARAMETER*");
                        Console.Write("INT32> Enter new count of middle neurons (> 0)...");
                        if (int.TryParse(Console.ReadLine(), out int userInputChecked2))
                        {
                            if (userInputChecked2 > 0)
                                Parameters.middleNeuronsCount = userInputChecked2;
                            else
                                UI.Send("Value out of range.", "error");
                        }
                        break;

                    case "4":
                        Console.Clear();
                        Console.WriteLine("*OUTPUT NEURONS PARAMETER*");
                        Console.Write("INT32> Enter new count of output neurons (> 0)...");
                        if (int.TryParse(Console.ReadLine(), out int userInputChecked3))
                        {
                            if (userInputChecked3 > 0)
                                Parameters.outputNeuronsCount = userInputChecked3;
                            else
                                UI.Send("Value out of range.", "error");
                        }
                        break;

                    case "5":
                        Console.Clear();
                        Console.WriteLine("*LAYERS PARAMETER*");
                        Console.Write("INT32> Enter new count of layers (> 2)...");
                        if (int.TryParse(Console.ReadLine(), out int layersCount))
                        {
                            if (layersCount > 2)
                            {
                                Parameters.layers = layersCount;
                                Parameters.Mlayers = layersCount - 2;
                            }
                            else
                                UI.Send("Value out of range.", "error");
                        }
                        break;

                    case "6":
                        Console.Clear();
                        Console.WriteLine("*KNOWLEDGE PARAMETER*");
                        Console.Write("STRING> Enter new knowledge file...");
                        string? file = Console.ReadLine();
                        if (File.Exists(file))
                            Parameters.knowledgeFile = file;
                        else
                            UI.Send("I couldn't find such a file :(", "error");
                        break;

                    case "7":
                        Console.Clear();
                        Console.WriteLine("*DROPOUT PERCENT PARAMETER*");
                        Console.Write("FLOAT> Enter new DropOut percent (0–70)... ");
                        if (int.TryParse(Console.ReadLine(), out int newDrop))
                        {
                            if (newDrop >= 0 && newDrop <= 70)
                                Parameters.DropOutPercent = newDrop;
                            else
                                UI.Send("Value out of range.", "error");
                        }
                        else
                            UI.Send("Invalid input.", "error");
                        break;

                    case "8":
                        Console.Clear();
                        Console.WriteLine("*LEARNING RATE PARAMETER*");
                        Console.Write("FLOAT> Enter new learning rate (0,0 – 1,0)... ");
                        if (float.TryParse(Console.ReadLine(), out float newLR))
                        {
                            if (newLR > 0 && newLR <= 1.0)
                                Parameters.learningRate = newLR;
                            else
                                UI.Send("Learning rate out of range.", "error");
                        }
                        else
                            UI.Send("Invalid input.", "error");
                        break;

                    case "9":
                        Console.Clear();
                        Console.WriteLine("*PASSES PARAMETER*");
                        Console.Write("INT32> Enter count of passes (> 0)... ");
                        if (int.TryParse(Console.ReadLine(), out int newPasses))
                        {
                            if (newPasses > 0)
                                Parameters.passes = newPasses;
                            else
                                UI.Send("Passes must be greater than zero.", "error");
                        }
                        else
                            UI.Send("Invalid input.", "error");
                        break;

                    case "10":
                        i++;
                        break;
                }
            }
        }
    }
}
