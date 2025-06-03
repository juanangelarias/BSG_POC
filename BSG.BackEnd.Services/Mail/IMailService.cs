using BSG.BackEnd.Common.Model;

namespace BSG.BackEnd.Services.Mail;

public interface IMailService
{
    bool SendEmail(EmailMessage emailMessage);
}