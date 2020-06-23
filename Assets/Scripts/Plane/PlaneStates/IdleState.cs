using Ship;
using UnityEngine;

namespace Plane.PlaneStates
{
    public class IdleState: IState
    {
        private PlaneBehaviour _owner;
        private float _maxDist;

        public IdleState(PlaneBehaviour owner)
        {
            _owner = owner;
            _maxDist = owner.MaxDistancePos;
            IsIgnore = false;
        }

        public bool IsIgnore { get; set; }
        public void StateLogic()
        {
            _owner.Move();
            if (!_owner.CheckDistance(_owner.TargetPos, .5f)) return;
            _owner.ChangeTarget(RandomizePos(), Vector2.zero);
            _owner.SaveTarget();
        }

        public void StartState()
        {
            _owner.ChangeTarget(RandomizePos(), Vector2.zero);
            _owner.SaveTarget();
        }

        public void ExitState()
        {
        }

        private Vector2 RandomizePos()
        {
            var shipPos = _owner.Ship.transform.position;
            return new Vector3(Random.Range(shipPos.x - _maxDist, shipPos.x + _maxDist), 
                Random.Range(shipPos.y - _maxDist, shipPos.y + _maxDist));
        }
    }
}