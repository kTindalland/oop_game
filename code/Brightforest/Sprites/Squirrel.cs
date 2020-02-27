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
        private int _health;
        private bool _alive;

        public bool Alive
        {
            get { return _alive; }
        }

        public Squirrel(Vector2 position, Texture2D sprite) : base(position, sprite)
        {
            _health = 10;
            _alive = true;
        }

        public void InflictDamage(int amount)
        {
            _health -= amount;
            _alive = _health > 0;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_alive)
            {
                base.Draw(spriteBatch);
            }
            
        }

        public override void Update(MouseState mouseState, KeyboardState keyboardState)
        {
            if (_alive)
            {
                base.Update(mouseState, keyboardState);
            }
            
        }
    }
}
