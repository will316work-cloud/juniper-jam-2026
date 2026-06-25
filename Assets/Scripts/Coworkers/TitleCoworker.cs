using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TitleCoworker : MonoBehaviour 
{
    public float TimeTresholdForBeingStuck = 2;
    public bool IsMoving; 
    public float _minSpinSpeed;
    public float _maxSpinSpeed;
    private float _spinSpeed;

    Vector3 _originalPosition; public Vector3 OriginalPosition => _originalPosition;
    private NavMeshAgent _agent;
    TitleCoworkerManager _manager;
    Coroutine _movementRoutine;
    List<TitlePathPoint> _currentPathPoints = new();
    private float _stuckTimerTime;
    ParticleSystem _walkParticle;
    Animator _animator;

    public void Initialize(TitleCoworkerManager manager, float movementSpeed)
    {
        _manager = manager;
        _walkParticle = gameObject.GetComponentInChildren<ParticleSystem>();
        _agent = gameObject.GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _originalPosition = transform.position;
        _stuckTimerTime = 0;
        SetMovementSpeed(movementSpeed);
        _spinSpeed = Random.Range(_minSpinSpeed, _maxSpinSpeed);
        if (Random.value < 0.5f) _spinSpeed = -_spinSpeed;
    }

    public void StartMoving(List<TitlePathPoint> pathPoints)
    {
        _walkParticle.Play();
        IsMoving = true;
        _currentPathPoints = pathPoints;
        _stuckTimerTime = 0;
        _manager.AddCoworkerToMovingCoworkers(this);
        _movementRoutine = StartCoroutine(PathPointHandler());
    }

    //the state code is never used. spinning logic is now based on IsMoving
    public void SetSpinningState(bool state)
    {
        if(state) _animator.SetTrigger("Spin");
        else _animator.SetTrigger("Idle");
    }

    IEnumerator PathPointHandler()
    {
        int pathPointIndex = 0;
        
        while(pathPointIndex < _currentPathPoints.Count)
        {
            TitlePathPoint currentPathPoint = _currentPathPoints[pathPointIndex];

            _agent.SetDestination(currentPathPoint.Position);
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
        foreach(TitlePathPoint point in _currentPathPoints) point.IsAvailable = true;

        _agent.SetDestination(_originalPosition);
        while(_agent.pathPending)
            yield return null;
        while (_agent.remainingDistance > _agent.stoppingDistance)
            yield return null;

        IsMoving = false;

        yield return new WaitForSeconds(1f);

        _manager.AddCoworkerToAvailableCoworkers(this);
        _movementRoutine = null;
        _walkParticle.Stop();
        _walkParticle.Clear();
        //SetSpinningState(false);
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
            _walkParticle.Stop();
            _walkParticle.Clear();
            IsMoving = false;
        }
    }    

    void StuckHandler()
    {
        if(_movementRoutine != null && _agent.velocity.magnitude <= 0.1f && _agent.remainingDistance > _agent.stoppingDistance && IsMoving)
        {
            _stuckTimerTime += Time.deltaTime;
            if(_stuckTimerTime >= 4f)
            {
                StopCoroutine(_movementRoutine);
                _movementRoutine = StartCoroutine(GoBackToOriginalPosition());
                _stuckTimerTime = 0;
            }
        }
        else if(_stuckTimerTime > 0f)
            _stuckTimerTime = 0f;
    }

    private void Spin() 
    {
        float _tempSpeed;
        if (IsMoving)
        {
            _tempSpeed = _spinSpeed * 2;
        }
        else
        {
            _tempSpeed = _spinSpeed;
        }
        transform.Rotate(0f, _tempSpeed * Time.deltaTime, 0f, Space.World);
    }

    void Update()
    {
        StuckHandler();
        Spin();
    }

    
    public void SetAvoidancePriority(int priority) => _agent.avoidancePriority = priority;
    public void SetMovementSpeed(float speed) => _agent.speed = speed;
    public void SetVelocityToZero() => _agent.velocity = Vector3.zero;
}
