using UnityEngine;

public class IdleState: IState
{
    private PlaneBehaviour _owner;
    private ShipBehaviour _ship;
    private float _maxDistancePos;
    private Vector2 _targetSpeed = Vector2.zero;
    private PlaneBehaviour _lastPlane;
    private float _minDistancePos;
    private Vector2 _prevTargetPos;
    private bool _isEvading;
    private bool _isLastPlaneExist = false;

    public IdleState(PlaneBehaviour owner)
    {
        _owner = owner;
        _ship = owner.Ship;
        _maxDistancePos = owner.MaxDistancePos;
        _lastPlane = owner.LastPlane;
        if (_lastPlane != null)
            _isLastPlaneExist = true;
        _minDistancePos = owner.MinDistancePos;
        _prevTargetPos = owner.TargetPos;
    } 

    public void StateLogic()
    {
        if(_isLastPlaneExist)
            CheckTarget(_lastPlane.transform.position, _lastPlane.GetSpeed);
        
        CheckTarget(_ship.transform.position, _ship.GetSpeed);
        
        _owner.Move(_owner.TargetPos, _targetSpeed, _isEvading);
        if (!CheckPos(_prevTargetPos)) return;
        _prevTargetPos = RandomizePos();
    }

    public void StateExit()
    {
    }

    public void StateEnter()
    {
        _owner.TargetPos = RandomizePos();
    }

    private void CheckTarget(Vector3 targetPos, Vector2 targetSpeed)
    {
        if(CheckDistance(targetPos))
        {
            _targetSpeed = targetSpeed;
            _owner.TargetPos = targetPos;
            _isEvading = true;
        }
        else
        {
            _targetSpeed = Vector2.zero;
            _owner.TargetPos = _prevTargetPos;
            _isEvading = false;
        }

    }
    
    private bool CheckDistance(Vector3 pos)
    {
        var dist = (pos - _owner.transform.position).magnitude;
        return dist <= _minDistancePos;
    }
    
    private Vector2 RandomizePos()
    {
        var shipPos = _ship.transform.position;
        return new Vector3(Random.Range(shipPos.x - _maxDistancePos, shipPos.x + _maxDistancePos), 
            Random.Range(shipPos.y - _maxDistancePos, shipPos.y + _maxDistancePos));
    }
    
    private bool CheckPos(Vector3 pos)
    {
        var planePos = _owner.transform.position;
        return Mathf.Abs(pos.x) - Mathf.Abs(planePos.x) <= .1f && Mathf.Abs(pos.y) - Mathf.Abs(planePos.y) <= .1f;
    }
    
}