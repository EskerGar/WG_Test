using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject ship;

    private Vector3 _offset;

    private void Start()
    {
        _offset = transform.position - ship.transform.position;
    }

    private void Update()
    {
        transform.position = ship.transform.position + _offset;
    }
}
