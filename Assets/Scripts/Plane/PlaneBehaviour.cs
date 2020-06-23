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
        public float MaxDistancePos { get; private set; }

        public Vector2 GetSpeed => _move.GetSpeed();
        private void Awake()
        {
            _move = GetComponent<PlaneMoveController>();
        }

        public void Initialize(ShipBehaviour ship)
        {
            Ship = ship;
            if(_planeFly == null)
                _planeFly = new PlaneFly(this);
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
    }
}
