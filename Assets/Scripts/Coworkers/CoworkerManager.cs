using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CoworkerManager : MonoBehaviour
{
    public List<Transform> PathPoints;
    public float CoworkerBaseMovementSpeed;
    public int MaximumBaseConcurrentMovingCoworkers;
    public float MinimumBaseTimeGapeBetweenMovingCoworkers;
    public int InitialChanceOfMovingWorkerInPercentage;
    public int ChanceIncreaseOnFailedCoworkerMovingInPercentage;

    private List<Coworker> _coworkers;
    private Transform _player;
    private float _coworkerMovementSpeed;
    private int _maximumConcurrentMovingCoworkers;
    private float _minimumTimeGapeBetweenMovingCoworkers;
    private float _coworkerMoverTimer;

    private int _chanceOfMovingWorkerInPercentage;
    private int _currentChanceOfMovingWorkerInPercentage;

    List<Coworker> _availableCoworkers = new();
    List<Coworker> _movingCoworkers = new();
    private bool _isCoworkerMoverActive; public bool IsCoworkerMoverActive => _isCoworkerMoverActive;

    public void Initialize(GameContext ctx)
    {
        _coworkers = FindObjectsByType<Coworker>().ToList();
        _availableCoworkers.AddRange(_coworkers);
        
        _player = ctx.PlayerControl.transform;
        _coworkerMovementSpeed = CoworkerBaseMovementSpeed;
        _maximumConcurrentMovingCoworkers = MaximumBaseConcurrentMovingCoworkers;
        _minimumTimeGapeBetweenMovingCoworkers = MinimumBaseTimeGapeBetweenMovingCoworkers;
        _chanceOfMovingWorkerInPercentage = InitialChanceOfMovingWorkerInPercentage;
        _currentChanceOfMovingWorkerInPercentage = _chanceOfMovingWorkerInPercentage;

        foreach (Coworker coworker in _coworkers)
            coworker.Initialize(this, _coworkerMovementSpeed);

        Debug.Log("Coworker count: " + _coworkers.Count);
    }

    void CoworkerMover()
    {
        if (!_isCoworkerMoverActive) return;

        _coworkerMoverTimer += Time.deltaTime;
        if (_coworkerMoverTimer >= _minimumTimeGapeBetweenMovingCoworkers)
        {
            if (Random.Range(0, 100) < _currentChanceOfMovingWorkerInPercentage)
            {
                MoveRandomWorker();
                _coworkerMoverTimer = 0; 
                _currentChanceOfMovingWorkerInPercentage = _chanceOfMovingWorkerInPercentage;
                return;
            }

            _currentChanceOfMovingWorkerInPercentage += ChanceIncreaseOnFailedCoworkerMovingInPercentage;
            _coworkerMoverTimer = 0;
        }
    }

    void Update()
    {
        CoworkerMover();
    }

    public void MoveRandomWorker()
    {
        if(_availableCoworkers.Count == 0 || _movingCoworkers.Count >= _maximumConcurrentMovingCoworkers) return;
        _availableCoworkers[Random.Range(0, _availableCoworkers.Count)].StartMoving(GetRandomPathPoints(3));
    }

    List<Transform> GetRandomPathPoints(int numberOfPointsRequested)
    {
        List<Transform> paths = new();
        List<Transform> allPoints = new(PathPoints);

        for(int i = 0; i < numberOfPointsRequested; i++)
        {
            if(i == 0)
            {
                Transform closestPoint = PathPointClosestToPlayer(allPoints);
                paths.Add(closestPoint);
                allPoints.Remove(closestPoint);
                continue;
            }

            int randomIndex = Random.Range(0, allPoints.Count);
            Transform point = allPoints[randomIndex];
            paths.Add(point);
            allPoints.RemoveAt(randomIndex);
        }
        return paths;
    }

    public Transform PathPointClosestToPlayer(List<Transform> pathPoints)
    {
        Transform closestPath = pathPoints[0];

        for(int i = 0; i < pathPoints.Count; i++)
        {
            if (Vector3.Distance(PlayerLocation(), pathPoints[i].position) < Vector3.Distance(PlayerLocation(), closestPath.position))
            {
                closestPath = pathPoints[i];
            }
        }
        return closestPath;
    }

    public void AddCoworkerToMovingCoworkers(Coworker coworker)
    {
        if (_availableCoworkers.Contains(coworker))
            _availableCoworkers.Remove(coworker);

        _movingCoworkers.Add(coworker);
        coworker.SetAvoidancePriority(20 * _movingCoworkers.Count);
    }
    public void AddCoworkerToAvailableCoworkers(Coworker coworker)
    {
        if (_movingCoworkers.Contains(coworker))
            _movingCoworkers.Remove(coworker);

        _availableCoworkers.Add(coworker);
        coworker.SetAvoidancePriority(1);
    }

    public void IncreaseCoworkerSpeed()
    {
        CoworkerBaseMovementSpeed++;

        foreach (Coworker coworker in _movingCoworkers)
            coworker.SetMovementSpeed(CoworkerBaseMovementSpeed);
        foreach (Coworker coworker in _availableCoworkers)
            coworker.SetMovementSpeed(CoworkerBaseMovementSpeed);
    }

    /// <summary>
    /// Resets the movement speed of all coworkers to the current movement speed not to the original base speed.
    /// </summary>
    public void ResetCoworkerMovementSpeed()
    {
        foreach (Coworker coworker in _movingCoworkers)
            coworker.SetMovementSpeed(_coworkerMovementSpeed);
        foreach (Coworker coworker in _availableCoworkers)
            coworker.SetMovementSpeed(_coworkerMovementSpeed);
    }

    Vector3 PlayerLocation() => _player.position;

    /// <summary>
    /// Stops all coworkers from moving.
    /// </summary>
    public void StopCoworkerMovement()
    {
        foreach(Coworker coworker in _movingCoworkers)
        {
            coworker.SetMovementSpeed(0);
            coworker.SetVelocityToZero();
        }
    }

    /// <summary>
    /// Resumes all coworkers movement.
    /// </summary>
    public void ContinueMovingCoworkersMovement()
    {
        foreach (Coworker coworker in _movingCoworkers)
            coworker.SetMovementSpeed(_coworkerMovementSpeed);
    }

    /// <summary>
    /// Teleports all coworkers back to their original position and stops their movement. Places them in the available coworker list.
    /// </summary>
    public void TeleportCoworkersToOriginalPlace()
    {
        List<Coworker> tempMovingCoworkers = new(_movingCoworkers);

        foreach (Coworker coworker in tempMovingCoworkers)
        {
            coworker.ResetMovement();
            coworker.transform.position = coworker.OriginalPosition;
            AddCoworkerToAvailableCoworkers(coworker);
            coworker.SetAvoidancePriority(1);
        }
    }

    /// <summary>
    /// Teleports all coworkers back to their original position and stops their movement. 
    /// Places them in the available coworker list. Sets their movement speed to the base speed.
    /// Resets the coworker mover timegap and concurrent coworker count.
    /// </summary>
    public void ResetSystem()
    {
        TeleportCoworkersToOriginalPlace();

        _coworkerMovementSpeed = CoworkerBaseMovementSpeed;
        _maximumConcurrentMovingCoworkers = MaximumBaseConcurrentMovingCoworkers;
        _minimumTimeGapeBetweenMovingCoworkers = MinimumBaseTimeGapeBetweenMovingCoworkers;
        _chanceOfMovingWorkerInPercentage = InitialChanceOfMovingWorkerInPercentage;
        _currentChanceOfMovingWorkerInPercentage = _chanceOfMovingWorkerInPercentage;

        foreach (Coworker coworker in _availableCoworkers)
        {
            coworker.SetAvoidancePriority(1);
            coworker.SetMovementSpeed(_coworkerMovementSpeed);
        }   
    }

    /// <summary>
    /// It only sets the automatic coworker mover system state. Does not effect the coworkers movement or position.
    /// </summary>
    /// <param name="state"></param>
    public void SetCoworkerMoverState(bool state)
    {
        _isCoworkerMoverActive = state;
    }
}