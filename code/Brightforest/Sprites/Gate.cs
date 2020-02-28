using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces.EventArguments;
using Interfaces.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Brightforest.Sprites
{
    public class Gate : ILetterbox
    {

        private int _health;

        // Public face to remaining health
        public int Health
        {
            get { return _health; }
            set { _health = value; }
        }

        public Gate()
        {
            Reset();
        }

        public void Reset()
        {
            _health = 500;
        }

        public void LetterBox(string returnAddress, PostOfficeEventArgs args)
        {
            switch (args.MessageName)
            {
                // Take health off the gate
                case "DamageHealth":

                    var damage = BitConverter.ToInt32(args.Data, 0);
                    _health -= damage;

                    break;
            }
        }
    }
}
