using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniverseRestaurant.Services
{
    public interface IMailSender
    {
        void SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
