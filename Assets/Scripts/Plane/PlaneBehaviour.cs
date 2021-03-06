﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Plane.PlaneStates;
using Ship;
using UnityEngine;

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
        private Rigidbody2D _rb;
        private Dictionary<GameObject, Vector2> evadeList = new Dictionary<GameObject, Vector2>();
        private Camera _cam;
        public bool IsHuntActive { get; set; }

        public float Radius { get; private set; }

        public float MaxDistancePos { get; private set; }

        private Vector2 GetSpeed => _move.GetSpeed();
        public Rigidbody2D GetRb => _rb;
        private void Awake()
        {
            _move = GetComponent<PlaneMoveController>();
            Radius = GetComponent<CircleCollider2D>().radius;
            _rb = GetComponent<Rigidbody2D>();
            _cam = Camera.main;
        }

        public void Initialize(ShipBehaviour ship, PlaneConfig configs)
        {
            Ship = ship;
            _move.Initialize(configs.GetMaxMoveSpeed, configs.GetMinMoveSpeed, configs.GetAngularSpeed);
            if(_planeFly == null)
                _planeFly = new PlaneFly(this);
            MaxDistancePos = Ship.GetMaxPlaneDistance;
            RestartPlane();
        }

        public void RestartPlane()
        {
            _prevPlane = Ship.GetPrevPlane();
            IsHuntActive = false;
            _planeFly.StartFly();
        }
        public void SubscribeToState(Action<IState> method) => StateMachine.OnStateChange += method;
        public void SubscribeToFly(Action<PlaneBehaviour> method) => _planeFly.OnReadyToFly += method;

        public void Move() => 
            _move.MoveToTarget(TargetPos);

        public void Evade() 
        {
            if(evadeList.Count > 0)
            {
                var go = evadeList.First();
                this.ChangeTarget(go.Key.transform.position, go.Value);
                _move.EvadeTarget(TargetPos, TargetSpeed);
            }else StateMachine.PrevState();
            
        }
        
        public void Pursuit() =>
            _move.PursuitTarget(TargetPos, TargetSpeed);
        

        private void FixedUpdate()
        {
            
            StateMachine.FixedUpdate();
        }

        private void Update()
        { 
            StateMachine.Update();
            CheckMaxDist();
        }

        private void CheckMaxDist()
        {
            if (StateMachine.StateIgnoring()) return;
            if(!this.CheckDistance(Ship.transform.position, MaxDistancePos))
                StateMachine.ChangeState(new IdleState(this));
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (StateMachine.StateIgnoring()) return;
            if(_prevPlane != null && other.gameObject.Equals(_prevPlane.gameObject))
                evadeList.Add(_prevPlane.gameObject, _prevPlane.GetSpeed);
            if(other.gameObject.Equals(Ship.gameObject))
                evadeList.Add(Ship.gameObject, Ship.GetSpeed);
            StateMachine.ChangeState(new EvadeState(this));
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if((_prevPlane != null && other.gameObject.Equals(_prevPlane.gameObject)) || other.gameObject.Equals(Ship.gameObject))
                evadeList.Remove(other.gameObject);
        }

        public void StartHunt()
        {
            if (StateMachine.StateIgnoring()) return;
            if (!IsHuntActive)
            {
                StateMachine.ChangeState(new PursuitState(this, _cam));
            }
            else
            {
                if (!StateMachine.PrevState())
                    StateMachine.ChangeState(new IdleState(this));
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(TargetPos, 1f);
            
        }
    }
}
