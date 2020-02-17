using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.StateHandling
{
    public interface IStateManager
    {
        IState GetActiveState();
    }
}
