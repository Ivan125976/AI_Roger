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

            using StreamWriter writer = new(MakeFileSplitOnIndexIfExists("roger", "roger"));

            writer.Write(
                $"""
                 [roger]
                 AIversion = {Parameters.version}
                 isDebug = {Parameters.isDebug}
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

                learningRate = Parameters.learningRate,
                DropOutPercent = Parameters.DropOutPercent,

                InputNeuronsCount = Parameters.inputNeuronsCount,
                MiddleNeuronsCount = Parameters.outputNeuronsCount,
                OutputNeuronsCount = Parameters.outputNeuronsCount,

                Layers = Parameters.layers,
                MLayers = Parameters.Mlayers,
            };

            string jsonData = JsonSerializer.Serialize(roger, new JsonSerializerOptions { WriteIndented = true });

            using StreamWriter writer = new(IO.MakeFileSplitOnIndexIfExists("roger", "json"));
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

            public bool IsDebug { get; set; }
            public int Passes { get; set; }

            public float learningRate { get; set; }
            public float DropOutPercent { get; set; }

            public int InputNeuronsCount { get; set; }
            public int MiddleNeuronsCount { get; set; }
            public int OutputNeuronsCount { get; set; }

            public int Layers { get; set; }
            public int MLayers { get; set; }
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
                IsDebug = Convert.ToBoolean(data["roger"]["isDebug"]),
                Passes = Convert.ToInt32(data["roger"]["passes"]),
                learningRate = float.Parse(data["roger"]["learningRate"]),
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
        /// Примечание: Проверку на json это или нет надо делать заранее, эта функция подразумевает что Paramaters.roger2 это json файл!!!
        /// </summary>
        /// <returns>
        /// Возвращает обьект класса Roger со всеми нужными данными для загрузки нейросети
        /// </returns>
        private static Roger LoadRogerFromJson()
        {
            using (JsonDocument document = JsonDocument.Parse(File.ReadAllText(Parameters.roger2)))
            {
                JsonElement root = document.RootElement;

                Roger roger = new()
                {
                    AIversion = root.GetProperty("AIversion").GetString(),
                    IsDebug = root.GetProperty("isDebug").GetBoolean(),
                    Passes = root.GetProperty("Passes").GetInt32(),

                    learningRate = float.Parse(root.GetProperty("learningRate").GetString()), // Переделаю это говно
                    DropOutPercent = float.Parse(root.GetProperty("learningRate").GetString()), // И вот это тоже

                    InputNeuronsCount = root.GetProperty("inputNeuronsCount").GetInt32(),
                    MiddleNeuronsCount = root.GetProperty("middleNeuronsCount").GetInt32(),
                    OutputNeuronsCount = root.GetProperty("outputNeuronsCount").GetInt32(),

                    Layers = root.GetProperty("Layers").GetInt32(),
                    MLayers = root.GetProperty("Mlayers").GetInt32()
                };

                return roger;
            }
        }

        /// <summary>
        /// Пытается создать файл в этой же директории, если такой файл уже существует то прибавляет индекс попыток пока не дойдёт до индекса, когда файла с таким именем не будет
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="extension">Расширение файла (без точки)</param>
        /// <param name="whatToReturn">true - Возвращает FileStream созданного файла, false - Возвращает путь к созданному файлу</param>
        /// <returns>Путь до итогового файла</returns>
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
