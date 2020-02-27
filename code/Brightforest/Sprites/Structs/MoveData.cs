using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Brightforest.Sprites.Structs
{
    public struct MoveData
    {
        public bool IsMoving { get; set; }
        public int Speed { get; set; } // Every 1 speed is 10 pixels / second
        public Vector2 MoveDestination { get; set; }
    }
}
