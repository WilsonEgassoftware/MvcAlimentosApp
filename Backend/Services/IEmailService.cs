namespace SupermarketAPI.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body, bool isHtml = true);
        Task SendVerificationEmailAsync(string to, string username, string verificationToken);
        Task SendInvoiceEmailAsync(string to, string username, string invoiceHtml);
    }
}
