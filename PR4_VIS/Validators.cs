namespace PR4_VIS
{
    public static class Validators
    {
        public static (bool IsValid, string Value, string Error)
            ValidateName(string input)
        {
            string[] words = input.Split(
                (char[]?)null,
                StringSplitOptions.RemoveEmptyEntries);

            if (words.Length < 2)
            {
                return (false, "", "ФИО содержит меньше двух слов");
            }

            string normalized = string.Join(" ",
                words.Select(w =>
                    char.ToUpper(w[0]) + w.Substring(1).ToLower()));

            return (true, normalized, "");
        }

        public static (bool IsValid, string Value, string Error)
            ValidateEmail(string input)
        {
            string email = input.Trim().ToLower();

            int count = email.Count(c => c == '@');

            if (count != 1)
                return (false, "", "Почта должна содержать ровно одну @");

            string[] parts = email.Split('@');
            string name = parts[0];
            string domain = parts[1];

            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(domain))
                return (false, "", "Одна из частей почты пустая");

            if (!domain.Contains('.') ||
                domain.StartsWith('.') ||
                domain.EndsWith('.'))
                return (false, "", "Неккоректный домен");

            return (true, email, "");
        }

        public static (bool IsValid, string Value, string Error)
            ValidatePhone(string input)
        {
            string digits = string.Concat(input.Where(char.IsDigit));

            if (digits.Length != 11)
                return (false, "", $"Телефон должен содержать 11 цифр, сейчас {digits.Length}");

            if (digits[0] != '7' && digits[0] != '8')
                return (false, "", "Телефон должен начинаться с 7 или 8");

            string formatted =
                $"+7 ({digits.Substring(1, 3)}) {digits.Substring(4, 3)}-{digits.Substring(7, 2)}-{digits.Substring(9, 2)}";

            return (true, formatted, "");
        }

        public static (bool IsValid, string Value, string Error)
            ValidateGroup(string input)
        {
            string group = input.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(group))
                return (false, "", "Группа не может быть пустой");

            return (true, group, "");
        }
    }
}