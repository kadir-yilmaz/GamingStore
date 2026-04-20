namespace GamingStore.WebUI.Settings
{
    public class MailSettings
    {
        public string SmtpServer { get; set; } = "smtp.gmail.com";
        public int SmtpPort { get; set; } = 587;
        public string SenderEmail { get; set; } = string.Empty;
        public string SenderName { get; set; } = "Gaming Store";
        public string Password { get; set; } = string.Empty;
    }
}
