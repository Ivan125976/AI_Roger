using IniParser;
using IniParser.Model;
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
                    fileName = $"roger.roger" ?? String.Empty;
                else
                    fileName = $"roger{index}.roger" ?? String.Empty;
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
            writer.Write("middleWeights = "); writer.Write(BuildStringJaggedMatrix(NeuralNetwork.middleWeights, 2));
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
                    fileName = $"roger.json" ?? String.Empty;
                else
                    fileName = $"roger{index}.json" ?? String.Empty;
                index++;
            }
            while (File.Exists(fileName));

            Roger roger = new()
            {
                AIversion = Parameters.version ?? String.Empty,

                InputNeurons = BuildStringArray(NeuralNetwork.inputNeurons) ?? String.Empty,
                MiddleNeurons = BuildStringMatrix(NeuralNetwork.middleNeurons) ?? String.Empty,
                OutputNeurons = BuildStringArray(NeuralNetwork.outputNeurons) ?? String.Empty,

                InputWeights = BuildStringArray(NeuralNetwork.inputWeights) ?? String.Empty,
                //TODO: ----------------------------------------------> ---------------------------------> ----------------------> ↓↓↓
                MiddleWeights = BuildStringJaggedMatrix(NeuralNetwork.middleWeights, 2) ?? String.Empty, //Ivan:  Я тут указал что вложенных матриц 2, но я точно не знаю сколько их на самом деле. Очень важно указать правильное значение, поэтому когда будешь использовать эту функцию, используй вычисление индекса вложенных матриц пожалуйста, или сразу хардкодь правильное значение, и пожалуйста исправь здесь значение на верное, я хз какое оно должно быть
                OutputWeights = BuildStringArray(NeuralNetwork.outputWeights) ?? String.Empty,

                Mbias = BuildStringMatrix(NeuralNetwork.Mbias) ?? String.Empty,
                Obias = BuildStringArray(NeuralNetwork.Obias) ?? String.Empty,
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
        /// <returns>true - json формат, false - roger формат</returns>
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
        /// Принять к сведению, как к работе с этим классом, там и просто к просмотру - Отсутствие данных обозначается String.Empty вместо null !!!
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
                AIversion = data["roger"]["AIversion"] ?? String.Empty,

                InputNeurons = data["neurons"]["inputNeurons"] ?? String.Empty,
                MiddleNeurons = data["neurons"]["middleNeurons"] ?? String.Empty,
                OutputNeurons = data["neurons"]["outputNeurons"] ?? String.Empty,

                InputWeights = data["weights"]["inputWeights"] ?? String.Empty,
                MiddleWeights = data["weights"]["middleWeights"] ?? String.Empty,
                OutputWeights = data["weights"]["outputWeights"] ?? String.Empty,

                Mbias = data["biases"]["Mbias"] ?? String.Empty,
                Obias = data["biases"]["Obias"] ?? String.Empty
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
            using (JsonDocument document = JsonDocument.Parse(Parameters.roger2))
            {
                JsonElement root = document.RootElement;

                Roger roger = new()
                {
                    AIversion = root.GetProperty("AIversion").GetString() ?? String.Empty,

                    InputNeurons = root.GetProperty("inputNeurons").GetString() ?? String.Empty,
                    MiddleNeurons = root.GetProperty("middleNeurons").GetString() ?? String.Empty,
                    OutputNeurons = root.GetProperty("outputNeurons").GetString() ?? String.Empty,

                    InputWeights = root.GetProperty("inputWeights").GetString() ?? String.Empty,
                    MiddleWeights = root.GetProperty("middleWeights").GetString() ?? String.Empty,
                    OutputWeights = root.GetProperty("outputWeights").GetString() ?? String.Empty,

                    Mbias = root.GetProperty("Mbias").GetString() ?? String.Empty,
                    Obias = root.GetProperty("Obias").GetString() ?? String.Empty
                };

                return roger;
            }
        }

        /// <summary>
        /// Пытается создать файл в этой же директории, если такой файл уже существует то прибавляет индекс попыток пока не дойдёт до индекса, когда файла с таким именем не будет
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="extension">Указывать расширение (без точки)</param>
        /// <returns>Имя итогового файла</returns>
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

            File.Create(filenameWithIndex);

            return filenameWithIndex;
        }
    }

}
