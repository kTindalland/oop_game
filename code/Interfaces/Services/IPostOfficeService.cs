using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces.EventArguments;

namespace Interfaces.Services
{
    public interface IPostOfficeService
    {
        bool SendMail(string returnAddress, PostOfficeEventArgs args);

        bool RegisterClient(ILetterbox client, string address);
    }
}
