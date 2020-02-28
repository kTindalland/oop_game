using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brightforest.Controls;
using Brightforest.Factories;
using Brightforest.Managers;
using Interfaces.EventArguments;
using Interfaces.Services;
using Interfaces.StateHandling;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Brightforest.States
{
    public class LoseState : IState, ILetterbox
    {
        private readonly ButtonFactory _buttonFactory;
        private readonly TextFactory _textFactory;
        private readonly IPostOfficeService _postOfficeService;
        private readonly LeaderboardManager _leaderboardManager;
        private long _playerScore;
        private string _playername;
        private bool _ready;

        public string StateRegisterName => "Lose";

        private Button _menuButton;

        public LoseState(ButtonFactory buttonFactory, TextFactory textFactory, IPostOfficeService postOfficeService, LeaderboardManager leaderboardManager)
        {
            _buttonFactory = buttonFactory;
            _textFactory = textFactory;
            _postOfficeService = postOfficeService;
            _leaderboardManager = leaderboardManager;
            _playerScore = 0L;

            // Check for 123456 because user cannot enter this
            _playername = "123456";

            // Ready to write to file.
            _ready = true;

            #region Button Stuff

            // Menu button stuff
            var menuArgs = new PostOfficeEventArgs()
            {
                SendAddress = "StateManager",
                MessageName = "ChangeState",
                Data = Encoding.ASCII.GetBytes("Menu")
            };

            _menuButton = _buttonFactory.GenerateButton("Menu", 20, 400, menuArgs);

            #endregion

        }

        private List<Text> GenerateTexts(List<string> lines)
        {
            // Create loads of text objects because it's a pain
            var textArray = lines.ToArray();

            // Offsets
            var startY = 20;
            var x = 20;

            // Add all the results
            var result = new List<Text>();
            for (int i = 0; i < textArray.Length; i++)
            {
                result.Add(_textFactory.GenerateText(textArray[i], x, startY + (i * 50)));
            }

            return result;
        }

        public bool Cleanup()
        {
            return true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Getting all entries and sorting them, then finding out where the player ranked.
            var allEntries = _leaderboardManager.GetAllScores();

            allEntries = allEntries.OrderByDescending(r => r.PlayerScore).ToList();

            // get the current players record
            var thisRecord = allEntries.Where(r => r.PlayerScore == _playerScore && r.Name == _playername).ToList();

            // Get the position
            int position;
            if (thisRecord.Count > 0)
            {
                position = allEntries.IndexOf(thisRecord[0]) + 1;
            }
            else
            {
                position = -1;
            }

            // All the lines
            var lines = new List<string>()
            {
                "You Lose!",
                $"Your score is {_playerScore}",
                $"Thankyou for playing, {_playername}",
                $"You came {position} position on our leaderboard."
            };

            // Generate the text objects
            var texts = GenerateTexts(lines);

            // Draw the text
            foreach (var text in texts)
            {
                text.Draw(spriteBatch);
            }

            _menuButton.Draw(spriteBatch);
        }

        public bool Initialise()
        {
            // Reset stuff
            _playerScore = 0L;
            _playername = "123456";
            _ready = true;

            #region Getting Player Meta Data

            var getNameArgs = new PostOfficeEventArgs()
            {
                SendAddress = "PlayerMetaData",
                MessageName = "RequestName",
                Data = new byte[0]
            };

            var getScoreArgs = new PostOfficeEventArgs()
            {
                SendAddress = "PlayerMetaData",
                MessageName = "RequestScore",
                Data = new byte[0]
            };

            _postOfficeService.SendMail(this.StateRegisterName, getNameArgs);
            _postOfficeService.SendMail(this.StateRegisterName, getScoreArgs);

            #endregion

            return true;
        }

        public bool Start()
        {
            return true;
        }

        public bool Stop()
        {
            return true;
        }

        public void Update(MouseState mouseState, KeyboardState keyboardState)
        {
            // Check if it hasn't been written already
            if (_playername != "123456" && _ready)
            {
                _leaderboardManager.AddScore(_playername, _playerScore);
                _ready = false;
            }

            // Update menu button
            _menuButton.Update(mouseState, keyboardState);

            return;
        }

        public void LetterBox(string returnAddress, PostOfficeEventArgs args)
        {
            // Sort the messages for this state
            switch (args.MessageName)
            {
                case "GetPlayerName":

                    var name = Encoding.ASCII.GetString(args.Data);
                    _playername = name;

                    break;

                case "GetPlayerScore":

                    var score = BitConverter.ToInt64(args.Data, 0);
                    _playerScore = score;

                    break;
            }
        }
    }
}
