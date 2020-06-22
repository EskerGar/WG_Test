using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePlane : MonoBehaviour
{
    [SerializeField] private GameObject prefabPlane;
    private InputController _input;

    private void Start()
    {
        _input = GetComponent<InputController>();
        _input.OnPlaneStart += Create;
    }

    private void Create(ShipBehaviour ship)
    {
        if(!ship.OverFlowCheck()) return;
        var plane = Instantiate(prefabPlane, transform.position, Quaternion.identity);
        var planeBehaviour = plane.GetComponent<PlaneBehaviour>();
        ship.AddPlane(planeBehaviour);
        planeBehaviour.Initialize(ship);
    }
}
