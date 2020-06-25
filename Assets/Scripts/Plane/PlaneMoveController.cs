using System;
using UnityEngine;

namespace Plane
{
    public class PlaneMoveController : MonoBehaviour
    {
        private Rigidbody2D _rb;
        private float _minMoveSpeed;
        private float _maxMoveSpeed;
        private float _maxAngularSpeed;

        public event Action<float> OnSpeedChange;

        public void Initialize(float maxMoveSpeed, float minMoveSpeed, float angularSpeed)
        {
            _rb = GetComponent<Rigidbody2D>();
            _maxMoveSpeed = maxMoveSpeed;
            _minMoveSpeed = minMoveSpeed;
            _maxAngularSpeed = angularSpeed;
        }

        public Vector2 GetSpeed() => _rb.velocity;

        private void FollowTo()
        {
            _rb.AddTorque(Vector2.SignedAngle(transform.up, _rb.velocity));
            if(((Vector2)transform.up - _rb.velocity).sqrMagnitude < 0.2 * 0.2)
            {
                _rb.angularVelocity = 0;
            }
            if (_rb.angularVelocity > _maxAngularSpeed)
                _rb.angularVelocity = _maxAngularSpeed;
            if (_rb.angularVelocity < -_maxAngularSpeed)
                _rb.angularDrag = -_maxAngularSpeed;
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
            FollowTo();
            OnSpeedChange?.Invoke(_rb.velocity.magnitude);
        }

        private Vector3 AvoidTarget(Vector3 target)
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

        private Vector3 EvadeTarget(Vector3 futurePos) => AvoidTarget(futurePos);

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

            var velocity = distance < slowingRadius
                ? desiredVelocity.normalized * (_maxMoveSpeed * (distance / slowingRadius))
                : desiredVelocity.normalized * _maxMoveSpeed;

            if (velocity.magnitude < _minMoveSpeed)
                velocity = desiredVelocity.normalized * _minMoveSpeed;
            return (velocity - _rb.velocity) ;
        }
    }
}
