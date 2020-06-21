using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] private KeyCode speedUp = KeyCode.UpArrow;
    [SerializeField] private KeyCode speedDown = KeyCode.DownArrow;
    private MovementController _move;

    public event Action<float> OnSpeedUp;
    public event Action<float> OnSpeedDown; 
    private float Rotation => Input.GetAxis("Horizontal");


    private void Start()
    {
        _move = GetComponent<MovementController>();
    }

    private void FixedUpdate()
    {
        if(Input.GetKeyDown(speedUp))
            OnSpeedUp?.Invoke(_move.GetSpeedStep);
        else if (Input.GetKeyDown(speedDown))
            OnSpeedDown?.Invoke(_move.GetSpeedStep);
        _move.Move();
        _move.Rotate(Rotation);
    }
    
}
