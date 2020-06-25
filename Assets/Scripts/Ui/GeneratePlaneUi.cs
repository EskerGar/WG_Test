using System.Collections.Generic;
using System.Linq;
using Plane;
using Ship;
using UnityEngine;

namespace Ui
{
    public class GeneratePlaneUi : MonoBehaviour
    {
        [SerializeField] private Vector3 offset;
        [SerializeField] private GameObject prefab;
        [SerializeField] private ShipBehaviour ship;
        private List<GameObject> _listPlaneUi = new List<GameObject>();
        private bool _isInfoTrue = true;
        private void Start()
        {
            ship.GetComponent<CreatePlane>().OnNewPlane += CreatePlaneUi;
            ship.GetComponent<InputController>().OnShowInfo += ShowInfo;
        }
        

        private void CreatePlaneUi(PlaneBehaviour plane)
        {
            var ui = Instantiate(prefab, transform);
            if(_listPlaneUi.Count > 0)
               ui.transform.position = _listPlaneUi.Last().transform.position + offset;
            _listPlaneUi.Add(ui);
           ui.GetComponent<PlaneUi>().Initialize(plane, _listPlaneUi.Count);
           if (_listPlaneUi.Count == ship.GetPlaneCount)
               ship.GetComponent<CreatePlane>().OnNewPlane -= CreatePlaneUi;
        }

        private void ShowInfo()
        {
            gameObject.SetActive(!_isInfoTrue);
            _isInfoTrue = !_isInfoTrue;
        }
    }
}
