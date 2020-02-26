using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Brightforest.Helpers
{
    public class NameInputHelper : IUpdateable
    {
        private char[] _name;
        private bool _ready;
        private int _nameLen;
        private int _maxLen;

        public NameInputHelper(int length)
        {
            // Initialise all fields
            _name = new char[length];
            _ready = true;

            _nameLen = 0;
            _maxLen = length;
        }

        public void Reset()
        {
            // Reinitialise all fields
            _name = new char[_maxLen];
            _nameLen = 0;
            _ready = true;
        }

        public void Update(MouseState mouseState, KeyboardState keyboardState)
        {
            
            var pressedKeys = keyboardState.GetPressedKeys();

            if (pressedKeys.Length > 0) // If keydown
            {
                if (_ready)
                {
                    _ready = false;

                    var keyPressed = pressedKeys[0].ToString();

                    if (keyPressed.Length == 1) // Is char
                    {
                        AddCharacter(keyPressed.ToCharArray()[0]);
                    }

                    if (keyPressed.Length == 2) // Is number
                    {
                        AddCharacter(keyPressed.ToCharArray()[1]);
                    }

                    if (keyPressed == "Back") // Backspace
                    {
                        RemoveCharacter();
                    }
                }
            }
            else // If keyup
            {
                _ready = true;
            }
            
        }

        
        public string GetName()
        {
            // Put the name together
            var stringBuilder = new StringBuilder();

            for (int i = 0; i < _nameLen; i++)
            {
                stringBuilder.Append(_name[i]);
            }

            return stringBuilder.ToString();
        }

        private void AddCharacter(char character)
        {
            // Check theres space
            if (_nameLen >= _maxLen)
            {
                return;
            }

            // Add character
            _name[_nameLen] = character;
            _nameLen++;
        }

        private void RemoveCharacter()
        {
            // Check theres space
            if (_nameLen <= 0)
            {
                return;
            }
            
            // Remove character
            _nameLen--;
        }
    }
}
