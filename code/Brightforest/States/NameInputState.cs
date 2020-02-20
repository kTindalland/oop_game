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
    public class NameInputState : IState
    {
        public string StateRegisterName { get; }
        private List<Button> _buttons;

        public NameInputState(ButtonFactory buttonFactory)
        {
            StateRegisterName = "NameInput";

            // Buttons
            PostOfficeEventArgs args;
            _buttons = new List<Button>();

            // Back button
            args = new PostOfficeEventArgs()
            {
                SendAddress = "StateManager",
                MessageName = "ChangeState",
                Data = Encoding.ASCII.GetBytes("Menu")
            };

            _buttons.Add(buttonFactory.GenerateButton("Back", 100, 100, args));
            
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
