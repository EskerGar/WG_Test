using System;
using System.Collections;
using Plane.PlaneStates;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Plane
{
    public class PlaneFly
    {
        private const float MaxFlyTime = 200f;
        private const float MaxExistTime = 300f;
        
        private readonly PlaneBehaviour _owner;
        private Coroutine _flyCoroutine;
        private Coroutine _evadeCoroutine;
        private readonly StateMachine _stateMachine;
        public event Action<PlaneBehaviour> OnReadyToFly;
        private PlaneBehaviour _prevPlane;
        private bool _isPrevPlaneExist;
        private float _minDistancePos = 1;

        public PlaneFly(PlaneBehaviour owner)
        {
            _owner = owner;
            _stateMachine = owner.StateMachine;
            _prevPlane = owner.Ship.GetPrevPlane();
            if (_prevPlane != null)
                _isPrevPlaneExist = true;
        }

        public void StartFly()
        {
            _owner.StartCoroutine(StartFlyingCoroutine());
        }
        
        private IEnumerator StartFlyingCoroutine()
        {
            _owner.StartCoroutine(StopFly());
            while (true)
            {
                _flyCoroutine = _owner.StartCoroutine(FlyAgainCoroutine());
                yield return _flyCoroutine;
            }
        }

        private IEnumerator StopFly()
        {
            yield return  new WaitForSeconds(MaxExistTime);
            _owner.StopCoroutine(_flyCoroutine);
            _owner.StopCoroutine(_evadeCoroutine);
            yield return BackToTheShipCoroutine();
            OnReadyToFly?.Invoke(_owner);
            _owner.gameObject.SetActive(false);
        }

        private IEnumerator FlyAgainCoroutine()
        {
            var prev = _stateMachine.ChangeState(new IdleState(_owner));
            _evadeCoroutine = _owner.StartCoroutine(CheckEvadeCoroutine(prev));
            var randFlyTime = Random.Range(10, MaxFlyTime);
            yield return new WaitForSeconds(randFlyTime);
            _owner.StopCoroutine(_evadeCoroutine);
            yield return BackToTheShipCoroutine();
            yield return new WaitForSeconds(2f); // Refueling
        }

        private IEnumerator BackToTheShipCoroutine()
        {
            _stateMachine.ChangeState(new BackState(_owner));
            yield return new WaitUntil(() => (_owner.Ship.transform.position - _owner.transform.position).magnitude <= .3f);
        }
        
        private void CheckOnEvade(IState prevState)
        {
            if (_isPrevPlaneExist && _prevPlane.gameObject.active)
                CheckTarget(_prevPlane.transform.position, _prevPlane.GetSpeed, prevState);
            //CheckTarget(_owner.Ship.transform.position, _owner.Ship.GetSpeed, prevState);
        }

        private void CheckTarget(Vector2 pos, Vector2 speed, IState prevState)
        {
            if(_owner.CheckDistance(pos, _minDistancePos))
            {
                _owner.ChangeTarget(pos, speed);
                _stateMachine.ChangeState(new EvadeState(_owner));
            }
            else if(_stateMachine.IsPrevStateExist())
                _stateMachine.ChangeState(prevState);
        }

        private IEnumerator CheckEvadeCoroutine(IState prevState)
        {
            yield return new WaitForSeconds(1f);
            while (true)
            {
                CheckOnEvade(prevState);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}