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

        // Ready to attack
        private bool _ready;

        // If the squirrel is alive
        public bool Alive
        {
            get { return _alive; }
        }

        public Squirrel(Vector2 position, Texture2D sprite, Gate gate) : base(position, sprite)
        {
            // Default values
            _health = 10;
            _alive = true;
            _gate = gate;
            _ready = true;
        }

        public void InflictDamage(int amount)
        {
            // Giving the squirrel damage
            _health -= amount;
            _alive = _health > 0;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_alive) // If it's alive, draw
            {
                base.Draw(spriteBatch);
            }

        }

        public override void Update(MouseState mouseState, KeyboardState keyboardState)
        {
            if (_alive) // If it's alive, update
            {
                base.Update(mouseState, keyboardState);

                if (!base.IsMoving) // If not moving then got to the end
                {
                    if (_ready)
                    {
                        // Reduce gate health by the amount
                        _gate.Health -= 15;

                        // Set a timer off for when it can attack again.
                        Timer timer = new Timer(1000);
                        timer.Elapsed += OnTimer;
                        timer.Start();

                        // now not ready to attack.
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
