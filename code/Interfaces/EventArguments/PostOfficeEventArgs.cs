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

        public string SendAddress { get; set; } // Where the letter goes
        public string MessageName { get; set; } // The message it will invoke
        public byte[] Data { get; set; } // The data carried along with it.
    }
}
