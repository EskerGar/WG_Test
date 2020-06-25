using Ship;
using UnityEngine;

namespace Plane.PlaneStates
{
    public class RefuelState: IState
    {
        public bool IsIgnore { get; set; } = false;
        public bool IsCanBePrev { get; set; } = false;
        private Rigidbody2D _ownerRb;
        private ShipBehaviour _ship;

        public RefuelState(PlaneBehaviour owner)
        {
            _ownerRb = owner.GetRb;
            _ship = owner.Ship;
        }
        public void StateLogicFixedUpdate()
        {
            
            
        }

        public void StateLogicUpdate()
        {
            _ownerRb.transform.position = _ship.transform.position;
        }

        public void StartState()
        {
            _ownerRb.velocity = Vector2.zero;
        }

        public void ExitState()
        {
            
        }

        public string GetStateName() => "Refuel";
    }
}