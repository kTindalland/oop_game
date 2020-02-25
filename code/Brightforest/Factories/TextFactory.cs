using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brightforest.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brightforest.Factories
{
    public class TextFactory
    {
        private readonly SpriteFont _font;

        public TextFactory(SpriteFont font)
        {
            _font = font;
        }

        public Text GenerateText(string text, int x, int y)
        {
            return new Text(_font, text, x, y);
        }

        public Text GenerateText(string text, Vector2 position)
        {
            return new Text(_font, text, position);
        }
    }
}
