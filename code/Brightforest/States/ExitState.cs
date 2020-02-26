using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces.StateHandling;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Brightforest.States
{
    public class ExitState : IState
    {
        private readonly Game1 _game;

        public string StateRegisterName { get; }

        public ExitState(Game1 game)
        {
            StateRegisterName = "Exit";
            _game = game;
        }

        public bool Cleanup()
        {
            return true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            return;
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
            _game.Quit();

            return;
        }
    }
}
