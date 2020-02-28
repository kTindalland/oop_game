using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Brightforest.Controls;
using Brightforest.Factories;
using Interfaces.EventArguments;
using Interfaces.StateHandling;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Brightforest.States
{
    public class FlavourTextState : IState
    {
        public string StateRegisterName => "FlavourText";

        private Button _continueButton;
        private readonly TextFactory _textFactory;

        private List<Text> _flavourText;

        public FlavourTextState(ButtonFactory buttonFactory, TextFactory textFactory)
        {
            // Fields
            _textFactory = textFactory;

            #region ContinueButton

            var continueArgs = new PostOfficeEventArgs()
            {
                SendAddress = "StateManager",
                MessageName = "ChangeState",
                Data = Encoding.ASCII.GetBytes("Play")
            };
            _continueButton = buttonFactory.GenerateButton("Continue", 20, 400, continueArgs);



            #endregion

            #region Text Stuff

            // Lines of flavour text
            var lines = new List<string>()
            {
                "The castle is under attack!",
                "The evil fire squirrels have beseiged Brightforest.",
                "Direct orders from the crown state you are in charge",
                "of defending as long as you can.",
                "Upgrades are M30 each, a new archer is M20.",
                "Good luck."
            };

            // Get the flavour text as Text objects
            _flavourText = GenerateTexts(lines);

            #endregion
        }

        private List<Text> GenerateTexts(List<string> lines)
        {
            // Create the array
            var textArray = lines.ToArray();

            // Get the starting coords
            var startY = 20;
            var x = 20;

            // The results list
            var result = new List<Text>();
            for (int i = 0; i < textArray.Length; i++)
            {
                // Fill it in
                result.Add(_textFactory.GenerateText(textArray[i], x, startY + (i * 50)));
            }

            return result;
        }

        public bool Cleanup()
        {
            return true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw all the text
            foreach (var text in _flavourText)
            {
                text.Draw(spriteBatch);
            }

            // Draw the continue button
            _continueButton.Draw(spriteBatch);
        }

        public bool Initialise()
        {
            return true;
        }

        public bool Start()
        {
            return true;
        }

        public bool Stop()
        {
            return true;
        }

        public void Update(MouseState mouseState, KeyboardState keyboardState)
        {
            // Update the continue button
            _continueButton.Update(mouseState, keyboardState);
        }
    }
}
