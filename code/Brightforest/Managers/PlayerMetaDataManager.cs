using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brightforest.Schema;
using Interfaces.EventArguments;
using Interfaces.Services;

namespace Brightforest.Managers
{
    public class PlayerMetaDataManager : ILetterbox
    {
        public string LetterboxName { get; set; }

        private Score _playerData;
        private readonly IPostOfficeService _postOfficeService;

        public PlayerMetaDataManager(IPostOfficeService postOfficeService)
        {
            LetterboxName = "PlayerMetaData";

            // Initialises the player data.
            _playerData = new Score()
            {
                Name = "Name Not Set",
                PlayerScore = 0
            };
            _postOfficeService = postOfficeService;
        }

        public void LetterBox(string returnAddress, PostOfficeEventArgs args)
        {
            // All in and out messages
            switch (args.MessageName)
            {
                // external power changes the current name
                case "ChangeName":
                    var newName = Encoding.ASCII.GetString(args.Data);

                    _playerData.Name = newName;

                    break;

                // External power adds value to the current score.
                case "AddScore":

                    var newScore = BitConverter.ToInt32(args.Data, 0);

                    _playerData.PlayerScore += newScore;

                    break;

                // Sends out current name in letter
                case "RequestName":

                    var reqArgs = new PostOfficeEventArgs()
                    {
                        SendAddress = returnAddress,
                        MessageName = "GetPlayerName",
                        Data = Encoding.ASCII.GetBytes(_playerData.Name)
                    };

                    _postOfficeService.SendMail(this.LetterboxName, reqArgs);

                    break;

                // Sends out current score in letter
                case "RequestScore":

                    var reqScoreArgs = new PostOfficeEventArgs()
                    {
                        SendAddress = returnAddress,
                        MessageName = "GetPlayerScore",
                        Data = BitConverter.GetBytes(_playerData.PlayerScore)
                    };

                    _postOfficeService.SendMail(this.LetterboxName, reqScoreArgs);

                    break;
            }
        }
    }
}
