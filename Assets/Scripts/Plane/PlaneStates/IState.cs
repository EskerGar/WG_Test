namespace Plane.PlaneStates
{
    public interface IState
    {
        void StateLogic();
        void StateExit();
        void StateEnter();
    
    }
}