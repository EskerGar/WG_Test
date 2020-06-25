using System;
using UnityEngine;

namespace Plane
{
    public class PlaneMoveController : MonoBehaviour
    {
        private Rigidbody2D _rb;
        private float _minMoveSpeed;
        private float _maxMoveSpeed;
        private float _angularSpeed;

        public event Action<float> OnSpeedChange;

        public void Initialize(float maxMoveSpeed, float minMoveSpeed, float angularSpeed)
        {
            _rb = GetComponent<Rigidbody2D>();
            _maxMoveSpeed = maxMoveSpeed;
            _minMoveSpeed = minMoveSpeed;
            _angularSpeed = angularSpeed;
        }

        public Vector2 GetSpeed() => _rb.velocity;

        public void FollowTo(Vector3 targetPos)
        {
            var dir = (targetPos - transform.position).normalized;
            transform.up = dir;
        }

        public void MoveToTarget(Vector3 target)
        {
            var steering =  SlowMoving(target, 3f);
            Move(steering, .1f);
        }

        public void EvadeTarget(Vector3 target, Vector3 targetSpeed)
        {
           var steering = EvadeTarget(FuturePosition(target, targetSpeed));
            Move(steering, 1f);
        }

        public void PursuitTarget(Vector3 target, Vector3 targetSpeed)
        {
            var steering =  SlowMoving(FuturePosition(target, targetSpeed), 3f);
            Move(steering, 1f); 
        }

        private void Move(Vector2 steering, float maxForce)
        {
            steering = Truncate(steering, maxForce);
            steering /= _rb.mass;
            _rb.velocity = Truncate(_rb.velocity + steering, _maxMoveSpeed); 
            OnSpeedChange?.Invoke(_rb.velocity.magnitude);
        }

        private Vector3 Kasatelnaya(Vector3 target)
        {
            var desiredVelocity = (transform.position - target).normalized * _maxMoveSpeed;
            var kas = new Vector2(desiredVelocity.y, desiredVelocity.x);
            if (Vector2.Dot(kas, _rb.velocity) < 0)
                kas = -kas;
            var steering = (kas - _rb.velocity) ;
            return steering;
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
            var steering = (desiredVelocity - _rb.velocity) ;
            return steering;
        }

        private Vector3 EvadeTarget(Vector3 futurePos) => Kasatelnaya(futurePos);

        private Vector3 FuturePosition(Vector3 target, Vector3 targetSpeed)
        {
            var distance  = target - transform.position;
            var updatesAhead  = distance.magnitude / _maxMoveSpeed;
            return  target + targetSpeed * updatesAhead;
        }

        private Vector2 SlowMoving(Vector3 target, float slowingRadius)
        {
            Vector2 desiredVelocity = target - transform.position;
            var distance = desiredVelocity.magnitude;

            desiredVelocity = distance < slowingRadius
                ? desiredVelocity.normalized * (_maxMoveSpeed * (distance / slowingRadius))
                : desiredVelocity.normalized * _maxMoveSpeed;

            if (desiredVelocity.magnitude < _minMoveSpeed)
                desiredVelocity = desiredVelocity.normalized * _minMoveSpeed;
            return (desiredVelocity - _rb.velocity) ;
        }
    }
}
