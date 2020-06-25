using UnityEngine;

namespace Plane.PlaneStates
{
    public interface IState
    {
        bool IsIgnore { get; set; }
        bool IsCanBePrev { get; set; }
        void StateLogic();
        void StartState();
        void ExitState();
        string GetStateName();

    }
}