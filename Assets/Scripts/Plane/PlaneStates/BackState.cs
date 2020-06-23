using Ship;

namespace Plane.PlaneStates
{
    public class BackState: IState
    {
        private PlaneBehaviour _owner;
        private ShipBehaviour _ship;
    
        public BackState(PlaneBehaviour owner)
        {
            _owner = owner;
            _ship = owner.Ship;
        }
    
        public void StateLogic()
        {
            _owner.TargetPos = _ship.transform.position;
            _owner.Move(_owner.TargetPos, _ship.GetSpeed, false);
            PlaneOnTheShip();
        }

        private bool CheckPos() => (_ship.transform.position - _owner.transform.position).magnitude <= .3f;

        private void PlaneOnTheShip()
        {
            if (CheckPos())
                _owner.IsReadyToFly = true;
        }

        public void StateExit()
        {
        
        }

        public void StateEnter()
        {
        
        
        }
    }
}