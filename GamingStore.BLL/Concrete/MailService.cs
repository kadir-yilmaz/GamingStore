using GamingStore.BLL.Abstract;
using GamingStore.EL.Settings;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Options;
using GamingStore.EL.Models;

namespace GamingStore.BLL.Concrete
{
    public class MailService : IEmailService
    {
        private readonly MailSettings _settings;

        public MailService(IOptions<MailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendOrderConfirmationAsync(string customerEmail, Order order)
        {
            var subject = $"Siparişiniz Alındı - Sipariş #{order.Id}";
            
            var content = BuildOrderConfirmationContent(order);
            
            await SendMailAsync(customerEmail, subject, content);
        }

        private string BuildOrderConfirmationContent(Order order)
        {
            var sb = new System.Text.StringBuilder();
            
            sb.AppendLine($"Merhaba {order.ShippingFirstName} {order.ShippingLastName},");
            sb.AppendLine();
            sb.AppendLine($"Siparişiniz #{order.Id} başarıyla alınmıştır.");
            sb.AppendLine();
            sb.AppendLine("📦 SİPARİŞ DETAYLARI");
            sb.AppendLine("─────────────────────────────────────");
            
            decimal total = 0;
            foreach (var line in order.Lines)
            {
                var lineTotal = line.Product.Price * line.Quantity;
                total += lineTotal;
                sb.AppendLine($"• {line.Product.Price:C2} x {line.Quantity} adet - {line.Product.Name}");
            }
            
            sb.AppendLine("─────────────────────────────────────");
            sb.AppendLine($"TOPLAM: {total:C2}");
            sb.AppendLine();
            
            sb.AppendLine("📍 TESLİMAT ADRESİ");
            sb.AppendLine("─────────────────────────────────────");
            sb.AppendLine($"{order.ShippingFirstName} {order.ShippingLastName}");
            sb.AppendLine($"{order.ShippingPhone}");
            sb.AppendLine($"{order.ShippingNeighborhood}, {order.ShippingDistrict}/{order.ShippingProvince}");
            sb.AppendLine($"{order.ShippingAddressDetail}");
            sb.AppendLine();
            
            if (order.GiftWrap)
            {
                sb.AppendLine("🎁 Hediye paketi olarak gönderilecektir.");
                sb.AppendLine();
            }
            
            sb.AppendLine("Siparişinizi tercih ettiğiniz için teşekkür ederiz!");
            sb.AppendLine();
            sb.AppendLine("Gaming Store Ekibi");
            
            return sb.ToString();
        }

        private async Task SendMailAsync(string toEmail, string subject, string content)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
            mimeMessage.To.Add(new MailboxAddress("", toEmail));
            mimeMessage.Subject = subject;
            mimeMessage.Body = new BodyBuilder { TextBody = content }.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(_settings.SmtpServer, _settings.SmtpPort, false);
            await client.AuthenticateAsync(_settings.SenderEmail, _settings.Password);
            await client.SendAsync(mimeMessage);
            await client.DisconnectAsync(true);
        }
    }
}
