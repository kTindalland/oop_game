using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brightforest.Controls;
using Brightforest.Factories;
using Brightforest.Managers;
using Brightforest.Schema;
using Interfaces.EventArguments;
using Interfaces.StateHandling;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XML_Handling;

namespace Brightforest.States
{
    public class LeaderboardState : IState
    {
        private readonly ButtonFactory _buttonFactory;
        private readonly TextFactory _textFactory;
        private readonly LeaderboardManager _leaderboardManager;

        private List<Button> _buttons;
        private List<Text> _text;

        public string StateRegisterName { get; }

        public LeaderboardState(ButtonFactory buttonFactory, TextFactory textFactory, LeaderboardManager leaderboardManager)
        {
            StateRegisterName = "Leaderboard";

            // Factories and managers
            _buttonFactory = buttonFactory;
            _textFactory = textFactory;
            _leaderboardManager = leaderboardManager;

            // Lists
            _text = new List<Text>();
            _buttons = new List<Button>();

            // Change the state to the menu
            var buttonArgs = new PostOfficeEventArgs()
            {
                SendAddress = "StateManager",
                MessageName = "ChangeState",
                Data = Encoding.ASCII.GetBytes("Menu")
            };

            _buttons.Add(_buttonFactory.GenerateButton("Back", 550, 400, buttonArgs));
        }

        public bool Cleanup()
        {
            // Reset all the text
            _text = new List<Text>();

            return true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw all the text and buttons
            foreach (var text in _text)
            {
                text.Draw(spriteBatch);
            }

            foreach (var button in _buttons)
            {
                button.Draw(spriteBatch);
            }
        }

        public bool Initialise()
        {
            // Get top ten
            var topTen = _leaderboardManager.GetTopTenScores();

            // Default placements
            var yOffset = 20;
            var xOffset = 30;
            var ySeperation = 40;

            // Go through each and generate the text
            for (int i = 0; i < 10; i++)
            {
                // Get the score
                var score = i < topTen.Count ? topTen[i] : new Score() { Name = "", PlayerScore = -1 };

                // Build the strings
                var stringBuilder = new StringBuilder();
                stringBuilder.Append($"{i + 1}. {score.Name}".PadRight(20));

                if (score.PlayerScore > -1) stringBuilder.Append($"{score.PlayerScore} Points");

                _text.Add(_textFactory.GenerateText(stringBuilder.ToString(), xOffset, yOffset + (ySeperation * i)));
            }

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
            foreach (var button in _buttons)
            {
                button.Update(mouseState, keyboardState);
            }
        }
    }
}
