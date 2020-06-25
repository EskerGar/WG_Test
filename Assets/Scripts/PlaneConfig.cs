using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "PlaneConfig")]
public class PlaneConfig : ScriptableObject
{
    [SerializeField] private float minMoveSpeed;
    [SerializeField] private float maxMoveSpeed;
    [SerializeField] private float angularSpeed;

    public float GetMinMoveSpeed => minMoveSpeed;
    public float GetMaxMoveSpeed => maxMoveSpeed;
    public float GetAngularSpeed => angularSpeed;

}
