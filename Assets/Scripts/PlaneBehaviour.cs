using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlaneBehaviour : MonoBehaviour
{
    private ShipBehaviour _ship;
    private Vector2 _targetPos;
    private PlaneMoveController _move;
    private float _maxDistancePos;

    public void Initialize(ShipBehaviour ship)
    {
        _ship = ship;
        _targetPos = transform.position;
        _maxDistancePos = _ship.GetMaxPlaneDistance;
        _move = GetComponent<PlaneMoveController>();
    }

    private void FixedUpdate()
    {
        _move.Move(_targetPos);
        IdleState(_targetPos);

    }

    private void IdleState(Vector3 pos)
    {
        if (!CheckPos(pos)) return;
        _targetPos = RandomizePos();
        _move.FollowTo(_targetPos);
    }

    private bool CheckPos(Vector3 pos)
    {
        var plane_pos = transform.position;
        return Mathf.Abs(pos.x) - Mathf.Abs(plane_pos.x) <= .1f && Mathf.Abs(pos.y) - Mathf.Abs(plane_pos.y) <= .1f;
    }

    private Vector2 RandomizePos()
    {
        var shipPos = _ship.transform.position;
        return new Vector3(Random.Range(shipPos.x - _maxDistancePos, shipPos.x + _maxDistancePos), 
                           Random.Range(shipPos.y - _maxDistancePos, shipPos.y + _maxDistancePos));
    }
}
