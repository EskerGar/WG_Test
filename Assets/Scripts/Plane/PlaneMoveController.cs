using UnityEngine;

namespace Plane
{
    public class PlaneMoveController : MonoBehaviour
    {
        private Rigidbody2D _rb;
        private float _minMoveSpeed;
        private float _maxMoveSpeed;
        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            _rb = GetComponent<Rigidbody2D>();
            _maxMoveSpeed = 10;
        }

        public Vector2 GetSpeed() => _rb.velocity;

        public void FollowTo(Vector3 targetPos)
        {
            var dir = (targetPos - transform.position).normalized;
            transform.up = dir;
        }

        public void MoveToTarget(Vector3 target, Vector3 targetSpeed, bool evade)
        {
            Vector2 steering;
            if(evade)
                steering = EvadeTarget(target, targetSpeed);
            else
                steering =  SlowMoving(target, 5f);
            steering = Truncate(steering, 10f);
            steering /= _rb.mass;
            _rb.velocity = Truncate(_rb.velocity + steering, _maxMoveSpeed);
        }
    
        private Vector2 Truncate(Vector2 vec, float max)
        {
            var i = max / vec.magnitude;
            i = i < 1f ? i : 1f;
            return vec * i;
        }

        private Vector3 AvoidTarget(Vector3 target)
        {
            Vector2 desiredVelocity = (transform.position - target).normalized * _maxMoveSpeed;
            var steering = (desiredVelocity - _rb.velocity) * Time.deltaTime;
            return steering;
        }

        private Vector3 EvadeTarget(Vector3 target, Vector3 targetSpeed)
        {
            var distance  = target - transform.position;
            var updatesAhead  = distance.magnitude / _maxMoveSpeed;
            var futurePosition  = target + targetSpeed * updatesAhead;
            return AvoidTarget(futurePosition);
        }

        private Vector2 SlowMoving(Vector3 target, float slowingRadius)
        {
            Vector2 desiredVelocity = target - transform.position;
            var distance = desiredVelocity.magnitude;

            desiredVelocity = distance < slowingRadius
                ? desiredVelocity.normalized * (_maxMoveSpeed * (distance / slowingRadius))
                : desiredVelocity.normalized * _maxMoveSpeed;

            return (desiredVelocity - _rb.velocity) * Time.deltaTime;
        }
    }
}
