using UnityEngine;

namespace Plane.PlaneStates
{
        public class StateMachine
        {
                private IState _currentState;
                private IState _prevState;

                public IState ChangeState(IState newState)
                {
                        //if (_currentState != null && _currentState.GetType() == newState.GetType()) return;
                        _currentState?.ExitState();
                        _prevState = _currentState;
                        _currentState = newState;
                        _currentState.StartState();
                        return _currentState;
                }

                public void PrevState() => _currentState = _prevState;

                public void Update() => _currentState?.StateLogic();

                public bool IsPrevStateExist() => _prevState != null;

                public bool StateIgnoring() => _currentState.IsIgnore;
        }
}