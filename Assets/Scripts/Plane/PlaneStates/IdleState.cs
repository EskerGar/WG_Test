using Ship;
using UnityEngine;
using UnityEngine.Assertions;

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
            IsCanBePrev = true;
        }

        public bool IsIgnore { get; set; }
        public bool IsCanBePrev { get; set; }

        public void StateLogic()
        {
            _owner.Move();
            if (!_owner.CheckDistance(_owner.TargetPos, .7f)) return;
            _owner.ChangeTarget(RandomizePos(), Vector2.zero);
            _owner.SaveTarget();
        }

        public void StartState()
        {
            if (!_owner.TargetPos.Equals(_owner.Ship.transform.position)) return;
            _owner.ChangeTarget(RandomizePos(), Vector2.zero);
            _owner.SaveTarget();
        }

        public void ExitState()
        {
        }

        public string GetStateName() => "Idle";

        private Vector2 RandomizePos()
        {
            Assert.IsTrue((_owner.Radius + _owner.Ship.GetRadius) < _maxDist, "MaxDistance should be > plane radius + ship radius");
            float x, y;
            while (true)
            {
                var shipPos = _owner.Ship.transform.position;
                x = Random.Range(shipPos.x - _maxDist / 2, shipPos.x + _maxDist / 2);
                y = Random.Range(shipPos.y - _maxDist / 2, shipPos.y + _maxDist / 2);
                var dist = new Vector2(x - shipPos.x, y - shipPos.y).magnitude;
                if (!(dist < _owner.Radius + _owner.Ship.GetRadius)) break;
            }

            return new Vector3(x , y);
        }
    }
}