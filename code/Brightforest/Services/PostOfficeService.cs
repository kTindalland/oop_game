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


        // Provides messaging between clients
        public PostOfficeService()
        {
            _clients = new Dictionary<string, ILetterbox>();
        }

        public bool RegisterClient(ILetterbox client, string address)
        {
            // Check if key already exists
            var keyAlreadyThere = _clients.Keys.Contains(address);

            // Return false if it is
            if (keyAlreadyThere)
            {
                return false;
            }

            // Otherwise add it to the clientel
            _clients[address] = client;

            return true;
        }

        // Call letterbox function and pass any messages through
        public bool SendMail(string returnAddress, PostOfficeEventArgs args)
        {
            // Validate key is there
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
