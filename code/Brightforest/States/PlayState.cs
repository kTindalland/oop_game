using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Brightforest.Controls;
using Brightforest.Factories;
using Brightforest.Managers;
using Brightforest.Sprites;
using Brightforest.Things;
using Interfaces.EventArguments;
using Interfaces.Services;
using Interfaces.StateHandling;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Brightforest.States
{
    public class PlayState : IState, ILetterbox
    {
        public string StateRegisterName { get; }
        public string LetterboxName => "PlayState";

        private readonly IPostOfficeService _postOfficeService;
        private readonly SquirrelFactory _squirrelFactory;
        private readonly ButtonFactory _buttonFactory;
        private readonly TextFactory _textFactory;
        private readonly StatsManager _statsManager;
        private readonly MoneyManager _moneyManager;
        private Texture2D _background;
        private readonly Texture2D _stone;
        private readonly Gate _gate;
        private readonly Rectangle _bounds;
        private UpgradesBar _upgradesBar;

        // All text
        private Text _amountOfArchers;
        private Text _rofLevel;
        private Text _damageLevel;
        private Text _money;
        private Text _waveText;
        private Text _gateHealth;

        private List<Squirrel> _enemies;
        private int _wave;
        private bool _inWave;

        private List<Archer> _archers;

        private List<Button> _upgradeButtons;

        private Button _quitButton;


        public PlayState(IPostOfficeService postOfficeService, SquirrelFactory squirrelFactory, ButtonFactory buttonFactory, TextFactory textFactory, StatsManager statsManager, MoneyManager moneyManager, Texture2D background, Rectangle bounds, Texture2D upgradebar, Texture2D stone, Gate gate)
        {
            _inWave = true;
            _wave = 0;
            _enemies = new List<Squirrel>();
            _archers = new List<Archer>();
            

            StateRegisterName = "Play";
            _postOfficeService = postOfficeService;
            _squirrelFactory = squirrelFactory;
            _buttonFactory = buttonFactory;
            _textFactory = textFactory;
            _statsManager = statsManager;
            _moneyManager = moneyManager;
            _background = background;
            _stone = stone;
            _gate = gate;
            _bounds = new Rectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height - 100);

            _upgradesBar = new UpgradesBar(new Vector2(0, _bounds.Height), new Rectangle(0,0, _bounds.Width, 100), upgradebar);


            #region UpgradeButtons

            _upgradeButtons = new List<Button>();

            // Add archer
            var addArcherArgs = new PostOfficeEventArgs()
            {
                SendAddress = "PlayState",
                MessageName = "AddArcher",
                Data = new byte[0]
            };

            _upgradeButtons.Add(_buttonFactory.GenerateButton("New Archer", 30, _bounds.Height + 5, addArcherArgs));

            // Increase Rate of Fire

            var rateOfFireArgs = new PostOfficeEventArgs()
            {
                SendAddress = "StatsManager",
                MessageName = "ModifyRateOfFire",
                Data = new byte[0]
            };

            _upgradeButtons.Add(_buttonFactory.GenerateButton("RoF++", 230, _bounds.Height + 5, rateOfFireArgs));

            // Increase damage modifier

            var damageArgs = new PostOfficeEventArgs()
            {
                SendAddress = "StatsManager",
                MessageName = "ModifyArrowDamage",
                Data = new byte[0]
            };

            _upgradeButtons.Add(_buttonFactory.GenerateButton("Damage++", 430, _bounds.Height +5, damageArgs));

            #endregion

            
            #region Text

            // Generate the text objects

            _amountOfArchers =
                _textFactory.GenerateText($"{_archers.Count.ToString()} Archer(s)", 30, _bounds.Height + 55);

            _rofLevel = _textFactory.GenerateText($"Level {_statsManager.RateOfFireLevel}", 230, _bounds.Height + 55);

            _damageLevel =
                _textFactory.GenerateText($"{_statsManager.DamageModifier}x Damage", 430, _bounds.Height + 55);

            _money = _textFactory.GenerateText($"Current Funds: M{_moneyManager.Money}", 20, 20);

            _waveText = _textFactory.GenerateText($"Wave {_wave}", 20, 70);

            _gateHealth = _textFactory.GenerateText($"Gate Health: {_gate.Health}", 20, 120);

            #endregion

            #region Quit Button

            var quitArgs = new PostOfficeEventArgs()
            {
                SendAddress = "StateManager",
                MessageName = "ChangeState",
                Data = Encoding.ASCII.GetBytes("Menu")
            };

            _quitButton = buttonFactory.GenerateButton("Quit", 600, 20, quitArgs);

            #endregion
        }

        public bool Cleanup()
        {


            return true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_background, new Vector2(0,0), _bounds, Color.White);

            // Draw the enemies if in a wave
            if (_inWave)
            {
                foreach (var squirrel in _enemies)
                {
                    squirrel.Draw(spriteBatch);
                }
            }

            // Draw all the archers
            foreach (var archer in _archers)
            {
                archer.Draw(spriteBatch);
            }

            
            // draw the upgrades bar
            _upgradesBar.Draw(spriteBatch);

            // Draw the upgrades button
            foreach (var upgradeButton in _upgradeButtons)
            {
                upgradeButton.Draw(spriteBatch);
            }

            // Draw the quit button
            _quitButton.Draw(spriteBatch);

            // Draw all the text stuff
            _rofLevel.Draw(spriteBatch);
            _amountOfArchers.Draw(spriteBatch);
            _damageLevel.Draw(spriteBatch);
            _money.Draw(spriteBatch);
            _waveText.Draw(spriteBatch);
            _gateHealth.Draw(spriteBatch);
        }

        public bool Initialise()
        {
            // Reset all the things
            _archers = new List<Archer>();

            AddArcher(); // Add single archer to start

            foreach (var upgradeButton in _upgradeButtons)
            {
                upgradeButton.CanUpdate = true;
            }

            // Reset the stats
            _statsManager.Reset();

            // Reset the enemies stuff
            _inWave = true;
            _wave = 0;
            _enemies = new List<Squirrel>();

            _moneyManager.Reset();

            _gate.Reset();

            return true;
        }

        private void AddArcher()
        {
            // Add an archer
            var newArcher = new Archer(new Vector2(125, 350), _stone, _statsManager);
            _archers.Add(newArcher);
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
            // Update all the text
            _amountOfArchers.DisplayText = $"{_archers.Count.ToString()} Archer(s)";
            _rofLevel.DisplayText = $"Level {_statsManager.RateOfFireLevel}";
            _damageLevel.DisplayText = $"{_statsManager.DamageModifier}x Damage";
            _money.DisplayText = $"Current Funds: M{_moneyManager.Money}";
            _waveText.DisplayText = $"Wave {_wave}";
            _gateHealth.DisplayText = $"Gate Health: {_gate.Health}";

            // Update all the upgrade buttons
            foreach (var upgradeButton in _upgradeButtons)
            {
                upgradeButton.Update(mouseState, keyboardState);
            }

            // Update the quit button
            _quitButton.Update(mouseState, keyboardState);

            // Update each archer
            foreach (var archer in _archers)
            {
                archer.Update(mouseState, keyboardState);
            }

            // If in a wave
            if (_inWave)
            {
                
                // Go through all enemies, update, if dead add to list
                var deadSquirrels = new List<Squirrel>();
                foreach (var squirrel in _enemies)
                {
                    squirrel.Update(mouseState, keyboardState);

                    if (!squirrel.Alive)
                    {
                        deadSquirrels.Add(squirrel);
                    }
                }

                // 15 money per squirrel
                var newMoneyArgs = new PostOfficeEventArgs()
                {
                    SendAddress = "MoneyManager",
                    MessageName = "AddMoney",
                    Data = BitConverter.GetBytes(15 * deadSquirrels.Count)
                };
                _postOfficeService.SendMail(this.LetterboxName, newMoneyArgs);

                // Add points 10 points per squirrel
                var newScoreArgs = new PostOfficeEventArgs()
                {
                    SendAddress = "PlayerMetaData",
                    MessageName = "AddScore",
                    Data = BitConverter.GetBytes(10 * deadSquirrels.Count)
                };
                _postOfficeService.SendMail(this.LetterboxName, newScoreArgs);
                
                foreach (var deadSquirrel in deadSquirrels)
                {
                    _enemies.Remove(deadSquirrel);
                }

                // If theres enemies
                if (_enemies.Count > 0)
                {
                    // order the squillows
                    var orderedSquirrels = _enemies.OrderBy(r => r.Position.X);

                    foreach (var archer in _archers)
                    {
                        // One of first 5
                        var firstX = 10;

                        var amount = orderedSquirrels.Count();
                        amount = amount >= firstX ? firstX : amount;

                        Random r = new Random((int)DateTime.Now.Ticks);
                        var index = r.Next(0, amount);

                        archer.FireArrow(orderedSquirrels.ToArray()[index]);
                    }
                }
            }
            
            // If all enemies are dead
            if (_enemies.Count == 0)
            {
                _wave++;
                _inWave = false;
                GenerateEnemies();
            }

            // If the gate is dead
            if (_gate.Health <= 0)
            {
                var changeStateArgs = new PostOfficeEventArgs()
                {
                    SendAddress = "StateManager",
                    MessageName = "ChangeState",
                    Data = Encoding.ASCII.GetBytes("Lose")
                };
                _postOfficeService.SendMail(this.LetterboxName, changeStateArgs);
            }
        }

        // Generate the enemies
        private void GenerateEnemies()
        {
            var rand = new Random((int)DateTime.Now.Ticks);
            _enemies = new List<Squirrel>();

            // Use fibbanacii numbers for wave amounts
            for (int i = 0; i < Fibbanacii(_wave + 2); i++)
            {
                int dist = rand.Next(-10, 11);

                var squirrel = _squirrelFactory.GenerateSquirrel(new Vector2(700, 350 + dist));
                squirrel.MoveTo(new Vector2(125 + dist/2, 350));

                squirrel.Speed = rand.Next(1, 5);

                _enemies.Add(squirrel);
            }

            // Set timer for when round to start
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

        public void LetterBox(string returnAddress, PostOfficeEventArgs args)
        {
            switch (args.MessageName)
            {
                case "AddArcher":
                    // Check money
                    if (_moneyManager.Money >= 20)
                    {
                        // Add archer
                        AddArcher();

                        _moneyManager.Money -= 20;
                    }
                    

                    break;
            }
        }
    }
}
