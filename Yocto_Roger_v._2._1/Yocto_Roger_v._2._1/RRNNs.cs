using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yocto_Roger_v._2._1;

namespace Yocto_Roger_2._1
{

    /*
     Это  RRNNs!
     RRNNs - Robot Realtime Neuron Network System (Быстродейственная система нейронных сетей для роботов)
    ***********
    Написано семечкой из команды "Emotion Corp ;)" В помощь Аксолотлю
    ***********
    
    Это своего рода API (По сути он и есть) для взаимодействия с нейросетью, с помощью роботов (ну или же для плат на подобии Raspberry PI)
    Надеюсь код более менее понятен...

    Как всегда аксолотль, семечка, хамелеон, грибочек.
    По кирпичикам построим мечту!
     */
    internal class RRNNs
    {
        string date_time = System.DateTime.Now.ToString(); // will be used in the future (I hope i dont forget...)
        static private Dictionary<string, int> pins = new();

        /*
         Dictionary keys:
            for_time // local time. Example: 26.02.2026 21:04:10
            input_neurons
            output_neurons
         */

        static public void setUpPins()
        {
            int i = 0;

            while (true)
            {
                Console.Write(
                    "1 - Back to main menu\n" +
                    "2. Pin for displaying date and time (default: off)\n" +
                    "3. Pin for input neurons\n" +
                    "4. Pin for output neurons\n" +
                    ">>> ");

                if (int.TryParse(Console.ReadLine(), out int input))
                {
                    bool exit = false;

                    string[] answers =
                    {
                    "Please, enter the pin number: ",
                    "Please, enter the pin number again: ",
                    "Sorry, but please, enter the pin number again, i swear, its one last time!: "
                    };
                    switch (input)
                    {
                        case 1:
                            Console.Clear();
                            exit = true;
                            break;

                        case 2:
                            Console.Write(answers[i]);
                            pins["for_time"] = Convert.ToInt32(Console.ReadLine());
                            i++;
                            break;

                        case 3:
                            Console.Write(answers[i]);
                            pins["input_neurons"] =  Convert.ToInt32(Console.ReadLine());
                            i++;
                            break;

                        case 4:
                            Console.Write(answers[i]);
                            pins["output_neurons"] = Convert.ToInt32(Console.ReadLine());
                            i++;
                            break;

                        default:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Incorrect input >:(");
                            Console.ForegroundColor = ConsoleColor.Gray;
                            break;
                    }

                    if (exit == true)
                        break;

                    if (i > 2)
                        i = 0;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Incorrect input >:(");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
            }
        }
    }
}
