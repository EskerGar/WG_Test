using UnityEngine;

namespace Plane
{
    public static class ExtendPlaneMethods
    {
        public static bool CheckDistance(this PlaneBehaviour plane,Vector3 pos, float minDistPos)
        {
            var dist = (pos - plane.transform.position).magnitude;
            return dist <= minDistPos;
        }

        public static void ChangeTarget(this PlaneBehaviour plane, Vector3 newTargetPos, Vector2 newTargetSpeed)
        {
            plane.TargetPos = newTargetPos;
            plane.TargetSpeed = newTargetSpeed;
        }

        public static void SaveTarget(this PlaneBehaviour plane)
        {           
            plane.PrevTargetPos = plane.TargetPos;
            plane.PrevTargetSpeed = plane.TargetSpeed; 
            
        }
    }
}