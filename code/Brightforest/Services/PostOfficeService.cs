using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces.EventArguments;
using Interfaces.Services;

namespace Brightforest.Services
{
    public class PostOfficeService : IPostOfficeService
    {
        private Dictionary<string, ILetterbox> _clients;

        public PostOfficeService()
        {
            _clients = new Dictionary<string, ILetterbox>();
        }

        public bool RegisterClient(ILetterbox client, string address)
        {
            var keyAlreadyThere = _clients.Keys.Contains(address);

            if (keyAlreadyThere)
            {
                return false;
            }

            _clients[address] = client;

            return true;
        }

        public bool SendMail(string returnAddress, PostOfficeEventArgs args)
        {
            var keyThere = _clients.Keys.Contains(args.SendAddress);

            if (!keyThere)
            {
                return false;
            }

            _clients[args.SendAddress].LetterBox(returnAddress, args);

            return true;
        }
    }
}
