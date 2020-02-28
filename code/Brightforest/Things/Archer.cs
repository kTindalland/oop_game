using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Brightforest.Managers;
using Brightforest.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using IDrawable = Interfaces.Graphics.IDrawable;
using IUpdateable = Interfaces.Graphics.IUpdateable;

namespace Brightforest.Things
{
    public class Archer : IUpdateable, IDrawable
    {
        private readonly Vector2 _startPosition;
        private readonly Texture2D _projectileTexture;
        private readonly StatsManager _statsManager;
        private bool _ready;
        private List<Projectile> _projectiles;


        public Archer(Vector2 startPosition, Texture2D projectileTexture, StatsManager statsManager)
        {
            // Set all the fields
            _startPosition = startPosition;
            _projectileTexture = projectileTexture;
            _statsManager = statsManager;
            _ready = true;
            _projectiles = new List<Projectile>();
        }

        public void FireArrow(Squirrel enemy)
        {
            if (_ready) // If archer is ready to fire arrow
            {
                // Fire a new projectile
                var newProj = new Projectile(_startPosition, _projectileTexture, enemy);
                newProj.Speed = 10;

                _projectiles.Add(newProj);

                // New random stuff
                Random r = new Random((int)DateTime.Now.Ticks);

                // Start a new timer for when archer can fire again
                var lowerBound = _statsManager.RateOfFire - 500 < 0 ? 50 : _statsManager.RateOfFire - 500;
                Timer timer = new Timer(r.Next(lowerBound, _statsManager.RateOfFire+500));
                timer.Elapsed += OnTimer;
                timer.Start();

                // Not ready anymore
                _ready = false;
            }
            
        }

        private void OnTimer(object sender, ElapsedEventArgs e)
        {
            _ready = true;

            ((Timer) sender).Stop();
        }

        public void Update(MouseState mouseState, KeyboardState keyboardState)
        {
            // Clean up projectiles
            var spentProjectiles = new List<Projectile>();
            foreach (var projectile in _projectiles)
            {
                // Update the projectiles
                projectile.Update(mouseState, keyboardState);

                // If projectile isn't moving
                if (!projectile.IsMoving)
                {
                    // Inflict some damage
                    Random r = new Random((int)DateTime.Now.Ticks);
                    projectile.Enemy.InflictDamage(r.Next(3 * _statsManager.DamageModifier, 5 * _statsManager.DamageModifier));
                    spentProjectiles.Add(projectile);
                }
            }

            // Remove the spent projectiles
            foreach (var spentProjectile in spentProjectiles)
            {
                _projectiles.Remove(spentProjectile);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw each prijectile
            foreach (var projectile in _projectiles)
            {
                projectile.Draw(spriteBatch);
            }
        }
    }
}
