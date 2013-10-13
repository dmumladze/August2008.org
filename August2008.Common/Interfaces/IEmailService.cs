using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace August2008.Common.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(string from, string to, string subject, string body);
    }
}
