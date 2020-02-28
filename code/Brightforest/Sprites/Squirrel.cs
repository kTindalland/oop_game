using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Brightforest.Sprites
{
    public class Squirrel : Sprite
    {
        private int _health;
        private bool _alive;
        private readonly Gate _gate;
        private bool _ready;

        public bool Alive
        {
            get { return _alive; }
        }

        public Squirrel(Vector2 position, Texture2D sprite, Gate gate) : base(position, sprite)
        {
            _health = 10;
            _alive = true;
            _gate = gate;
            _ready = true;
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

                if (!base.IsMoving) // If not moving then got to the end
                {
                    if (_ready)
                    {
                        _gate.Health -= 15;

                        Timer timer = new Timer(1000);
                        timer.Elapsed += OnTimer;
                        timer.Start();

                        _ready = false;
                    }
                }
            }
            
        }

        private void OnTimer(object sender, ElapsedEventArgs e)
        {
            ((Timer)sender).Stop();
            _ready = true;
        }
    }
}
