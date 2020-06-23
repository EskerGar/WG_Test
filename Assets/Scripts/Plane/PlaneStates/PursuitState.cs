namespace Plane.PlaneStates
{
    public class PursuitState: IState
    {
        public bool IsIgnore { get; set; } = false;
        private PlaneBehaviour _owner;

        public PursuitState(PlaneBehaviour owner)
        {
            _owner = owner;
        }

        public void StateLogic()
        {
           _owner.Pursuit();
        }

        public void StartState()
        {
           
        }

        public void ExitState()
        {
            _owner.ChangeTarget(_owner.PrevTargetPos, _owner.PrevTargetSpeed);
        }
        
    }
}