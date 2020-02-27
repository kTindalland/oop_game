using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Brightforest.Sprites
{
    public class Squirrel : Sprite
    {
        public Squirrel(Vector2 position, Texture2D sprite) : base(position, sprite)
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void Update(MouseState mouseState, KeyboardState keyboardState)
        {
            base.Update(mouseState, keyboardState);
        }
    }
}
