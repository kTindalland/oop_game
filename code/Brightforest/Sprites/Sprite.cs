using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Brightforest.Sprites.Structs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using IDrawable = Interfaces.Graphics.IDrawable;
using IUpdateable = Interfaces.Graphics.IUpdateable;

namespace Brightforest.Sprites
{
    public class Sprite : IDrawable, IUpdateable
    {
        private MoveData _moveData;
        private Vector2 _position;
        private Texture2D _texture;

        // If the sprite is moving
        public bool IsMoving
        {
            get { return _moveData.IsMoving; }
        }

        // The sprites position
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        // the sprites speed
        public int Speed
        {
            get { return _moveData.Speed; }
            set { _moveData.Speed = value; }
        }

        public Sprite(Vector2 position, Texture2D texture)
        {
            // Default move data
            _moveData = new MoveData()
            {
                IsMoving = false,
                Speed = 3,
                MoveDestination = new Vector2(0, 0)
            };

            // Set starting position
            _position = position;

            _texture = texture;
        }

        public void MoveTo(Vector2 dest)
        {
            // Start the sprite moving
            _moveData.IsMoving = true;
            _moveData.MoveDestination = dest;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, Color.White);
            return;
        }

        public virtual void Update(MouseState mouseState, KeyboardState keyboardState)
        {
            if (_moveData.IsMoving) // Execute move logic
            {
                // Update cycle is 1/60th of a second.
                var pixelsPerSecond = _moveData.Speed * 10;
                double movePixels = (double)pixelsPerSecond / 60.00;

                // Get full difference.

                var difference = _moveData.MoveDestination - _position;

                var differenceMagnitude = Math.Sqrt((Math.Pow(difference.X, 2) + (Math.Pow(difference.Y, 2))));

                // Validate magnitudeRatio
                var differenceBetweenMagAndMove = Math.Abs(differenceMagnitude - movePixels);
                if (differenceBetweenMagAndMove <= 1.5)
                {
                    differenceMagnitude = movePixels;
                }

                var magnitudeRatio = differenceMagnitude / movePixels;

                var step = new Vector2(difference.X / (float)magnitudeRatio, difference.Y / (float)magnitudeRatio);

                // Add the step
                _position += step;


                // If very close.
                if (differenceMagnitude < 15)
                {
                    _position = _moveData.MoveDestination;
                    _moveData.IsMoving = false;
                }
            }

            return;
        }
    }
}
