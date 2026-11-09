namespace Application.Common.Services.EmailManager;

public interface IEmailService
{
    Task SendEmailAsync(string email, string subject, string htmlMessage);

}
