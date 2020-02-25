using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IDrawable = Interfaces.Graphics.IDrawable;

namespace Brightforest.Controls
{
    public class Text : IDrawable
    {
        private string _displayText;
        private Vector2 _position;
        private Color _color;
        private readonly SpriteFont _font;
       

        public string DisplayText
        {
            get { return _displayText; }
            set { _displayText = value; }
        }

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }


        public Text(SpriteFont font, string displayText, Vector2 position)
        {
            DisplayText = displayText;
            _font = font;
            _position = position;
            _color = Color.Black;
        }

        public Text(SpriteFont font, string displayText, int x, int y)
        {
            DisplayText = displayText;
            _font = font;
            _position = new Vector2(x, y);
            _color = Color.Black;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, _displayText, _position, Color.Black);
        }
    }
}
