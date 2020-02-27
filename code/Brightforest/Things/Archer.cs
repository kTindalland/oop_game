using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
        private bool _ready;
        private List<Projectile> _projectiles;


        public Archer(Vector2 startPosition, Texture2D projectileTexture)
        {
            _startPosition = startPosition;
            _projectileTexture = projectileTexture;
            _ready = true;
            _projectiles = new List<Projectile>();
        }

        public void FireArrow(Squirrel enemy)
        {
            if (_ready)
            {
                var newProj = new Projectile(_startPosition, _projectileTexture, enemy);
                newProj.Speed = 10;

                _projectiles.Add(newProj);

                Random r = new Random((int)DateTime.Now.Ticks);
                Timer timer = new Timer(r.Next(500, 2000));
                timer.Elapsed += OnTimer;
                timer.Start();

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
            var spentProjectiles = new List<Projectile>();
            foreach (var projectile in _projectiles)
            {
                projectile.Update(mouseState, keyboardState);

                if (!projectile.IsMoving)
                {
                    Random r = new Random((int)DateTime.Now.Ticks);
                    projectile.Enemy.InflictDamage(r.Next(3, 5));
                    spentProjectiles.Add(projectile);
                }
            }

            foreach (var spentProjectile in spentProjectiles)
            {
                _projectiles.Remove(spentProjectile);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var projectile in _projectiles)
            {
                projectile.Draw(spriteBatch);
            }
        }
    }
}
