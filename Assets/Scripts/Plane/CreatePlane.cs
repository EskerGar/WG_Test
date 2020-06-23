using Ship;
using UnityEngine;

namespace Plane
{
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
            if (ship.OverFlowCheck())
            {
                ship.StartFreePlane(ship);
                return;
            }
            var plane = Instantiate(prefabPlane, transform.position + new Vector3(.5f, .5f, 0), Quaternion.identity);
            var planeBehaviour = plane.GetComponent<PlaneBehaviour>();
            planeBehaviour.Initialize(ship);
            ship.AddPlane(planeBehaviour);
        }
    }
}
