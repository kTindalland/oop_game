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

        // Public face of raw rate of fire, in milliseconds
        public int RateOfFire
        {
            get { return _rateOfFire; }
        }

        // Public face of RoF level, this is how many times it's been halved
        public int RateOfFireLevel
        {
            get { return _timesHalvedRoF; }
        }

        // Public face to damage multiplier
        public int DamageModifier
        {
            get { return _damageModifier; }
        }

        // Get if can half RoF again
        public bool CanHalfRof => (_timesHalvedRoF < 3);

        // Get if can increase the damage again
        public bool CanIncreaseDamage => (_damageModifier < 5);

        // Handles the players stats, like rate of fire and damage output
        public StatsManager(IPostOfficeService postOfficeService, MoneyManager moneyManager)
        {
            LetterboxName = "StatsManager";

            // Default values
            Reset();
            _postOfficeService = postOfficeService;
            _moneyManager = moneyManager;
        }

        public void Reset()
        {
            _rateOfFire = 4000;
            _damageModifier = 1;
            _timesHalvedRoF = 0;
        }

        // All post office messages coming in and replies
        public void LetterBox(string returnAddress, PostOfficeEventArgs args)
        {
            // to disable a button
            var disableArgs = new PostOfficeEventArgs()
            {
                SendAddress = returnAddress,
                MessageName = "Disable",
                Data = new byte[0]
            };

            // To reduce the player money
            var reduceMoneyArgs = new PostOfficeEventArgs()
            {
                SendAddress = "MoneyManager",
                MessageName = "AddMoney",
                Data = BitConverter.GetBytes(-30)
            };

            switch (args.MessageName)
            {
                // Change damage multiplier
                case "ModifyArrowDamage":

                    if (CanIncreaseDamage && _moneyManager.Money >= 30)
                    {
                        _damageModifier++; // Increases the damage output by 1x

                        if (!CanIncreaseDamage)
                        {
                            // Disable damage upgrade button
                            _postOfficeService.SendMail(this.LetterboxName, disableArgs);
                        }

                        // reduce the money
                        _postOfficeService.SendMail(this.LetterboxName, reduceMoneyArgs);
                    }
                    

                    break;

                // Change rate of fire
                case "ModifyRateOfFire":

                    if (CanHalfRof && _moneyManager.Money >= 30)
                    {
                        _rateOfFire /= 2; // Doubles the rate of fire of projectiles.
                        _timesHalvedRoF++;

                        if (!CanHalfRof)
                        {
                            // Disable the rof upgrade button
                            _postOfficeService.SendMail(this.LetterboxName, disableArgs);
                        }

                        // Reduce the money
                        _postOfficeService.SendMail(this.LetterboxName, reduceMoneyArgs);
                    }
                    

                    break;
            }
        }
    }
}
