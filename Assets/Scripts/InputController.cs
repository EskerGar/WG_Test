﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] private KeyCode speedUp = KeyCode.UpArrow;
    [SerializeField] private KeyCode speedDown = KeyCode.DownArrow;
    [SerializeField] private KeyCode startPlane = KeyCode.H;
    private MovementController _move;
    private ShipBehaviour _ship;
    public event Action<float> OnSpeedChange;
    public event Action<ShipBehaviour> OnPlaneStart;
    private float Rotation => Input.GetAxis("Horizontal");


    private void Start()
    {
        _move = GetComponent<MovementController>();
        _ship = GetComponent<ShipBehaviour>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(speedUp))
            OnSpeedChange?.Invoke(_move.GetSpeedStep);
        else if (Input.GetKeyDown(speedDown))
            OnSpeedChange?.Invoke(-_move.GetSpeedStep);
        
        if(Input.GetKeyDown(startPlane))
            OnPlaneStart?.Invoke(_ship);
    }

    private void FixedUpdate()
    {
        _move.Move();
        _move.Rotate(Rotation);
    }
    
}
