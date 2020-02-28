using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces.EventArguments;
using Interfaces.Services;

namespace Brightforest.Managers
{
    public class MoneyManager : ILetterbox
    {
        public string LetterboxName => "MoneyManager";

        private int _money;

        public MoneyManager(IPostOfficeService postOfficeService)
        {
            _money = 0;
        }

        public int Money
        {
            get { return _money; }
        }

        public void Reset()
        {
            _money = 0;
        }

        public void LetterBox(string returnAddress, PostOfficeEventArgs args)
        {
            switch (args.MessageName)
            {
                case "AddMoney":

                    var newMoney = BitConverter.ToInt32(args.Data, 0);

                    _money += newMoney;

                    break;
            }
        }
    }
}
