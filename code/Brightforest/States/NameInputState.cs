using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brightforest.Controls;
using Brightforest.Factories;
using Brightforest.Helpers;
using Interfaces.EventArguments;
using Interfaces.Services;
using Interfaces.StateHandling;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Brightforest.States
{
    public class NameInputState : IState, ILetterbox
    {
        public string StateRegisterName { get; }
        private List<Button> _buttons;
        private NameInputHelper _nameHelper;
        private readonly TextFactory _textFactory;
        private readonly IPostOfficeService _postOfficeService;
        private Text _name;
        private int _maxLen;

        public NameInputState(ButtonFactory buttonFactory, TextFactory textFactory, IPostOfficeService postOfficeService, Rectangle bounds)
        {
            // Initialise fields
            StateRegisterName = "NameInput";
            _maxLen = 5;

            // Set up buttons
            #region Buttons

            // Buttons
            PostOfficeEventArgs args;
            _buttons = new List<Button>();

            // Back button
            args = new PostOfficeEventArgs()
            {
                SendAddress = "StateManager",
                MessageName = "ChangeState",
                Data = Encoding.ASCII.GetBytes("Menu")
            };

            _buttons.Add(buttonFactory.GenerateButton("Back", 100, 100, args));

            // Play button
            var playArgs = new PostOfficeEventArgs()
            {
                SendAddress = "StateManager",
                MessageName = "ChangeState",
                Data = Encoding.ASCII.GetBytes("Play")
            };

            _buttons.Add(buttonFactory.GenerateButton("Play", 100, 100, playArgs));

            // Get real values
            var seperation = 100;
            var totalWidth = (_buttons[0].Width * 2) + seperation;
            var leftSpace = bounds.Width - totalWidth;
            var margin = leftSpace / 2;

            var yValue = 300;

            // Set real values
            _buttons[0].Position = new Vector2(margin, yValue);
            _buttons[1].Position = new Vector2(margin + seperation + _buttons[0].Width, yValue);

            #endregion

            // Initialise more fields
            _nameHelper = new NameInputHelper(_maxLen);
            _textFactory = textFactory;
            _postOfficeService = postOfficeService;
            _name = _textFactory.GenerateText(_nameHelper.GetName(), margin, 100);
        }

        public bool Cleanup()
        {
            return true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var button in _buttons)
            {
                button.Draw(spriteBatch);
            }

            _name.Draw(spriteBatch);
        }

        public bool Initialise()
        {
            // Cleanslate the namehelper
            _nameHelper.Reset();

            return true;
        }

        public bool Start()
        {
            return true;
        }

        public bool Stop()
        {
            // Send finished name off to the player meta data
            var args = new PostOfficeEventArgs()
            {
                SendAddress = "PlayerMetaData",
                MessageName = "ChangeName",
                Data = Encoding.ASCII.GetBytes(_nameHelper.GetName())
            };

            // Start process to reset score to 0
            var scoreargs = new PostOfficeEventArgs()
            {
                SendAddress = "PlayerMetaData",
                MessageName = "RequestScore",
                Data = new byte[10]
            };

            _postOfficeService.SendMail(this.StateRegisterName, args);
            _postOfficeService.SendMail(this.StateRegisterName, scoreargs);

            return true;
        }

        public void Update(MouseState mouseState, KeyboardState keyboardState)
        {
            foreach (var button in _buttons)
            {
                button.Update(mouseState, keyboardState);
            }

            _nameHelper.Update(mouseState, keyboardState);

            // Get latest text
            _name.DisplayText = FormatName(_nameHelper.GetName());
        }

        private string FormatName(string name)
        {

            var baseText = "My name is: ";
            var replacement = "_";

            // Actual length of inputted name
            var lengthOfName = name.Length;

            var stringBuilder = new StringBuilder();

            stringBuilder.Append(baseText);

            for (int i = 0; i < lengthOfName; i++)
            {
                if (i == 0) // First character only
                {
                    stringBuilder.Append($"{char.ToUpper(name[i])} ");
                }
                else
                {
                    stringBuilder.Append($"{char.ToLower(name[i])} ");
                }
                
            }

            // If there are spaces free
            for (int i = 0; i < (_maxLen - lengthOfName); i++)
            {
                stringBuilder.Append($"{replacement} ");
            }

            return stringBuilder.ToString();
        }

        public void LetterBox(string returnAddress, PostOfficeEventArgs args)
        {
            switch (args.MessageName)
            {
                case "GetPlayerScore":

                    // Get current score
                    var currentScore = BitConverter.ToInt64(args.Data, 0);

                    // Flip score
                    currentScore *= -1;

                    //Args to set score to 0
                    var scoreArgs = new PostOfficeEventArgs()
                    {
                        SendAddress = "PlayerMetaData",
                        MessageName = "AddScore",
                        Data = BitConverter.GetBytes(currentScore)
                    };

                    _postOfficeService.SendMail(returnAddress, scoreArgs);

                    break;
            }
        }
    }
}
