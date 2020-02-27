using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Brightforest.Controls;
using Brightforest.Factories;
using Brightforest.Sprites;
using Brightforest.Things;
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
        private Texture2D _background;
        private readonly Texture2D _stone;
        private readonly Rectangle _bounds;
        private UpgradesBar _upgradesBar;

        private List<Squirrel> _enemies;
        private int _wave;
        private bool _inWave;

        private List<Archer> _archers;


        public PlayState(SquirrelFactory squirrelFactory, Texture2D background, Rectangle bounds, Texture2D upgradebar, Texture2D stone)
        {
            _inWave = true;
            _wave = 0;
            _enemies = new List<Squirrel>();
            _archers = new List<Archer>();
            

            StateRegisterName = "Play";

            _squirrelFactory = squirrelFactory;

            _background = background;
            _stone = stone;
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

            if (_inWave)
            {
                foreach (var squirrel in _enemies)
                {
                    squirrel.Draw(spriteBatch);
                }
            }

            foreach (var archer in _archers)
            {
                archer.Draw(spriteBatch);
            }

            _upgradesBar.Draw(spriteBatch);
        }

        public bool Initialise()
        {
            _archers = new List<Archer>();

            var firstArcher = new Archer(new Vector2(125, 350), _stone);

            _archers.Add(firstArcher);
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
            foreach (var archer in _archers)
            {
                archer.Update(mouseState, keyboardState);
            }

            if (_inWave)
            {
                

                var deadSquirrels = new List<Squirrel>();
                foreach (var squirrel in _enemies)
                {
                    squirrel.Update(mouseState, keyboardState);

                    if (!squirrel.Alive)
                    {
                        deadSquirrels.Add(squirrel);
                    }
                }

                foreach (var deadSquirrel in deadSquirrels)
                {
                    _enemies.Remove(deadSquirrel);
                }

                if (_enemies.Count > 0)
                {
                    var orderedSquirrels = _enemies.OrderBy(r => r.Position.X);

                    foreach (var archer in _archers)
                    {
                        archer.FireArrow(orderedSquirrels.First());
                    }
                }
            }
            

            if (_enemies.Count == 0)
            {
                _wave++;
                _inWave = false;
                GenerateEnemies();
            }
        }

        private void GenerateEnemies()
        {
            var rand = new Random((int)DateTime.Now.Ticks);
            _enemies = new List<Squirrel>();

            for (int i = 0; i < Fibbanacii(_wave); i++)
            {
                int dist = rand.Next(-10, 11);

                var squirrel = _squirrelFactory.GenerateSquirrel(new Vector2(700, 350 + dist));
                squirrel.MoveTo(new Vector2(125 + dist/2, 350));

                squirrel.Speed = rand.Next(1, 5);

                _enemies.Add(squirrel);
            }

            Timer waveTimer = new Timer();
            waveTimer.Interval = 5000;
            waveTimer.Elapsed += OnTimer;
            waveTimer.Start();

        }

        private void OnTimer(object sender, ElapsedEventArgs e)
        {
            _inWave = true;
            ((Timer) sender).Stop();
        }

        private int Fibbanacii(int number)
        {
            int n1 = 0, n2 = 1, n3=0, i;
            for (i = 2; i < number; ++i) //loop starts from 2 because 0 and 1 are already printed    
            {
                n3 = n1 + n2;
                n1 = n2;
                n2 = n3;
            }

            return n3;
        }


    }
}
