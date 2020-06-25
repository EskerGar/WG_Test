using System;
using UnityEngine;

namespace Plane.PlaneStates
{
        public class StateMachine
        {
                private IState _currentState;
                private IState _prevState;

                public event Action<IState> OnStateChange; 

                public bool ChangeState(IState newState)
                {
                        if (_currentState != null && _currentState.GetType() == newState.GetType()) return false;
                        _currentState?.ExitState();
                        if(_currentState != null && _currentState.IsCanBePrev )
                                _prevState = _currentState;
                        _currentState = newState;
                        _currentState.StartState();
                        OnStateChange?.Invoke(_currentState);
                        return true;
                }

                public bool PrevState()
                {
                        return _prevState != null && ChangeState(_prevState);
                } 

                public void Update() => _currentState?.StateLogicUpdate();

                public void FixedUpdate() => _currentState?.StateLogicFixedUpdate();

                public bool StateIgnoring() => _currentState.IsIgnore;
        }
}