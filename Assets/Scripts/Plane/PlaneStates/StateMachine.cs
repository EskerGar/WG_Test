namespace Plane.PlaneStates
{
        public class StateMachine
        {
                private IState _currentState;

                public void ChangeState(IState newState)
                {
                        _currentState?.StateExit();
                        _currentState = newState;
                        _currentState.StateEnter();
                }

                public void Update() => _currentState?.StateLogic();
        }
}