namespace PR4_VIS
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[]? lines = Storage.ReadInput();

            if (lines == null)
                return;

            List<DataBase> validAnketa = new List<DataBase>();
            List<ErrorDataBase> errors = new List<ErrorDataBase>();

            int totalLinesCount = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                int lineNumber = i + 1;

                if (string.IsNullOrWhiteSpace(line))
                    continue;

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

                var nameResult = Validators.ValidateName(name);
                var emailResult = Validators.ValidateEmail(email);
                var phoneResult = Validators.ValidatePhone(phone);
                var groupResult = Validators.ValidateGroup(group);

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

            Report.PrintResults(sortedAnketas, errors, totalLinesCount);

            Storage.SaveToFiles(sortedAnketas, errors);
        }
    }
}