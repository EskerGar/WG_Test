using System;
using UnityEngine;

namespace Plane.PlaneStates
{
        public class StateMachine
        {
                private IState _currentState;
                private IState _prevState;

                public event Action<IState> OnStateChange; 

                public void ChangeState(IState newState)
                {
                        if (_currentState != null && _currentState.GetType() == newState.GetType()) return;
                        _currentState?.ExitState();
                        if(_currentState != null && _currentState.IsCanBePrev)
                                _prevState = _currentState;
                        _currentState = newState;
                        _currentState.StartState();
                        OnStateChange?.Invoke(_currentState);
                }

                public bool PrevState()
                {
                        if (_prevState == null) return false;
                        ChangeState(_prevState);
                        return true;
                } 

                public void Update() => _currentState?.StateLogic();

                public bool StateIgnoring() => _currentState.IsIgnore;
        }
}