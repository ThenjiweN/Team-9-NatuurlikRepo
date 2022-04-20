using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;


namespace NatuurlikBase.Services
{
    public class SendGridEmailSender : IEmailSender
    {
        private readonly IConfiguration configuration;
        private readonly ILogger logger;

        public SendGridEmailSender(IConfiguration configuration, ILogger<SendGridEmailSender> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            string sendGridApiKey = configuration["SendGridApiKey"];
            if (string.IsNullOrEmpty(sendGridApiKey))
            {
                throw new Exception("The 'SendGridApiKey' is not configured");
            }

            var client = new SendGridClient(sendGridApiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("bugtestrun@gmail.com", "Natuurlik"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(toEmail));

            var response = await client.SendEmailAsync(msg);
            if (response.IsSuccessStatusCode)
            {
                logger.LogInformation("Email queued successfully");
            }
            else
            {
                logger.LogError("Failed to send email");
            }
        }
    }
}
