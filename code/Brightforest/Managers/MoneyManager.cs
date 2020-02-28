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

        // Amount of money the player has
        private int _money;

        public MoneyManager(IPostOfficeService postOfficeService)
        {
            // Start with 50 m's
            _money = 50;
        }

        // Public face of money
        public int Money
        {
            get { return _money; }
            set { _money = value; }
        }

        // Reset the players money
        public void Reset()
        {
            _money = 50;
        }

        // Get post office messages
        public void LetterBox(string returnAddress, PostOfficeEventArgs args)
        {
            switch (args.MessageName)
            {
                // Add money to the player
                case "AddMoney":

                    var newMoney = BitConverter.ToInt32(args.Data, 0);

                    _money += newMoney;

                    break;
            }
        }
    }
}
