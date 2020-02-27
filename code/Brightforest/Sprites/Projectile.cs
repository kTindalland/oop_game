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
    public class Projectile : Sprite
    {
        private readonly Squirrel _enemy;

        public Squirrel Enemy
        {
            get { return _enemy; }
        }

        public Projectile(Vector2 position, Texture2D texture, Squirrel enemy) : base(position, texture)
        {
            _enemy = enemy;
            base.MoveTo(_enemy.Position);
        }

        public override void Update(MouseState mouseState, KeyboardState keyboardState)
        {
            if (IsMoving)
            {
                base.MoveTo(_enemy.Position);
            }

            base.Update(mouseState, keyboardState);
        }
    }
}
