using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] private KeyCode speedUp = KeyCode.UpArrow;
    [SerializeField] private KeyCode speedDown = KeyCode.DownArrow;
    private MovementController _move;

    public event Action<float> OnSpeedChange;
    private float Rotation => Input.GetAxis("Horizontal");


    private void Start()
    {
        _move = GetComponent<MovementController>();
    }

    private void FixedUpdate()
    {
        if(Input.GetKeyDown(speedUp))
            OnSpeedChange?.Invoke(_move.GetSpeedStep);
        else if (Input.GetKeyDown(speedDown))
            OnSpeedChange?.Invoke(-_move.GetSpeedStep);
        _move.Move();
        _move.Rotate(Rotation);
    }
    
}
