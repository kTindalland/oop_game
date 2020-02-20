using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace Interfaces.Graphics
{
    public interface IUpdateable
    {
        void Update(MouseState mouseState, KeyboardState keyboardState);
    }
}
