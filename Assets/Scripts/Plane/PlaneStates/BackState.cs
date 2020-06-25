using Ship;
using UnityEngine;

namespace Plane.PlaneStates
{
    public class BackState: IState
    {
        private PlaneBehaviour _owner;

        public BackState(PlaneBehaviour owner)
        {
            _owner = owner;
            IsIgnore = true;
            IsCanBePrev = false;
        }

        public bool IsIgnore { get; set; }
        public bool IsCanBePrev { get; set; }

        public void StateLogicFixedUpdate()
        {
            _owner.Move();
            
        }

        public void StateLogicUpdate()
        {
            _owner.ChangeTarget(_owner.Ship.transform.position, _owner.Ship.GetSpeed);
        }

        public void StartState()
        {
            _owner.ChangeTarget(_owner.Ship.transform.position, _owner.Ship.GetSpeed);
        }

        public void ExitState()
        {
            _owner.ChangeTarget(_owner.PrevTargetPos, _owner.PrevTargetSpeed);
        }

        public string GetStateName() => "Back";
    }
}