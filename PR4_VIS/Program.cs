using System.Text;

using static Kondratiev_PR4.Program;

namespace Kondratiev_PR4
{
    public class DataBase
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public string MasledPhone { get; set; } = "";
        public string Group { get; set; } = "";
    }
    public class ErrorDataBase
    {
        public int Id { get; set; }
        public string OriginalLine { get; set; } = "";
        public string Reason { get; set; } = "";

    }
    internal class Program
    {
        const string InputFile = @"C:\Users\polun\source\repos\Kondratievv_PR4\Kondratievv_PR4\database.txt";
        const string ComplitedFile = @"C:\Users\polun\source\repos\Kondratievv_PR4\Kondratievv_PR4\complited.txt";
        const string ErrorsFile = @"C:\Users\polun\source\repos\Kondratievv_PR4\Kondratievv_PR4\errors.txt";

        static void Main(string[] args)
        {
            if (!File.Exists(InputFile))
            {
                Console.WriteLine($"Ошибка: Файл {InputFile} не найден.");
                return;
            }

            string[] lines;

            try
            {
                lines = File.ReadAllLines(InputFile, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка чтения файла: {ex.Message}");
                return;
            }

            List<DataBase> validAnketa = new List<DataBase>();
            List<ErrorDataBase> errors = new List<ErrorDataBase>();

            int totalLinesCount = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                int lineNumber = i + 1;

                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                totalLinesCount++;

                string[] parts = line.Split(';');

                if (parts.Length != 4)
                {
                    errors.Add(new ErrorDataBase
                    {
                        Id = lineNumber,
                        OriginalLine = line,
                        Reason = $"Ожидалось 4 поля, сейчас {parts.Length}"
                    });
                    continue;
                }

                string name = parts[0];
                string email = parts[1];
                string phone = parts[2];
                string group = parts[3];

                var nameResult = ValidateName(name);
                var emailResult = ValidateEmail(email);
                var phoneResult = ValidatePhone(phone);
                var groupResult = ValidateGroup(group);

                List<string> lineErrors = new List<string>();
                if (!nameResult.IsValid) lineErrors.Add(nameResult.Error);
                if (!emailResult.IsValid) lineErrors.Add(emailResult.Error);
                if (!phoneResult.IsValid) lineErrors.Add(phoneResult.Error);
                if (!groupResult.IsValid) lineErrors.Add(groupResult.Error);

                if (lineErrors.Count > 0)
                {
                    errors.Add(new ErrorDataBase
                    {
                        Id = lineNumber,
                        OriginalLine = line,
                        Reason = string.Join(" | ", lineErrors)
                    });
                }
                else
                {
                    validAnketa.Add(new DataBase
                    {
                        Id = lineNumber,
                        Name = nameResult.Value,
                        Email = emailResult.Value,
                        Phone = phoneResult.Value,
                        Group = groupResult.Value
                    });
                }
            }

            var sortedAnketas = validAnketa.OrderBy(a => a.Name).ToList();

            PrintResults(sortedAnketas, errors, totalLinesCount);

            SaveToFiles(sortedAnketas, errors);

            static (bool IsValid, string Value, string Error) ValidateName(string input)
            {
                string[] words = input.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);

                if (words.Length < 2)
                {
                    return (false, "", "ФИО содержит меньше двух слов");
                }

                string normalized = string.Join(" ", words.Select(w => char.ToUpper(w[0]) + w.Substring(1).ToLower()));

                return (true, normalized, "");
            }
            static (bool IsValid, string Value, string Error) ValidateEmail(string input)
            {
                string email = input.Trim().ToLower();

                int count = email.Count(c => c == '@');
                if (count != 1)
                {
                    return (false, "", "Почта должна содержать ровно одну @");
                }

                string[] parts = email.Split('@');
                string name = parts[0];
                string domain = parts[1];

                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(domain))
                {
                    return (false, "", "Одна из частей почты пустая");
                }

                if (!domain.Contains('.') || domain.StartsWith('.') || domain.EndsWith('.'))
                {
                    return (false, "", "Неккоректный домен");
                }
                return (true, email, "");
            }
            static (bool IsValid, string Value, string Error) ValidatePhone(string input)
            {
                string digits = string.Concat(input.Where(char.IsDigit));

                if (digits.Length != 11)
                {
                    return (false, "", $"Телефон должен содержать 11 цифр, сейчас {digits.Length}");
                }

                if (digits[0] != '7' && digits[0] != '8')
                {
                    return (false, "", "Телефон должен начинаться с 7 или 8");
                }

                string formatted = $"+7 ({digits.Substring(1, 3)}) {digits.Substring(4, 3)}-{digits.Substring(7, 2)}-{digits.Substring(9, 2)}";

                return (true, formatted, "");
            }
            static (bool IsValid, string Value, string Error) ValidateGroup(string input)
            {
                string group = input.Trim().ToLower();

                if (string.IsNullOrWhiteSpace(group))
                    return (false, "", "Группа не может быть пустой");

                return (true, group, "");
            }
            static void PrintResults(List<DataBase> anketa, List<ErrorDataBase> errors, int totalLinesCount)
            {
                Console.WriteLine(" --Принятые анкеты-- ");
                if (anketa.Count == 0)
                {
                    Console.WriteLine("Нет принятых анкет.");
                }
                else
                {
                    Console.WriteLine($"{"№",-3} | {"ФИО",-30} | {"Почта",-20} | {"Телефон",-18} | {"Группа",-7}");
                    Console.WriteLine(new string('-', 52));

                    int index = 1;
                    foreach (var a in anketa)
                    {
                        Console.WriteLine($"{index,-3} | {a.Name,-30} | {a.Email,-20} | {a.Phone,-18} | {a.Group,-7}");
                        index++;
                    }
                }

                Console.WriteLine("\n --Бракованные строки-- ");
                if (errors.Count == 0)
                {
                    Console.WriteLine("Ошибок нет.");
                }
                else
                {
                    foreach (var e in errors)
                    {
                        Console.WriteLine($"Строка {e.Id}: {e.Reason}");
                        Console.WriteLine($"  -> {e.OriginalLine}");
                    }
                }

                Console.WriteLine("\n --Итоги-- ");
                Console.WriteLine($"Всего обработано строк: {totalLinesCount}");
                Console.WriteLine($"Принято анкет: {anketa.Count}");
                Console.WriteLine($"Отклонено: {errors.Count}");

                Console.WriteLine("Распределение по группам:");
                var groupStats = anketa.GroupBy(a => a.Group).OrderBy(g => g.Key);
                if (!groupStats.Any())
                    Console.WriteLine("  (нет данных)");
                else
                {
                    foreach (var g in groupStats)
                    {
                        Console.WriteLine($"  {g.Key}: {g.Count()}");
                    }
                }
            }
            static void SaveToFiles(List<DataBase> anketas, List<ErrorDataBase> errors)
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
}
