using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneMoveController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private float _minMoveSpeed;
    private float _maxMoveSpeed;
    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        _rb = GetComponent<Rigidbody2D>();
        _maxMoveSpeed = 10;
    }

    public void FollowTo(Vector3 targetPos)
    {
        var dir = (targetPos - transform.position).normalized;
        transform.up = dir;
    }

    public void Move(Vector3 target)
    {
        Vector2 desiredVelocity = (target - transform.position).normalized * _maxMoveSpeed;
        var steering =  (desiredVelocity - _rb.velocity) * Time.deltaTime;
        steering = Truncate(steering, 10f);
        steering /= _rb.mass;
        _rb.velocity = Truncate(_rb.velocity + steering, _maxMoveSpeed);
    }

    private Vector2 Truncate(Vector2 vec, float max)
    {
        var i = max / vec.magnitude;
        i = i < 1f ? i : 1f;
        return vec * i;
    }
    
    
}
