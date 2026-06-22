using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Coworker : MonoBehaviour 
{
    public float TimeTresholdForBeingStuck = 2;
    Vector3 _originalPosition; public Vector3 OriginalPosition => _originalPosition;
    private NavMeshAgent _agent;
    CoworkerManager _manager;
    Coroutine _movementRoutine;


    private float _stuckTimerTime;
    public void Initialize(CoworkerManager manager, float movementSpeed)
    {
        _manager = manager;
        _agent = gameObject.GetComponent<NavMeshAgent>();
        _originalPosition = transform.position;
        _stuckTimerTime = 0;
        SetMovementSpeed(movementSpeed);
    }

    public void StartMoving(List<Transform> pathPoints)
    {
        _stuckTimerTime = 0;
        _manager.AddCoworkerToMovingCoworkers(this);
        _movementRoutine = StartCoroutine(PathPointHandler(pathPoints));
    }

    IEnumerator PathPointHandler(List<Transform> pathPoints)
    {
        int pathPointIndex = 0;
        
        while(pathPointIndex < pathPoints.Count)
        {
            Transform currentPathPoint = pathPoints[pathPointIndex];

            _agent.SetDestination(currentPathPoint.position);
            while(_agent.pathPending)
                yield return null;

            while (_agent.remainingDistance > _agent.stoppingDistance)
                yield return null;

            pathPointIndex++;
        }

        _movementRoutine = StartCoroutine(GoBackToOriginalPosition());
    }

    IEnumerator GoBackToOriginalPosition()
    {
        _agent.SetDestination(_originalPosition);
        while(_agent.pathPending)
            yield return null;
        while (_agent.remainingDistance > _agent.stoppingDistance)
            yield return null;

        yield return new WaitForSeconds(1f);

        _movementRoutine = null;
        _manager.AddCoworkerToAvailableCoworkers(this);
    }

    /// <summary>
    /// Stops the movement coroutine.
    /// </summary>
    public void ResetMovement()
    {
        if (_movementRoutine != null)
        {
            StopCoroutine(_movementRoutine);
            _movementRoutine = null;
            _agent.ResetPath();
            _agent.velocity = Vector3.zero;
            _stuckTimerTime = 0f;
        }
    }    

    void StuckHandler()
    {
        if(_movementRoutine != null && _agent.velocity.magnitude <= 0.1f && _agent.remainingDistance > _agent.stoppingDistance)
        {
            _stuckTimerTime += Time.deltaTime;
            if(_stuckTimerTime >= 2f)
            {
                StopCoroutine(_movementRoutine);
                _movementRoutine = StartCoroutine(GoBackToOriginalPosition());
                _stuckTimerTime = 0;
            }
        }
        else if(_stuckTimerTime > 0f)
            _stuckTimerTime = 0f;
    }

    void Update()
    {
        StuckHandler();
    }

    public void SetAvoidancePriority(int priority) => _agent.avoidancePriority = priority;
    public void SetMovementSpeed(float speed) => _agent.speed = speed;
    public void SetVelocityToZero() => _agent.velocity = Vector3.zero;
}
