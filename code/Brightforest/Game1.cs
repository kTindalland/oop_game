using System.Diagnostics;
using System.Linq;
using System.Text;
using Brightforest.Controls;
using Brightforest.Factories;
using Brightforest.Managers;
using Brightforest.Schema;
using Brightforest.Services;
using Brightforest.Sprites;
using Brightforest.States;
using Interfaces.EventArguments;
using Interfaces.Services;
using Interfaces.StateHandling;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XML_Handling;
using IUpdateable = Interfaces.Graphics.IUpdateable;

namespace Brightforest
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        private SpriteFont _font;
        private Texture2D _buttonTexture;

        private IPostOfficeService _postOffice;
        private IStateManager _stateManager;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            _postOffice = new PostOfficeService();

            _stateManager = new StateManager(_postOffice);

            IsFixedTimeStep = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here


            IsMouseVisible = true;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load textures and fonts
            _font = Content.Load<SpriteFont>("BlackChancery");
            _buttonTexture = Content.Load<Texture2D>("WoodButton");
            var squirrel1 = Content.Load<Texture2D>("squirrel1");
            var gateLevel = Content.Load<Texture2D>("gate_level");

            var upgradeBar = new Texture2D(GraphicsDevice, GraphicsDevice.PresentationParameters.Bounds.Width, 100);

            var colorData = new Color[100 * GraphicsDevice.PresentationParameters.Bounds.Width];
            for (int i = 0; i < 100 * GraphicsDevice.PresentationParameters.Bounds.Width; i++)
            {
                colorData[i] = Color.DarkSlateGray;
            }
            upgradeBar.SetData(colorData);

            var stone = new Texture2D(GraphicsDevice, 10, 10);
            var stoneColors = new Color[100];
            for (int i = 0; i < 100; i++)
            {
                stoneColors[i] = Color.Black;
            }

            stone.SetData(stoneColors);

            // Create Objects
            var gate = new Gate();

            // Create factories
            var buttonFactory = new ButtonFactory(_buttonTexture, _font, _postOffice);
            var textFactory = new TextFactory(_font);
            var squirrelFactory = new SquirrelFactory(squirrel1, gate);

            
            // Create Managers
            var leaderboardManager = new LeaderboardManager();
            var playerMetaDataManager = new PlayerMetaDataManager(_postOffice);

            var moneyManager = new MoneyManager(_postOffice);
            var statsManager = new StatsManager(_postOffice, moneyManager);

            // Register states to the state manager
            _stateManager.RegisterState(new MenuState(buttonFactory, textFactory));

            var nameInputState = new NameInputState(buttonFactory, textFactory, _postOffice,
                GraphicsDevice.PresentationParameters.Bounds);
            _stateManager.RegisterState(nameInputState);
            _stateManager.RegisterState(new LeaderboardState(buttonFactory, textFactory, leaderboardManager));
            _stateManager.RegisterState(new ExitState(this));

            var playState = new PlayState(_postOffice, squirrelFactory, buttonFactory, textFactory, statsManager, moneyManager, gateLevel,
                GraphicsDevice.PresentationParameters.Bounds, upgradeBar, stone, gate);

            _stateManager.RegisterState(playState);

            var loseState = new LoseState(buttonFactory, textFactory, _postOffice, leaderboardManager);

            _stateManager.RegisterState(loseState);

            var flavourText = new FlavourTextState(buttonFactory, textFactory);
            _stateManager.RegisterState(flavourText);


            // Register clients to the post office
            _postOffice.RegisterClient((ILetterbox)_stateManager, "StateManager");
            _postOffice.RegisterClient((ILetterbox) playerMetaDataManager, playerMetaDataManager.LetterboxName);
            _postOffice.RegisterClient((ILetterbox) nameInputState, nameInputState.StateRegisterName);
            _postOffice.RegisterClient((ILetterbox) statsManager, statsManager.LetterboxName);
            _postOffice.RegisterClient((ILetterbox) playState, playState.LetterboxName);
            _postOffice.RegisterClient((ILetterbox) moneyManager, moneyManager.LetterboxName);
            _postOffice.RegisterClient((ILetterbox) gate, "Gate");
            _postOffice.RegisterClient((ILetterbox) loseState, loseState.StateRegisterName);

            // Set the initial state to the menu
            _postOffice.SendMail("Null",
                new PostOfficeEventArgs()
                {
                    SendAddress = "StateManager",
                    MessageName = "SetInitialState",
                    Data = Encoding.ASCII.GetBytes("Menu")
                });

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            var mouseState = Mouse.GetState();
            var keyboardState = Keyboard.GetState();

            // Update current state
            _stateManager.Update(mouseState, keyboardState);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            
            // Draw current state
            _stateManager.Draw(_spriteBatch);

            _spriteBatch.End();
            base.Draw(gameTime);
        }

        public void Quit()
        {
            this.Exit();
        }
    }
}
