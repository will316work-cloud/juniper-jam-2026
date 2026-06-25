using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TitleCoworkerManager : MonoBehaviour
{
    private List<TitlePathPoint> _pathPoints;
    // public float CoworkerBaseMovementSpeed;
    // public int MaximumBaseConcurrentMovingCoworkers;
    // public float MinimumBaseTimeGapeBetweenMovingCoworkers;
    public int InitialChanceOfMovingWorkerInPercentage;
    public int ChanceIncreaseOnFailedCoworkerMovingInPercentage;
    public bool IsDebugOn;

    private List<TitleCoworker> _coworkers;
    private Transform _player;
    private float _coworkerMovementSpeed;
    private int _maximumConcurrentMovingCoworkers;
    private float _minimumTimeGapeBetweenMovingCoworkers;
    private float _coworkerMoverTimer;

    private int _chanceOfMovingWorkerInPercentage;
    private int _currentChanceOfMovingWorkerInPercentage;

    [SerializeField] List<TitleCoworker> _availableCoworkers = new();
    [SerializeField] List<TitleCoworker> _movingCoworkers = new();
    private bool _isCoworkerMoverActive; public bool IsCoworkerMoverActive => _isCoworkerMoverActive;

    public void Initialize(GameContext ctx)
    {
        _coworkers = FindObjectsByType<TitleCoworker>().ToList();
        _pathPoints = FindObjectsByType<TitlePathPoint>().ToList();

        foreach (TitlePathPoint point in _pathPoints) point.Initialize();

        _availableCoworkers.AddRange(_coworkers);
        
        _player = ctx.PlayerControl.transform;
        // _coworkerMovementSpeed = CoworkerBaseMovementSpeed;
        // _maximumConcurrentMovingCoworkers = MaximumBaseConcurrentMovingCoworkers;
        // _minimumTimeGapeBetweenMovingCoworkers = MinimumBaseTimeGapeBetweenMovingCoworkers;
        _chanceOfMovingWorkerInPercentage = InitialChanceOfMovingWorkerInPercentage;
        _currentChanceOfMovingWorkerInPercentage = _chanceOfMovingWorkerInPercentage;

        foreach (TitleCoworker coworker in _coworkers)
            coworker.Initialize(this, _coworkerMovementSpeed);

        if(IsDebugOn) Debug.Log("Coworker count: " + _coworkers.Count);
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

            if(IsDebugOn) Debug.Log("Coworker moving failed.");
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
        if(_availableCoworkers.Count == 0)
        {
            if(IsDebugOn) Debug.Log("No available coworkers to move.");
            return;
        }
        if(_movingCoworkers.Count >= _maximumConcurrentMovingCoworkers)
        {
            if(IsDebugOn) Debug.Log("Maximum number of moving coworkers reached.");
            return;
        }

        _availableCoworkers[Random.Range(0, _availableCoworkers.Count)].StartMoving(GetRandomPathPoints(3));
        if(IsDebugOn) Debug.Log("Moving coworker.");
    }

    List<TitlePathPoint> GetRandomPathPoints(int numberOfPointsRequested)
    {
        List<TitlePathPoint> paths = new();
        List<TitlePathPoint> availabelPathPoints = new();
        foreach(TitlePathPoint point in _pathPoints)
            if(point.IsAvailable) availabelPathPoints.Add(point);

        for(int i = 0; i < numberOfPointsRequested; i++)
        {
            if(i == 0)
            {
                TitlePathPoint closestPoint = PathPointClosestToPlayer(availabelPathPoints);
                paths.Add(closestPoint);
                availabelPathPoints.Remove(closestPoint);
                closestPoint.IsAvailable = false;
                continue;
            }

            int randomIndex = Random.Range(0, availabelPathPoints.Count);
            TitlePathPoint point = availabelPathPoints[randomIndex];
            paths.Add(point);
            availabelPathPoints.RemoveAt(randomIndex);
            point.IsAvailable = false;
        }
        return paths;
    }

    public TitlePathPoint PathPointClosestToPlayer(List<TitlePathPoint> pathPoints)
    {
        TitlePathPoint closestPath = pathPoints[0];

        for(int i = 0; i < pathPoints.Count; i++)
        {
            if (Vector3.Distance(PlayerLocation(), pathPoints[i].Position) < Vector3.Distance(PlayerLocation(), closestPath.Position))
            {
                closestPath = pathPoints[i];
            }
        }
        return closestPath;
    }

    public void AddCoworkerToMovingCoworkers(TitleCoworker coworker)
    {
        if (_availableCoworkers.Contains(coworker))
            _availableCoworkers.Remove(coworker);

        _movingCoworkers.Add(coworker);
        coworker.SetAvoidancePriority(20 * _movingCoworkers.Count);
    }
    public void AddCoworkerToAvailableCoworkers(TitleCoworker coworker)
    {
        if (_movingCoworkers.Contains(coworker))
            _movingCoworkers.Remove(coworker);

        _availableCoworkers.Add(coworker);
        coworker.SetAvoidancePriority(1);
    }

    public void SetCoworkerMovementSpeed(float speed)
    {
        _coworkerMovementSpeed = speed;

        foreach (TitleCoworker coworker in _movingCoworkers)
            coworker.SetMovementSpeed(_coworkerMovementSpeed);
        foreach (TitleCoworker coworker in _availableCoworkers)
            coworker.SetMovementSpeed(_coworkerMovementSpeed);
    }

    public void SetMaximumConcurrentMovingCoworkers(int count) => _maximumConcurrentMovingCoworkers = count;

    /// <summary>
    /// Resets the movement speed of all coworkers to the current movement speed not to the original base speed.
    /// </summary>
    public void ResetCoworkerMovementSpeed()
    {
        foreach (TitleCoworker coworker in _movingCoworkers)
            coworker.SetMovementSpeed(_coworkerMovementSpeed);
        foreach (TitleCoworker coworker in _availableCoworkers)
            coworker.SetMovementSpeed(_coworkerMovementSpeed);
    }

    Vector3 PlayerLocation() => _player.position;

    /// <summary>
    /// Stops all coworkers from moving.
    /// </summary>
    public void StopCoworkerMovement()
    {
        if(_movingCoworkers.Count == 0) return;
        foreach(TitleCoworker coworker in _movingCoworkers)
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
        if(_movingCoworkers.Count == 0) return;
        foreach (TitleCoworker coworker in _movingCoworkers)
            coworker.SetMovementSpeed(_coworkerMovementSpeed);

        if(IsDebugOn) Debug.Log("Resuming movement of " + _movingCoworkers.Count + " coworkers.");
    }

    /// <summary>
    /// Teleports all coworkers back to their original position and stops their movement. Places them in the available coworker list.
    /// </summary>
    public void TeleportCoworkersToOriginalPlace()
    {
        List<TitleCoworker> tempMovingCoworkers = new(_movingCoworkers);

        foreach (TitleCoworker coworker in tempMovingCoworkers)
        {
            coworker.ResetMovement();
            coworker.transform.position = coworker.OriginalPosition;
            AddCoworkerToAvailableCoworkers(coworker);
            coworker.SetAvoidancePriority(1);
        }

        ResetPathPoints();
    }

    /// <summary>
    /// Teleports all coworkers back to their original position and stops their movement. 
    /// Places them in the available coworker list. Sets their movement speed to the base speed.
    /// Resets the coworker mover timegap and concurrent coworker count.
    /// </summary>
    public void ResetSystem()
    {
        TeleportCoworkersToOriginalPlace();

        // _coworkerMovementSpeed = CoworkerBaseMovementSpeed;
        // _maximumConcurrentMovingCoworkers = MaximumBaseConcurrentMovingCoworkers;
        // _minimumTimeGapeBetweenMovingCoworkers = MinimumBaseTimeGapeBetweenMovingCoworkers;
        _chanceOfMovingWorkerInPercentage = InitialChanceOfMovingWorkerInPercentage;
        _currentChanceOfMovingWorkerInPercentage = _chanceOfMovingWorkerInPercentage;

        foreach (TitleCoworker coworker in _availableCoworkers)
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
    void ResetPathPoints()
    {
        foreach (TitlePathPoint point in _pathPoints) point.IsAvailable = true;
    }
    public void SetMoverGapTime(float gapTime) => _minimumTimeGapeBetweenMovingCoworkers = gapTime;
}