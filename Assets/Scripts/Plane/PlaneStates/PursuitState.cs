using UnityEngine;

namespace Plane.PlaneStates
{
    public class PursuitState: IState
    {
        public bool IsIgnore { get; set; } = false;
        private PlaneBehaviour _owner;
        private Vector3 prevPos = Vector3.one;

        public PursuitState(PlaneBehaviour owner)
        {
            _owner = owner;
        }

        public void StateLogic()
        {
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
        
    }
}