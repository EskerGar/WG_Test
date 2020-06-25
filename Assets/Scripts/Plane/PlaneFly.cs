using System;
using System.Collections;
using Plane.PlaneStates;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Plane
{
    public class PlaneFly
    {
        private const float MaxFlyTime = 20f;
        private const float MaxExistTime = 30f;
        
        private readonly PlaneBehaviour _owner;
        private Coroutine _flyCoroutine;
        private readonly StateMachine _stateMachine;
        public event Action<PlaneBehaviour> OnReadyToFly;

        public PlaneFly(PlaneBehaviour owner)
        {
            _owner = owner;
            _stateMachine = owner.StateMachine;
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
            yield return BackToTheShipCoroutine();
            OnReadyToFly?.Invoke(_owner);
            _owner.gameObject.SetActive(false);
        }

        private IEnumerator FlyAgainCoroutine()
        {
            if(!_stateMachine.PrevState())
                _stateMachine.ChangeState(new IdleState(_owner));
            var randFlyTime = Random.Range(5, MaxFlyTime);
            yield return new WaitForSeconds(randFlyTime);
            yield return BackToTheShipCoroutine();
            _stateMachine.ChangeState(new RefuelState(_owner));
            yield return new WaitForSeconds(2f); // Refueling
        }

        private IEnumerator BackToTheShipCoroutine()
        {
            _stateMachine.ChangeState(new BackState(_owner));
            yield return new WaitUntil(() => (_owner.CheckDistance(_owner.Ship.transform.position, .5f)));
        }
    }
}