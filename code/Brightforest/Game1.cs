using System.Diagnostics;
using System.Linq;
using System.Text;
using Brightforest.Controls;
using Brightforest.EventArgs;
using Brightforest.Factories;
using Brightforest.Managers;
using Brightforest.Schema;
using Brightforest.Services;
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

            _font = Content.Load<SpriteFont>("BlackChancery");
            _buttonTexture = Content.Load<Texture2D>("WoodButton");

            var buttonFactory = new ButtonFactory(_buttonTexture, _font, _postOffice);
            var textFactory = new TextFactory(_font);

            var leaderboardManager = new LeaderboardManager();

            _stateManager.RegisterState(new MenuState(buttonFactory));
            _stateManager.RegisterState(new NameInputState(buttonFactory));
            _stateManager.RegisterState(new LeaderboardState(buttonFactory, textFactory, leaderboardManager));

            _postOffice.RegisterClient((ILetterbox)_stateManager, "StateManager");

            _postOffice.SendMail("Null",
                new PostOfficeEventArgs()
                {
                    SendAddress = "StateManager",
                    MessageName = "SetInitialState",
                    Data = Encoding.ASCII.GetBytes("Menu")
                });

            // TODO: use this.Content to load your game content here
        }

        private void OnClick(object sender, OnClickEventArgs args)
        {
            Debug.WriteLine(args.Message);
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
            //_spriteBatch.Draw(_buttonTexture, new Vector2(90, 95), Color.White);
            //_spriteBatch.DrawString(_font, "Hello world", new Vector2(100, 100), Color.Black);

            _stateManager.Draw(_spriteBatch);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
