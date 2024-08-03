using pva.Models;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace pva.Services
{
    public interface INotificationService
    {
        Task<bool> HandleWarningsAndSendEmailAsync(Value newValue);
    }

    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailNotification _emailNotif;

        public NotificationService(ApplicationDbContext context, IOptions<EmailNotification> emailSettings)
        {
            _context = context;
            _emailNotif = emailSettings.Value;
        }

        public async Task<bool> HandleWarningsAndSendEmailAsync(Value newValue)
        {
            var station = await _context.Stations.FindAsync(newValue.StationId);
            if (station == null) return false;

            bool isFloodWarning = newValue.Level >= station.FloodLevel;
            bool isDroughtWarning = newValue.Level <= station.DroughtLevel;

            if (isFloodWarning)
            {
                await SendEmailAsync(
                    _emailNotif.Recipient,
                    $"{station.Name} - Flood Warning Alert",
                    $"Flood warning level exceeded at station '{station.Name}'. Current level: {newValue.Level}. Maximum value: {station.FloodLevel}."
                );
            }

            if (isDroughtWarning)
            {
                await SendEmailAsync(
                    _emailNotif.Recipient,
                    $"{station.Name} - Drought Warning Alert",
                    $"Drought warning level exceeded at station '{station.Name}'. Current level: {newValue.Level}. Minimum value: {station.DroughtLevel}."
                );
            }

            return true;
        }

        private async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            try
            {
                var smtpClient = new SmtpClient(_emailNotif.SmtpHost)
                {
                    Port = _emailNotif.SmtpPort,
                    Credentials = null,
                    EnableSsl = false,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_emailNotif.FromEmail, _emailNotif.FromName),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = false,
                };

                mailMessage.To.Add(new MailAddress(toEmail));

                await smtpClient.SendMailAsync(mailMessage);
                Console.WriteLine($"E-mail sent to {toEmail} with a subject '{subject}'.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending e-mail: {ex.Message}");
                throw;
            }
        }

    }
}
