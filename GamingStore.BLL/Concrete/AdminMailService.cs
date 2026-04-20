using GamingStore.BLL.Abstract;
using GamingStore.EL.Models;
using GamingStore.EL.Settings;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System;

namespace GamingStore.BLL.Concrete
{
    public class AdminMailService : IAdminMailService
    {
        private readonly MailSettings _mailSettings;

        public AdminMailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendMailAsync(MailRequest mailRequest)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail));
            mimeMessage.To.Add(new MailboxAddress("Admin", _mailSettings.AdminEmail));
            mimeMessage.Subject = mailRequest.Subject;
            mimeMessage.Body = new BodyBuilder { TextBody = mailRequest.Content }.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(_mailSettings.SmtpServer, _mailSettings.SmtpPort, false);
            await client.AuthenticateAsync(_mailSettings.SenderEmail, _mailSettings.Password);
            await client.SendAsync(mimeMessage);
            await client.DisconnectAsync(true);
        }
    }
}
