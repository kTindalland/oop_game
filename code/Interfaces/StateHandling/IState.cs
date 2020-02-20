using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces.Graphics;

namespace Interfaces.StateHandling
{
    public interface IState : IDrawable, IUpdateable
    {
        bool Initialise(); // Bool to confirm initialisation has completed properly
        bool Stop(); // Bool to confirm state has ended properly
        bool Start(); // Start the state
        bool Cleanup(); // Cleanup and reset the state.

        string StateRegisterName { get; } // Name in state register
    }
}
