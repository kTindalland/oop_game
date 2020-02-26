using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces.EventArguments;

namespace Interfaces.Services
{
    public interface ILetterbox
    {
        void LetterBox(string returnAddress, PostOfficeEventArgs args); // Able to accept incoming messages
    }
}
