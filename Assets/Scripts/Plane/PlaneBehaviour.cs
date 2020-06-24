using System;
using System.Collections;
using Plane.PlaneStates;
using Ship;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Plane
{
    [RequireComponent(typeof(PlaneMoveController))]
    public class PlaneBehaviour : MonoBehaviour
    {
        public Vector2 TargetPos { get; set; }
        public Vector2 PrevTargetPos { get; set; }
        public Vector2 TargetSpeed { get; set; }
        public Vector2 PrevTargetSpeed { get; set; }
        public ShipBehaviour Ship { get; private set; }

        public StateMachine StateMachine { get; } = new StateMachine();
        private PlaneFly _planeFly;
        private PlaneMoveController _move;
        private PlaneBehaviour _prevPlane;

        public float Radius { get; private set; }

        public float MaxDistancePos { get; private set; }

        private Vector2 GetSpeed => _move.GetSpeed();
        private void Awake()
        {
            _move = GetComponent<PlaneMoveController>();
            Radius = GetComponent<CircleCollider2D>().radius;
        }

        public void Initialize(ShipBehaviour ship)
        {
            Ship = ship;
            if(_planeFly == null)
                _planeFly = new PlaneFly(this);
            _prevPlane = Ship.GetPrevPlane();
            MaxDistancePos = Ship.GetMaxPlaneDistance;
            _planeFly.StartFly();
        }

        public void SubscribeToFly(Action<PlaneBehaviour> method) => _planeFly.OnReadyToFly += method;

        public void Move() => 
            _move.MoveToTarget(TargetPos);

        public void Evade() =>
            _move.EvadeTarget(TargetPos, TargetSpeed);
        
        public void Pursuit() =>
            _move.PursuitTarget(TargetPos, TargetSpeed);
        

        private void FixedUpdate()
        {
            StateMachine.Update();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (StateMachine.StateIgnoring())
            {
                StateMachine.ChangeState(new BackState(this));
                return;
            }
            if(_prevPlane != null && other.gameObject.Equals(_prevPlane.gameObject))
                this.ChangeTarget(_prevPlane.transform.position, _prevPlane.GetSpeed);
            if(other.gameObject.Equals(Ship.gameObject))
                this.ChangeTarget(Ship.transform.position, Ship.GetSpeed);
            else return;
            StateMachine.ChangeState(new EvadeState(this));
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if(_prevPlane != null && other.gameObject.Equals(_prevPlane.gameObject))
                this.ChangeTarget(_prevPlane.transform.position, _prevPlane.GetSpeed);
            if(other.gameObject.Equals(Ship.gameObject))
                this.ChangeTarget(Ship.transform.position, Ship.GetSpeed);
            else return;
            StateMachine.ChangeState(new IdleState(this));
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(TargetPos, .1f);
        }
    }
}
