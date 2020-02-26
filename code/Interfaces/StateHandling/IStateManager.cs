using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces.Graphics;

namespace Interfaces.StateHandling
{
    public interface IStateManager : IDrawable, IUpdateable
    {
        IState GetActiveState(); // Returns the currently active state
        void RegisterState(IState state); // Registers a state to the state manager
    }
}
