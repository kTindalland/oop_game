using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brightforest.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Brightforest.Factories
{
    public class SquirrelFactory
    {
        private Texture2D _squirelTexture;

        public SquirrelFactory(Texture2D texture)
        {
            _squirelTexture = texture;
        }

        public Squirrel GenerateSquirrel(Vector2 position)
        {
            var result = new Squirrel(position, _squirelTexture);
            return result;
        }
    }
}
