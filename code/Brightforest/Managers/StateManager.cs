using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces.EventArguments;
using Interfaces.Services;
using Interfaces.StateHandling;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Brightforest.Managers
{
    public class StateManager : IStateManager, ILetterbox
    {
        private IState _currentActiveState;
        private Dictionary<string, IState> _allStates;
        private IPostOfficeService _postOffice;
        private readonly string _postAddress;


        public StateManager(IPostOfficeService postOffice)
        {
            _postOffice = postOffice;
            _postAddress = "StateManager";

            _allStates = new Dictionary<string, IState>();
        }

        public IState GetActiveState()
        {
            throw new NotImplementedException();
        }

        public void LetterBox(string returnAddress, PostOfficeEventArgs args)
        {
            string newState;

            switch (args.MessageName)
            {
                case "ChangeState":

                    newState = Encoding.ASCII.GetString(args.Data);

                    ChangeState(newState, returnAddress);

                    break;

                case "SetInitialState":

                    newState = Encoding.ASCII.GetString(args.Data);

                    _currentActiveState = _allStates[newState];

                    break;
            }
        }

        private void ChangeState(string newState, string returnAddress)
        {
            var containsKey = _allStates.Keys.Contains(newState);

            if (!containsKey)
            {
                var postOfficeArgs = new PostOfficeEventArgs()
                {
                    SendAddress = returnAddress,
                    MessageName = "Error",
                    Data = Encoding.ASCII.GetBytes("State is not registered within the state manager.")
                };

                _postOffice.SendMail(_postAddress, postOfficeArgs);
                return;
            }

            StopCurrentState();
            CleanupCurrentState();

            _currentActiveState = _allStates[newState];
            
            InitialiseCurrentState();
            StartCurrentState();
        }

        private void StopCurrentState()
        {
            _currentActiveState.Stop();
        }

        private void InitialiseCurrentState()
        {
            _currentActiveState.Initialise();
        }

        private void StartCurrentState()
        {
            _currentActiveState.Start();
        }

        private void CleanupCurrentState()
        {
            _currentActiveState.Cleanup();
        }

        public void RegisterState(IState state)
        {
            // Check if key already exists
            var keyThereAlready = _allStates.Keys.Contains(state.StateRegisterName);

            if (!keyThereAlready) _allStates[state.StateRegisterName] = state;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_currentActiveState == null) return;
            _currentActiveState.Draw(spriteBatch);
        }

        public void Update(MouseState mouseState, KeyboardState keyboardState)
        {
            if (_currentActiveState == null) return;
            _currentActiveState.Update(mouseState, keyboardState);
        }
    }
}
