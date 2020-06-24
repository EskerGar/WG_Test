using Ship;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Ui
{
    public class ShipUi : MonoBehaviour
    {
        [SerializeField] private Text speedText;
        [SerializeField] private Text freePlaneText;
        [SerializeField] private Text createPlaneText;
        [SerializeField] private ShipBehaviour ship;
        private void Start()
        {
            ship.GetComponent<MovementController>().OnSpeedChange += ChangeSpeed;
            ship.OnCountFreePlaneChanged += ChangePlaneCount;
            ship.OnCreatePlane += ChangeCreatePlaneCount;
        }

        private void ChangeSpeed(float speed)
        {
            speedText.text = "MoveSpeed: " + speed.ToString();
        }

        private void ChangePlaneCount(int count)
        {
            freePlaneText.text = "FreePlane: " + count.ToString();
        }

        private void ChangeCreatePlaneCount(int count)
        {
            createPlaneText.text = "CreatePlane: " + count.ToString();
        }
    }
}
