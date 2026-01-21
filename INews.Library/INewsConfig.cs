namespace API_iNews
{
    public class INewsConfig
    {
        public string Server { get; set; } = "192.88.8.21";
        public string BackupServer { get; set; } = "192.88.8.22";
        public string Username { get; set; } = "checkfile";
        public string Password { get; set; } = "12345678";
        public int Timeout { get; set; } = 5000;
        public string FieldMapping { get; set; } = "title,page-number";
    }
}
