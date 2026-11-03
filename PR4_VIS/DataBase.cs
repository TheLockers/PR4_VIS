namespace PR4_VIS
{
    public class DataBase
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Group { get; set; } = "";
    }

    public class ErrorDataBase
    {
        public int Id { get; set; }
        public string OriginalLine { get; set; } = "";
        public string Reason { get; set; } = "";
    }
}