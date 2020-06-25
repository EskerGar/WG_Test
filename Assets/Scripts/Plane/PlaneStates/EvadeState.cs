using Ship;
using UnityEngine;

namespace Plane.PlaneStates
{
    public class EvadeState: IState
    {
        private PlaneBehaviour _owner;
        private GameObject _target;

        public EvadeState(PlaneBehaviour owner)
        {
            _owner = owner;
            IsIgnore = false;
            IsCanBePrev = false;
        }


        public bool IsIgnore { get; set; }
        public bool IsCanBePrev { get; set; }

        public void StateLogicFixedUpdate()
        {
            _owner.Evade();
        }

        public void StateLogicUpdate()
        {
        }

        public void StartState()
        {
        }

        public void ExitState()
        {
            _owner.ChangeTarget(_owner.PrevTargetPos, _owner.PrevTargetSpeed);
        }

        public string GetStateName() => "Evade";
    }
}