using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IDrawable = Interfaces.Graphics.IDrawable;

namespace Brightforest.Controls
{
    public class UpgradesBar : IDrawable
    {
        private readonly Vector2 _position;
        private readonly Rectangle _bounds;
        private readonly Texture2D _texture;

        public UpgradesBar(Vector2 position, Rectangle bounds, Texture2D texture)
        {
            _position = position;
            _bounds = bounds;
            _texture = texture;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, _bounds, Color.White);
        }
    }
}
