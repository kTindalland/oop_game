using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Interfaces.EventArguments;
using Interfaces.Services;

namespace Brightforest.Managers
{
    public class StatsManager : ILetterbox
    {
        public string LetterboxName { get; set; }

        private int _rateOfFire;
        private int _damageModifier;

        private int _timesHalvedRoF;
        private readonly IPostOfficeService _postOfficeService;
        private readonly MoneyManager _moneyManager;

        public int RateOfFire
        {
            get { return _rateOfFire; }
        }

        public int RateOfFireLevel
        {
            get { return _timesHalvedRoF; }
        }

        public int DamageModifier
        {
            get { return _damageModifier; }
        }

        public bool CanHalfRof => (_timesHalvedRoF < 3);
        public bool CanIncreaseDamage => (_damageModifier < 5);

        public StatsManager(IPostOfficeService postOfficeService, MoneyManager moneyManager)
        {
            LetterboxName = "StatsManager";

            _rateOfFire = 6000;
            _damageModifier = 1;
            _timesHalvedRoF = 0;
            _postOfficeService = postOfficeService;
            _moneyManager = moneyManager;
        }

        public void Reset()
        {
            _rateOfFire = 6000;
            _damageModifier = 1;
            _timesHalvedRoF = 0;
        }

        public void LetterBox(string returnAddress, PostOfficeEventArgs args)
        {
            var disableArgs = new PostOfficeEventArgs()
            {
                SendAddress = returnAddress,
                MessageName = "Disable",
                Data = new byte[0]
            };

            var reduceMoneyArgs = new PostOfficeEventArgs()
            {
                SendAddress = "MoneyManager",
                MessageName = "AddMoney",
                Data = BitConverter.GetBytes(-30)
            };

            switch (args.MessageName)
            {
                case "ModifyArrowDamage":

                    if (CanIncreaseDamage && _moneyManager.Money >= 30)
                    {
                        _damageModifier++; // Increases the damage output by 1x

                        if (!CanIncreaseDamage)
                        {
                            _postOfficeService.SendMail(this.LetterboxName, disableArgs);
                        }

                        _postOfficeService.SendMail(this.LetterboxName, reduceMoneyArgs);
                    }
                    

                    break;

                case "ModifyRateOfFire":

                    if (CanHalfRof && _moneyManager.Money >= 30)
                    {
                        _rateOfFire /= 2; // Doubles the rate of fire of projectiles.
                        _timesHalvedRoF++;

                        if (!CanHalfRof)
                        {
                            _postOfficeService.SendMail(this.LetterboxName, disableArgs);
                        }

                        _postOfficeService.SendMail(this.LetterboxName, reduceMoneyArgs);
                    }
                    

                    break;
            }
        }
    }
}
