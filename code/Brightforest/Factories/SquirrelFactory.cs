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
        private readonly Gate _gate;

        public SquirrelFactory(Texture2D texture, Gate gate)
        {
            _squirelTexture = texture;
            _gate = gate;
        }

        public Squirrel GenerateSquirrel(Vector2 position)
        {
            // Create a new squirrel so I don't have to
            var result = new Squirrel(position, _squirelTexture, _gate);
            return result;
        }
    }
}
