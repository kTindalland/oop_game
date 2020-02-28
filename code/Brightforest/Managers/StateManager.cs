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

            // A dictionary of all states
            _allStates = new Dictionary<string, IState>();
        }

        public IState GetActiveState()
        {
            // Returns the currently active state
            return _currentActiveState;
        }

        public void LetterBox(string returnAddress, PostOfficeEventArgs args)
        {
            string newState;

            switch (args.MessageName)
            {
                // Change the currently active state
                case "ChangeState":

                    newState = Encoding.ASCII.GetString(args.Data);

                    ChangeState(newState, returnAddress);

                    break;

                // Set the initial state - change state without calling any stops and cleanups
                case "SetInitialState":

                    newState = Encoding.ASCII.GetString(args.Data);

                    _currentActiveState = _allStates[newState];

                    break;
            }
        }

        // Change the state
        private void ChangeState(string newState, string returnAddress)
        {
            var containsKey = _allStates.Keys.Contains(newState);

            if (!containsKey)
            {
                // If the state isn't registered, then kick out the error to the error letterbox
                var postOfficeArgs = new PostOfficeEventArgs()
                {
                    SendAddress = returnAddress,
                    MessageName = "Error",
                    Data = Encoding.ASCII.GetBytes("State is not registered within the state manager.")
                };

                _postOffice.SendMail(_postAddress, postOfficeArgs);
                return;
            }

            // Stop the state
            StopCurrentState();

            // Cleanup the state
            CleanupCurrentState();

            // Change the state
            _currentActiveState = _allStates[newState];
            
            // Initialise the state
            InitialiseCurrentState();

            // Start the state
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
            // Call draw method of current state
            if (_currentActiveState == null) return;
            _currentActiveState.Draw(spriteBatch);
        }

        public void Update(MouseState mouseState, KeyboardState keyboardState)
        {
            // Call update method of current state
            if (_currentActiveState == null) return;
            _currentActiveState.Update(mouseState, keyboardState);
        }
    }
}
