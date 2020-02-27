using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brightforest.Controls;
using Brightforest.Factories;
using Brightforest.Sprites;
using Interfaces.StateHandling;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Brightforest.States
{
    public class PlayState : IState
    {
        public string StateRegisterName { get; }
        
        private readonly SquirrelFactory _squirrelFactory;
        private Sprite _testSprite;
        private Texture2D _background;
        private readonly Rectangle _bounds;
        private UpgradesBar _upgradesBar;
        

        public PlayState(SquirrelFactory squirrelFactory, Texture2D background, Rectangle bounds, Texture2D upgradebar)
        {

            StateRegisterName = "Play";

            _squirrelFactory = squirrelFactory;
            _testSprite = squirrelFactory.GenerateSquirrel(new Vector2(700, 350));

            _background = background;
            _bounds = new Rectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height - 100);

            _upgradesBar = new UpgradesBar(new Vector2(0, _bounds.Height), new Rectangle(0,0, _bounds.Width, 100), upgradebar);
        }

        public bool Cleanup()
        {
            return true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_background, new Vector2(0,0), _bounds, Color.White);

            _testSprite.Draw(spriteBatch);
            _upgradesBar.Draw(spriteBatch);
        }

        public bool Initialise()
        {
            _testSprite.MoveTo(new Vector2(120, 350));
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
            _testSprite.Update(mouseState, keyboardState);
        }
    }
}
