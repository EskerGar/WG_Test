using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(PlaneMoveController))]
public class PlaneBehaviour : MonoBehaviour
{
    private const float MaxFlyTime = 20f;
    private const float MaxExistTime = 30f;
    
    public Vector2 TargetPos { get; set; }
    private PlaneMoveController _move;
    public float MaxDistancePos { get; private set; }
    public PlaneBehaviour LastPlane { get; private set; }
    public float MinDistancePos { get; private set; } = 3;
    public bool IsReadyToFly { get; set; }
    public ShipBehaviour Ship { get; private set; }

    private readonly StateMachine _stateMachine = new StateMachine();
    private Coroutine _flyCoroutine;
    public event Action<PlaneBehaviour> OnReadyToFly;

    public Vector2 GetSpeed => _move.GetSpeed();

    private void Start()
    {
        _move = GetComponent<PlaneMoveController>();
    }

    public void Initialize(ShipBehaviour ship)
    {
        Ship = ship;
        TargetPos = transform.position;
        MaxDistancePos = Ship.GetMaxPlaneDistance;
        LastPlane = ship.GetPrevPlane();
        StartCoroutine(StartFlyingCoroutine());
    }

    public void Move(Vector3 target, Vector3 moveSpeed, bool isEvade) => _move.MoveToTarget(target, moveSpeed, isEvade);


    private void FixedUpdate()
    {
        _stateMachine.Update();
    }

    private IEnumerator StartFlyingCoroutine()
    {
        StartCoroutine(StopFly());
        while (true)
        {
            _flyCoroutine = StartCoroutine(FlyAgainCoroutine());
            yield return _flyCoroutine;
        }
    }

    private IEnumerator StopFly()
    {
        yield return  new WaitForSeconds(MaxExistTime);
        StopCoroutine(_flyCoroutine);
        _stateMachine.ChangeState(new BackState(this));
        yield return new WaitUntil(() => IsReadyToFly);
        OnReadyToFly?.Invoke(this);
        gameObject.SetActive(false);
    }

    private IEnumerator FlyAgainCoroutine()
    {
        IsReadyToFly = false;
        _stateMachine.ChangeState(new IdleState(this));
        var randFlyTime = Random.Range(2, MaxFlyTime);
        yield return new WaitForSeconds(randFlyTime);
        _stateMachine.ChangeState(new BackState(this));
        yield return new WaitUntil(() => IsReadyToFly);
        yield return new WaitForSeconds(2f); // Refueling
    }
    
}
