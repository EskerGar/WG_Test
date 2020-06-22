using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class MovementController : MonoBehaviour
{
    [SerializeField] [Min(0)] private float moveSpeed;
    [SerializeField] [Min(0)] private float rotateSpeed;
    [SerializeField] private float speedStep;
    [SerializeField] private float minMoveSpeed;
    [SerializeField] private float maxMoveSpeed;

    public float GetSpeedStep => speedStep;

    private Rigidbody2D _rb;
    private InputController _input;
    private void Start()
    {
        Initialize();
    }

    protected  virtual void Initialize()
    {
        _rb = GetComponent<Rigidbody2D>();
        _input = GetComponent<InputController>();
        _input.OnSpeedChange += ChangeSpeed;
    }


    private void ChangeSpeed(float value)
    {
        moveSpeed += value;
            if (moveSpeed > maxMoveSpeed)
                moveSpeed = maxMoveSpeed;
            else if (moveSpeed < minMoveSpeed)
                moveSpeed = minMoveSpeed;
    }
    
    public void Move() => _rb.velocity = transform.up * (moveSpeed * Time.deltaTime);

    public void Rotate(float moveVector) => _rb.rotation += ( rotateSpeed * moveVector * Time.deltaTime);
}
