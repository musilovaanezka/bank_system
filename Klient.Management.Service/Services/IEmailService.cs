
using Klient.Management.Service.Models;

namespace Klient.Management.Service.Services
{
    public interface IEmailService
    {
        void SendEmail(Message message);
    }
}
