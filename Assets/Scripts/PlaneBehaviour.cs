using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneBehaviour : MonoBehaviour
{
    private ShipBehaviour _ship;

    public void Initialize(ShipBehaviour ship)
    {
        _ship = ship;
    }
}
