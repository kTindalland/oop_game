using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brightforest.Controls;
using Brightforest.Factories;
using Interfaces.EventArguments;
using Interfaces.StateHandling;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Brightforest.States
{
    public class MenuState : IState
    {
        public string StateRegisterName { get; }
        private List<Button> _buttons;

        public MenuState(ButtonFactory buttonFactory)
        {
            StateRegisterName = "Menu";

            // Buttons
            _buttons = new List<Button>();

            // Play button
            var playArgs = new PostOfficeEventArgs()
            {
                SendAddress = "StateManager",
                MessageName = "ChangeState",
                Data = Encoding.ASCII.GetBytes("NameInput")
            };

            _buttons.Add( buttonFactory.GenerateButton("Play", 100, 100, playArgs) );

            // Leaderboard
            var leaderboardArgs = new PostOfficeEventArgs()
            {
                SendAddress = "StateManager",
                MessageName = "ChangeState",
                Data = Encoding.ASCII.GetBytes("Leaderboard")
            };

            _buttons.Add( buttonFactory.GenerateButton("Leaderboard", 100, 150, leaderboardArgs) );


            // Exit
            var exitArgs = new PostOfficeEventArgs()
            {
                SendAddress = "StateManager",
                MessageName = "ChangeState",
                Data = Encoding.ASCII.GetBytes("Exit")
            };

            _buttons.Add( buttonFactory.GenerateButton("Exit", 100, 200, exitArgs) );
        }

        public bool Cleanup()
        {
            return true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var button in _buttons)
            {
                button.Draw(spriteBatch);
            }
        }

        public bool Initialise()
        {
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
