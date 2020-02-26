using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.EventArguments
{
    public class PostOfficeEventArgs : EventArgs
    {
        // Not strictly an interface but belongs here really.
        public PostOfficeEventArgs()
        {
            SendAddress = "";
            MessageName = "";
            Data = new byte[0];
        }

        public string SendAddress { get; set; }
        public string MessageName { get; set; }
        public byte[] Data { get; set; }
    }
}
