using UnityEngine;

namespace Plane.PlaneStates
{
    public class PursuitState: IState
    {
        public bool IsIgnore { get; set; } = false;
        public bool IsCanBePrev { get; set; }
        private PlaneBehaviour _owner;

        public PursuitState(PlaneBehaviour owner)
        {
            _owner = owner;
            IsCanBePrev = true;
        }

        public void StateLogic()
        {
            var prevPos = Vector3.one;
           _owner.Pursuit();
           var mousePos = Input.mousePosition;
           mousePos.z = 10;
           mousePos = Camera.main.ScreenToWorldPoint (mousePos);
           var speed = ((Vector2)mousePos - (Vector2)prevPos);
           prevPos = mousePos;
           _owner.ChangeTarget(mousePos, speed);
        }

        public void StartState()
        {
           
        }

        public void ExitState()
        {
        }

        public string GetStateName() => "Pursuit";
    }
}