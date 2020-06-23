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

        private void Start()
        {
            _move = GetComponent<MovementController>();
        }

        public Vector2 GetSpeed => _move.GetSpeed;

        public float GetMaxPlaneDistance => maxPlaneDistance;

        public void AddPlane(PlaneBehaviour plane)
        {
            plane.OnReadyToFly += AddFreePlane;
            _stackPlane.Push(plane);
            _prevPlane = plane;
        }

        private void AddFreePlane(PlaneBehaviour plane) => _freeStackPlane.Push(plane);

        private PlaneBehaviour GetFreePlane() => _freeStackPlane.Count > 0 ? _freeStackPlane.Pop() : null;

        public PlaneBehaviour GetPrevPlane()
        {
            return _prevPlane;
        }


        public void StartFreePlane(ShipBehaviour ship)
        {
            var plane = GetFreePlane();
            if (plane == null) return; 
            plane.transform.position += new Vector3(.5f, .5f, 0);
            plane.gameObject.SetActive(true);
            plane.Initialize(ship);
            _prevPlane = plane;
        }

        public bool OverFlowCheck() => _stackPlane.Count >= planeCount;
    }
}
