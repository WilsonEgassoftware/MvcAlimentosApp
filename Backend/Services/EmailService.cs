using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace SupermarketAPI.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = true)
        {
            try
            {
                var smtpSettings = _configuration.GetSection("SmtpSettings");
                var smtpServer = smtpSettings["Server"] ?? "smtp.gmail.com";
                var smtpPort = int.Parse(smtpSettings["Port"] ?? "587");
                var smtpUsername = smtpSettings["Username"];
                var smtpPassword = smtpSettings["Password"];
                var fromEmail = smtpSettings["FromEmail"] ?? smtpUsername;
                var fromName = smtpSettings["FromName"] ?? "Supermarket System";

                // Si no hay configuración SMTP, solo loguear (para desarrollo)
                if (string.IsNullOrEmpty(smtpUsername) || string.IsNullOrEmpty(smtpPassword))
                {
                    _logger.LogWarning("SMTP no configurado. Email no enviado a: {Email}", to);
                    _logger.LogInformation("Email que se hubiera enviado:\nTo: {To}\nSubject: {Subject}\nBody: {Body}", to, subject, body);
                    return;
                }

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(fromName, fromEmail));
                message.To.Add(new MailboxAddress("", to));
                message.Subject = subject;
                message.Body = new TextPart(isHtml ? TextFormat.Html : TextFormat.Plain)
                {
                    Text = body
                };

                using var client = new SmtpClient();
                await client.ConnectAsync(smtpServer, smtpPort, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(smtpUsername, smtpPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                _logger.LogInformation("Email enviado exitosamente a: {Email}", to);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al enviar email a: {Email}", to);
                throw;
            }
        }

        public async Task SendVerificationEmailAsync(string to, string username, string verificationToken)
        {
            var verificationUrl = $"{_configuration["AppBaseUrl"] ?? "http://localhost:5173"}/verify-email?token={verificationToken}";
            
            var emailBody = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background-color: #2563eb; color: white; padding: 20px; text-align: center; border-radius: 5px 5px 0 0; }}
        .content {{ background-color: #f9fafb; padding: 30px; border-radius: 0 0 5px 5px; }}
        .button {{ display: inline-block; background-color: #2563eb; color: white; padding: 12px 24px; text-decoration: none; border-radius: 5px; margin: 20px 0; }}
        .footer {{ text-align: center; margin-top: 20px; color: #666; font-size: 12px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>🛒 Supermarket System</h1>
        </div>
        <div class='content'>
            <h2>¡Bienvenido, {username}!</h2>
            <p>Gracias por registrarte en nuestro sistema de supermercado.</p>
            <p>Para activar tu cuenta, por favor haz clic en el siguiente botón:</p>
            <div style='text-align: center;'>
                <a href='{verificationUrl}' class='button'>Verificar Email</a>
            </div>
            <p>O copia y pega este enlace en tu navegador:</p>
            <p style='word-break: break-all; color: #2563eb;'>{verificationUrl}</p>
            <p>Este enlace expirará en 24 horas.</p>
            <p>Si no solicitaste este registro, ignora este email.</p>
        </div>
        <div class='footer'>
            <p>© {DateTime.UtcNow.Year} Supermarket System. Todos los derechos reservados.</p>
        </div>
    </div>
</body>
</html>";

            await SendEmailAsync(to, "Verifica tu cuenta - Supermarket System", emailBody);
        }

        public async Task SendInvoiceEmailAsync(string to, string username, string invoiceHtml)
        {
            var subject = $"Factura de Compra - Supermarket System - {DateTime.UtcNow:yyyy-MM-dd}";
            
            var emailBody = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 800px; margin: 0 auto; padding: 20px; }}
        .header {{ background-color: #2563eb; color: white; padding: 20px; text-align: center; border-radius: 5px 5px 0 0; }}
        .content {{ background-color: #ffffff; padding: 30px; }}
        .footer {{ text-align: center; margin-top: 20px; color: #666; font-size: 12px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>🛒 Supermarket System</h1>
            <p>Factura de Compra</p>
        </div>
        <div class='content'>
            <p>Estimado/a <strong>{username}</strong>,</p>
            <p>Gracias por tu compra. Adjuntamos tu factura:</p>
            {invoiceHtml}
        </div>
        <div class='footer'>
            <p>© {DateTime.UtcNow.Year} Supermarket System. Todos los derechos reservados.</p>
        </div>
    </div>
</body>
</html>";

            await SendEmailAsync(to, subject, emailBody);
        }
    }
}
