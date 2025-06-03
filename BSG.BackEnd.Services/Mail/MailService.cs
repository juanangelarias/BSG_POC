using BSG.BackEnd.Common.Model;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace BSG.BackEnd.Services.Mail;

public class MailService(MailParameters parameters) : IMailService
{
    private bool _success;

    public bool SendEmail(EmailMessage emailMessage)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(parameters.From));
        email.To.AddRange(emailMessage.To?.Select(MailboxAddress.Parse));
        email.Subject = emailMessage.Subject;
        email.Body = emailMessage.IsHtml 
            ? new TextPart(TextFormat.Html) {Text = emailMessage.Body} 
            : new TextPart(TextFormat.Plain) {Text = emailMessage.Body};

        return Send(email);
    }
    
    private bool Send(MimeMessage email)
    {
        _success = false;

        if (parameters.SmtpServer == null || parameters.Port == null)
            return false;
        
        var secureSocketOptions = parameters.EnableSsl ?? false
            ? SecureSocketOptions.StartTls
            : SecureSocketOptions.None;
        
        using var smtp = new SmtpClient();
        smtp.MessageSent += MessageSent!;
        smtp.Connect(parameters.SmtpServer, parameters.Port ?? 0, secureSocketOptions);
        smtp.Authenticate(parameters.Username, parameters.Password);
        smtp.Send(email);
        smtp.Disconnect(true);

        return _success;
    }

    private void MessageSent(object o, MessageSentEventArgs args)
    {
        _success = true;
    }
}