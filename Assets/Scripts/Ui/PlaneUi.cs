using System;
using Plane;
using Plane.PlaneStates;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class PlaneUi : MonoBehaviour
    {
        [SerializeField] private Text nameText;
        [SerializeField] private Text speedText;
        [SerializeField] private Text stateText;
        private PlaneBehaviour _plane;
        
        public void Initialize(PlaneBehaviour plane, int planeCount)
        {
            _plane = plane;
            NewPlane(planeCount.ToString());
            _plane.GetComponent<PlaneMoveController>().OnSpeedChange += ChangeSpeed;
            _plane.SubscribeToState(ChangeState);
        }

        private void NewPlane(string planeName)
        {
            nameText.text = "Plane " + planeName;
        }

        private void ChangeSpeed(float speed)
        {
            var roundSpeed = (float) Math.Round(speed, 2);
            speedText.text = "Speed: " + roundSpeed.ToString();
        }

        private void ChangeState(IState state)
        {
            stateText.text = "State: " + state.GetStateName();
        }
    }
}
