using System.Diagnostics;
using System.Threading.Tasks;

namespace dotnet_g24.Services {
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender {
        public Task SendEmailAsync(string email, string subject, string message) {
            // TODO: actually send email?
            Debug.WriteLine("Email send to "+email+", Subject: "+ subject+", Message: "+message);
            return Task.CompletedTask;
        }
    }
}
