namespace PR4_VIS
{
    public static class Report
    {
        public static void PrintResults(
            List<DataBase> anketa,
            List<ErrorDataBase> errors,
            int totalLinesCount)
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
    }
}