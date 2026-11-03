using System.Text;

namespace PR4_VIS
{
    public static class Storage
    {
        public const string InputFile =
            @"C:\Users\polun\source\repos\Kondratievv_PR4\Kondratievv_PR4\database.txt";

        public const string ComplitedFile =
            @"C:\Users\polun\source\repos\Kondratievv_PR4\Kondratievv_PR4\complited.txt";

        public const string ErrorsFile =
            @"C:\Users\polun\source\repos\Kondratievv_PR4\Kondratievv_PR4\errors.txt";

        public static string[]? ReadInput()
        {
            if (!File.Exists(InputFile))
            {
                Console.WriteLine($"Ошибка: Файл {InputFile} не найден.");
                return null;
            }

            try
            {
                return File.ReadAllLines(InputFile, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка чтения файла: {ex.Message}");
                return null;
            }
        }

        public static void SaveToFiles(
            List<DataBase> anketas,
            List<ErrorDataBase> errors)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(ComplitedFile, false, Encoding.UTF8))
                {
                    foreach (var a in anketas)
                    {
                        writer.WriteLine($"{a.Name} {a.Email} {a.Phone} {a.Group}");
                    }
                }

                using (StreamWriter writer = new StreamWriter(ErrorsFile, false, Encoding.UTF8))
                {
                    foreach (var e in errors)
                    {
                        writer.WriteLine($"Строка {e.Id}: {e.OriginalLine}");
                        writer.WriteLine($"Причина: {e.Reason}");
                        writer.WriteLine();
                    }
                }
                Console.WriteLine($"\nДанные успешно сохранены в {ComplitedFile} и {ErrorsFile}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nОшибка при сохранении: {ex.Message}");
            }
        }
    }
}