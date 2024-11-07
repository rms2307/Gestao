using Gestao.Data;
using Microsoft.AspNetCore.Identity;
using System.Net.Mail;

namespace Gestao.Infra.Mail
{
    public class EmailSender(ILogger<EmailSender> logger, SmtpClient smtpClient, IConfiguration configuration) : IEmailSender<ApplicationUser>
    {
        private readonly ILogger logger = logger;
        private readonly SmtpClient smtpClient = smtpClient;
        private readonly IConfiguration configuration = configuration;

        public Task SendConfirmationLinkAsync(ApplicationUser user, string email,
            string confirmationLink) => SendEmailAsync(email, "Confirm your email",
            "Please confirm your account by " +
            $"<a href='{confirmationLink}'>clicking here</a>.");

        public Task SendPasswordResetLinkAsync(ApplicationUser user, string email,
            string resetLink) => SendEmailAsync(email, "Reset your password",
            $"Please reset your password by <a href='{resetLink}'>clicking here</a>.");

        public Task SendPasswordResetCodeAsync(ApplicationUser user, string email,
            string resetCode) => SendEmailAsync(email, "Reset your password",
            $"Please reset your password using the following code: {resetCode}");

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            await Execute(subject, message, toEmail);
        }

        public async Task Execute(string subject, string message, string toEmail)
        {
            MailMessage mailMessage = new()
            {
                From = new MailAddress(configuration.GetValue<string>("EmailSender:User")!),
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };
            mailMessage.To.Add(new MailAddress(toEmail));

            await smtpClient.SendMailAsync(mailMessage);

            logger.LogInformation("Email to {EmailAddress} sent!", toEmail);
        }
    }
}
