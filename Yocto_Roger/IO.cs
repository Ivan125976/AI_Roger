using IniParser;
using IniParser.Model;
using System.Globalization;
using System.Text;
using System.Text.Json;
using static Yocto_Roger.Auxiliary;

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
            string fileName;
            int index = 0;

            do
            {
                if (index == 0)
                    fileName = $"roger.roger";
                else
                    fileName = $"roger{index}.roger";
                index++;
            }
            while (File.Exists(fileName));

            using StreamWriter writer = new(fileName);

            writer.WriteLine("[roger]");
            writer.WriteLine($"AIversion = {Parameters.version}");
            writer.WriteLine();

            writer.WriteLine("[neurons]");
            writer.Write("inputNeurons = "); WriteAll(NeuralNetwork.inputNeurons, writer, true);
            writer.Write("middleNeurons = "); WriteMatrix(writer, NeuralNetwork.middleNeurons);
            writer.Write("outputNeurons = "); WriteAll(NeuralNetwork.outputNeurons, writer, true);
            writer.WriteLine();

            writer.WriteLine("[weights]");
            writer.Write("inputWeights = "); WriteAll(NeuralNetwork.inputWeights, writer, true);
            writer.Write("middleWeights = "); writer.Write(BuildStringJaggedMatrix(NeuralNetwork.middleWeights));
            writer.Write("outputWeights = "); WriteMatrix(writer, NeuralNetwork.outputWeights);

            writer.WriteLine("[biases]");
            writer.Write("Mbias = "); WriteMatrix(writer, NeuralNetwork.Mbias);
            writer.Write("Obias = "); WriteAll(NeuralNetwork.Obias, writer, true);
        }

        public static void SaveRogerToJson()
        {
            string fileName;
            int index = 0;

            do
            {
                if (index == 0)
                    fileName = $"roger.json";
                else
                    fileName = $"roger{index}.json";
                index++;
            }
            while (File.Exists(fileName));

            Roger roger = new()
            {
                AIversion = Parameters.version,

                InputNeurons = BuildStringArray(NeuralNetwork.inputNeurons),
                MiddleNeurons = BuildStringMatrix(NeuralNetwork.middleNeurons),
                OutputNeurons = BuildStringArray(NeuralNetwork.outputNeurons),

                InputWeights = BuildStringArray(NeuralNetwork.inputWeights),
                MiddleWeights = BuildStringJaggedMatrix(NeuralNetwork.middleWeights), // Вроде правильно работает, но Иван, проверь пожалуйста, я то даже не пойму работает оно или нет, для меня не крашится - значит работает а за корректность хз как то
                OutputWeights = BuildStringArray(NeuralNetwork.outputWeights),

                Mbias = BuildStringMatrix(NeuralNetwork.Mbias),
                Obias = BuildStringArray(NeuralNetwork.Obias),
            };

            string jsonData = JsonSerializer.Serialize(roger, new JsonSerializerOptions { WriteIndented = true });

            using StreamWriter writer = new(fileName);
            writer.Write(jsonData);
        }

        /// <summary>
        /// Этот метод определяет тип и парсит файл Роджера и предоставляет объект в котором все данные лежат в виде строк, их придётся приводить в нужные типы данных с помощью соответствующих функций которые вроде есть в этом классе
        /// </summary>
        /// <returns>Объект класса Roger</returns>
        public static Roger LoadRoger()
        {
            if (!File.Exists(Parameters.roger2))
                UI.Send("Roger file not found", "error");
            else // Сделал else чтобы при отсутствии файла код дальше не выполнялся
            {
                switch (CheckFormat())
                {
                    case true: // Json
                        return LoadRogerFromJson();

                    case false: // Roger 
                        return LoadRogerFromRoger();
                }
            }
            return null; // Заглушка чтобы компилятор не ругался, я хз как это исправлять. Я могу это с помощью GOTO исправить, но блин... По идее это вообще "недостижимый код"
        }

        /// <summary>
        /// Проверка формата записи
        /// </summary>
        /// <returns>true - json формат, false - roger формат/returns>
        private static bool? CheckFormat()
        {
            if (Parameters.roger2.EndsWith(".json"))
                return true;
            else if (Parameters.roger2.EndsWith(".roger") || Parameters.roger2.EndsWith(".roger2"))
                return false;

            else return null;
        }

        /// <summary>
        /// Класс который будет хранить в себе данные для загрузки/сохранения нейросети.
        /// Данные хранятся в виде строк, поэтому данные из него требуется инициализировать с помощью специальной функции InitRogersData, написанной специально для того чтобы не инициализировать всё вручную
        /// For Axolotl: Если каких-то данных не хватает, просто допиши их в класс 
        /// 
        /// PS: Если добавляешь или убираешь какое либо поле в классе Roger, делай тоже самое в функции InitRogersData() !!! *пожалуйста*
        /// </summary>
        public class Roger
        {
            public string AIversion { get; set; }

            public string InputNeurons { get; set; }
            public string MiddleNeurons { get; set; }
            public string OutputNeurons { get; set; }

            public string InputWeights { get; set; }
            public string MiddleWeights { get; set; }
            public string OutputWeights { get; set; }

            public string Mbias { get; set; }
            public string Obias { get; set; }
        }

        /// <summary>
        /// Функция которая возвращает объект класса Roger, с данными извлечёнными из файла формата .roger
        /// </summary>
        /// <returns>Объект класса Roger</returns>
        private static Roger LoadRogerFromRoger()
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile(Parameters.roger2);

            Roger roger = new()
            {
                AIversion = data["roger"]["AIversion"],

                InputNeurons = data["neurons"]["inputNeurons"],
                MiddleNeurons = data["neurons"]["middleNeurons"],
                OutputNeurons = data["neurons"]["outputNeurons"],

                InputWeights = data["weights"]["inputWeights"],
                MiddleWeights = data["weights"]["middleWeights"],
                OutputWeights = data["weights"]["outputWeights"],

                Mbias = data["biases"]["Mbias"],
                Obias = data["biases"]["Obias"]
            };

            return roger;
        }

        /// <summary>
        /// Примечание: Проверку на json это или нет надо делать заранее, эта функция подразумевает что Paramaters.roger2 это json файл!!!
        /// </summary>
        /// <returns>
        /// Возвращает обьект класса Roger со всеми нужными данными для загрузки нейросети
        /// </returns>
        private static Roger LoadRogerFromJson()
        {
            using JsonDocument document = JsonDocument.Parse(Parameters.roger2);
            JsonElement root = document.RootElement;

            Roger roger = new()
            {
                AIversion = root.GetProperty("AIversion").GetString(),

                InputNeurons = root.GetProperty("inputNeurons").GetString(),
                MiddleNeurons = root.GetProperty("middleNeurons").GetString(),
                OutputNeurons = root.GetProperty("outputNeurons").GetString(),

                InputWeights = root.GetProperty("inputWeights").GetString(),
                MiddleWeights = root.GetProperty("middleWeights").GetString(),
                OutputWeights = root.GetProperty("outputWeights").GetString(),

                Mbias = root.GetProperty("Mbias").GetString(),
                Obias = root.GetProperty("Obias").GetString()
            };

            return roger;
        }
    }

}
