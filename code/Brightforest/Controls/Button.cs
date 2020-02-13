using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brightforest.EventArgs;
using Interfaces.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using IDrawable = Interfaces.Graphics.IDrawable;

namespace Brightforest.Controls
{
    public class Button : IDrawable, IUpdatable
    {
        private readonly string _text;
        private readonly Vector2 _position;
        private readonly Texture2D _buttonSprite;
        private readonly SpriteFont _font;
        private readonly Vector2 _textPosition;
        private readonly Rectangle _buttonCollisionRectangle;

        public Button(string text, Vector2 position, Texture2D buttonSprite, SpriteFont font)
        {
            // Store local variables passed in
            _text = text;
            _position = position;
            _buttonSprite = buttonSprite;
            _font = font;

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

        public void Update(MouseState mouseState, KeyboardState keyboardState)
        {
            // Set up mouse collision rectangle
            var mouseCollisionRectangle = new Rectangle(mouseState.X, mouseState.Y, 1, 1);

            if (mouseCollisionRectangle.Intersects(_buttonCollisionRectangle) && mouseState.LeftButton == ButtonState.Released)
            {
                // Collision occured, fire event
                OnClick(this, new OnClickEventArgs());
            }
        }

        #region OnClickEvent

        public delegate void OnClickHandler(object sender, OnClickEventArgs args);

        public event OnClickHandler OnClick;

        #endregion
    }
}
