using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brightforest.Controls;
using Interfaces.EventArguments;
using Interfaces.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brightforest.Factories
{
    public class ButtonFactory
    {
        private IPostOfficeService _postOffice;
        private Texture2D _defaultTexture;
        private SpriteFont _font;

        public ButtonFactory(Texture2D defaultTexture, SpriteFont font, IPostOfficeService postOffice)
        {
            _defaultTexture = defaultTexture;
            _font = font;
            _postOffice = postOffice;
        }

        public Button GenerateButton(string text, Vector2 position)
        {
            var button = new Button(text, position, _defaultTexture, _font, _postOffice);

            return button;
        }

        public Button GenerateButton(string text, Vector2 position, PostOfficeEventArgs args)
        {
            var button = new Button(text, position, _defaultTexture, _font, _postOffice, args);

            return button;
        }

        public Button GenerateButton(string text, int x, int y)
        {
            var position = new Vector2(x, y);
            var button = new Button(text, position, _defaultTexture, _font, _postOffice);

            return button;
        }

        public Button GenerateButton(string text, int x, int y, PostOfficeEventArgs args)
        {
            var position = new Vector2(x, y);
            var button = new Button(text, position, _defaultTexture, _font, _postOffice, args);

            return button;
        }
    }
}
