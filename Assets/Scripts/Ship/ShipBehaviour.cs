using System;
using System.Collections.Generic;
using Plane;
using UnityEngine;

namespace Ship
{
    public class ShipBehaviour : MonoBehaviour
    {
        [SerializeField] private int planeCount;

        [SerializeField] private float maxPlaneDistance;
        private readonly Stack<PlaneBehaviour> _stackPlane = new Stack<PlaneBehaviour>();
        private readonly Stack<PlaneBehaviour> _freeStackPlane = new Stack<PlaneBehaviour>();
        private MovementController _move;
        private PlaneBehaviour _prevPlane;
        private float _radius;

        public float GetRadius => _radius;
        public Vector2 GetSpeed => _move.GetSpeed;
        public float GetMaxPlaneDistance => maxPlaneDistance;
        public int GetPlaneCount => planeCount;
        public event Action<int> OnCountFreePlaneChanged;
        public event Action<int> OnCreatePlane; 

        private void Start()
        {
            _move = GetComponent<MovementController>();
            _radius = GetComponent<CircleCollider2D>().radius;
        }

        public void AddPlane(PlaneBehaviour plane)
        {
            plane.SubscribeToFly(AddFreePlane);
            _stackPlane.Push(plane);
            _prevPlane = plane;
            OnCreatePlane?.Invoke(_stackPlane.Count);
        }

        private void AddFreePlane(PlaneBehaviour plane) 
        {
            _freeStackPlane.Push(plane);
            OnCountFreePlaneChanged?.Invoke(_freeStackPlane.Count);
        }

        private PlaneBehaviour GetFreePlane()
        {
            if (_freeStackPlane.Count > 0)
            {
                OnCountFreePlaneChanged?.Invoke(_freeStackPlane.Count - 1);
                return _freeStackPlane.Pop();
            }
            OnCountFreePlaneChanged?.Invoke(0);
            return null;
        }

        public PlaneBehaviour GetPrevPlane()
        {
            return _prevPlane;
        }


        public void StartFreePlane(ShipBehaviour ship)
        {
            var plane = GetFreePlane();
            if (plane == null) return; 
            plane.transform.position = transform.position + new Vector3(.5f, .5f, 0);
            plane.gameObject.SetActive(true);
            plane.RestartPlane();
            _prevPlane = plane;
        }

        public bool OverFlowCheck() => _stackPlane.Count >= planeCount;
    }
}
