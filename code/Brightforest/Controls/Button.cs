using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brightforest.EventArgs;
using Interfaces.EventArguments;
using Interfaces.Graphics;
using Interfaces.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using IDrawable = Interfaces.Graphics.IDrawable;
using IUpdateable = Interfaces.Graphics.IUpdateable;

namespace Brightforest.Controls
{
    public class Button : IDrawable, IUpdateable, ILetterbox
    {
        private readonly string _text;
        private Vector2 _position;
        private readonly Texture2D _buttonSprite;
        private readonly SpriteFont _font;
        private Vector2 _textPosition;
        private Rectangle _buttonCollisionRectangle;
        private readonly IPostOfficeService _postOffice;
        private readonly Guid _guid;
        private readonly PostOfficeEventArgs _args;
        private bool _clickReady;

        public int Width
        {
            get { return _buttonSprite.Width; }
        }

        public Vector2 Position
        {
            get { return _position; }
            set
            {
                // Make sure to re-do all positioning and collision stuff
                _position = value;
                var textRadii = _font.MeasureString(_text) / 2;
                var buttonCenter = new Vector2(_buttonSprite.Width / 2, _buttonSprite.Height / 2);
                var textOffset = buttonCenter - textRadii;
                _textPosition = _position + textOffset;

                _buttonCollisionRectangle = new Rectangle((int)_position.X, (int)_position.Y, _buttonSprite.Width, _buttonSprite.Height);
            }
        }

        public Button(string text, Vector2 position, Texture2D buttonSprite, SpriteFont font, IPostOfficeService postOffice)
        {
            // Initialise local variables
            _clickReady = false;
            _guid = Guid.NewGuid();
            _args = new PostOfficeEventArgs();

            // Store local variables passed in
            _text = text;
            _position = position;
            _buttonSprite = buttonSprite;
            _font = font;
            _postOffice = postOffice;

            // Register at the post office
            _postOffice.RegisterClient(this, _guid.ToString());

            // Measure and center text
            var textRadii = _font.MeasureString(_text) / 2;
            var buttonCenter = new Vector2(_buttonSprite.Width / 2, _buttonSprite.Height / 2);
            var textOffset = buttonCenter - textRadii;
            _textPosition = _position + textOffset;

            // Set up rectangles for collision.
            _buttonCollisionRectangle = new Rectangle((int)_position.X, (int)_position.Y, _buttonSprite.Width, _buttonSprite.Height);
        }

        public Button(string text, Vector2 position, Texture2D buttonSprite, SpriteFont font, IPostOfficeService postOffice, PostOfficeEventArgs args)
        {
            // Initialise local variables
            _clickReady = false;
            _guid = Guid.NewGuid();

            // Store local variables passed in
            _text = text;
            _position = position;
            _buttonSprite = buttonSprite;
            _font = font;
            _postOffice = postOffice;
            _args = args;

            // Register at the post office
            _postOffice.RegisterClient(this, _guid.ToString());

            // Measure and center text
            var textRadii = _font.MeasureString(_text) / 2;
            var buttonCenter = new Vector2(_buttonSprite.Width / 2, _buttonSprite.Height / 2);
            var textOffset = buttonCenter - textRadii;
            _textPosition = _position + textOffset;

            // Set up rectangles for collision.
            _buttonCollisionRectangle = new Rectangle((int)_position.X, (int)_position.Y, _buttonSprite.Width, _buttonSprite.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_buttonSprite, _position, Color.White);
            spriteBatch.DrawString(_font, _text, _textPosition, Color.Black);
        }

        public void LetterBox(string returnAddress, PostOfficeEventArgs args)
        {
            return;
        }

        public void Update(MouseState mouseState, KeyboardState keyboardState)
        {
            // Set up mouse collision rectangle
            var mouseCollisionRectangle = new Rectangle(mouseState.X, mouseState.Y, 1, 1);

            if (mouseState.LeftButton == ButtonState.Pressed && mouseCollisionRectangle.Intersects(_buttonCollisionRectangle)) _clickReady = true;


            if (mouseState.LeftButton == ButtonState.Released && _clickReady)
            {
                if (mouseCollisionRectangle.Intersects(_buttonCollisionRectangle))
                {
                    // Collision occured, send letter
                    _postOffice.SendMail(_guid.ToString(), _args);

                    // Un-ready click
                    _clickReady = false;
                }
            }
        }
    }
}
