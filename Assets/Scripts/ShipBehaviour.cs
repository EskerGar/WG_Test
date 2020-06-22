using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBehaviour : MonoBehaviour
{
    [SerializeField] private int planeCount;

    [SerializeField] private float maxPlaneDistance;
    private readonly List<PlaneBehaviour> _planeList = new List<PlaneBehaviour>();

    public float GetMaxPlaneDistance => maxPlaneDistance;
    public event Action OnOverFlowPlanes;

    public void AddPlane(PlaneBehaviour plane) => _planeList.Add(plane);

    public bool OverFlowCheck()
    {
        if (_planeList.Count < planeCount) return true;
        OnOverFlowPlanes?.Invoke();
        return false;

    }
}
