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
            _defaultTexture = defaultTexture; // The texture this factory uses
            _font = font; // The font this facotry uses
            _postOffice = postOffice; // Give the buttons this post office object
        }

        // Get button with no args, doesn't really do much, just looks like a button
        public Button GenerateButton(string text, Vector2 position)
        {
            var button = new Button(text, position, _defaultTexture, _font, _postOffice);

            return button;
        }

        // With args, uses post office to get functionality
        public Button GenerateButton(string text, Vector2 position, PostOfficeEventArgs args)
        {
            var button = new Button(text, position, _defaultTexture, _font, _postOffice, args);

            return button;
        }

        // Only difference with these two is it makes the vector 2 for you, very useful 10/10 IGN
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
