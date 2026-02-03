using Microsoft.AspNetCore.Identity.UI.Services;

namespace Ksiegarnia.Helpers;

public class ConsoleEmailSenderHelper : IEmailSender {
    private readonly ILogger<ConsoleEmailSenderHelper> _logger;

    public ConsoleEmailSenderHelper(ILogger<ConsoleEmailSenderHelper> logger)
    {
        _logger = logger;
    }

    // fałszywa metoda wysyłająca mejle
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        _logger.LogInformation("FAKE CONSOLE EMAIL to: {Email}\nSubject: {Subject}\nBody:\n{Body}",
            email, subject, htmlMessage);

        return Task.CompletedTask;
    }
}