using UnityEngine;

namespace Plane.PlaneStates
{
    public class PursuitState: IState
    {
        public bool IsIgnore { get; set; } = false;
        public bool IsCanBePrev { get; set; }
        private PlaneBehaviour _owner;
        private Vector2 _prevPos = Vector2.zero;
        private Camera _cam;

        public PursuitState(PlaneBehaviour owner, Camera cam)
        {
            _owner = owner;
            IsCanBePrev = true;
            _cam = cam;
        }

        public void StateLogicFixedUpdate()
        {
            _owner.Pursuit();
           
        }

        public void StateLogicUpdate()
        {
            var mousePos = Input.mousePosition;
            mousePos.z = 10;
            mousePos = _cam.ScreenToWorldPoint (mousePos);
            var speed = ((Vector2)mousePos - _prevPos);
            _prevPos = mousePos;
            _owner.ChangeTarget(mousePos, speed); 
        }

        public void StartState()
        {
            _owner.IsHuntActive = true;
        }

        public void ExitState()
        {
            _owner.IsHuntActive = false;
        }

        public string GetStateName() => "Pursuit";
    }
}