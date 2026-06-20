using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    #region Serialized fields
    [SerializeField] private float _minTimeBetweenTasks = 5;
    [SerializeField] private float _taskGettingAttemptFrequency;
    [SerializeField] private float _baseChanceToGetTask = 10;
    [SerializeField] private float _chanceIncreaseAfterFailedAttempt = 10;
    
    #endregion
    #region Private fields
    private float _chanceToGetTask;
    private float _timeSinceLastTask;

    private bool _hasActiveTask;
    private bool _isTryingToGetTask;

    private Coroutine _taskGettingAttemptCoroutine;
    private bool _isSystemActive;

    PlayerTask _currentTask;
    List<PlayerTask> _playerTasks = new();
    List<PlayerTask> _completedTasks = new();
    #endregion

    #region Unity lifecycle methods
    void Update()
    {
        if(!_isSystemActive)
        TimeSinceLastTaskTimer();
    }

    public void Initialize(GameContext ctx)
    {
        _chanceToGetTask = _baseChanceToGetTask;

        foreach (PlayerTask task in _playerTasks)
            task.Initialize(ctx);
    }
    #endregion
    
    public void RestartTaskSystem()
    {
        _chanceToGetTask = _baseChanceToGetTask;
        _timeSinceLastTask = 0;
        _hasActiveTask = false;
        _isTryingToGetTask = true;
        SetSystemState(true);

        Debug.Log("Task system restarted and reset.");
    }

    public void SetSystemState(bool state) => _isSystemActive = state;

    public void GetRandomTask()
    {
        if(_taskGettingAttemptCoroutine != null)
            StopCoroutine(_taskGettingAttemptCoroutine);

        _hasActiveTask = true;
        _chanceToGetTask = _baseChanceToGetTask;

        if(_playerTasks.Count == 0)
        {
            _playerTasks.AddRange(_completedTasks);
            _completedTasks.Clear();
        }

        PlayerTask randomTask = _playerTasks[Random.Range(0, _playerTasks.Count)];

        _currentTask = randomTask;
        _playerTasks.Remove(randomTask);
        _completedTasks.Add(randomTask);

        Debug.Log($"Getting random task");
    }

    /// <summary>
    /// Accumulates time since last task completion. When the minimum amount of time is passed between tasks it calls the random task getter and stops the time accumulation. 
    /// </summary>
    void TimeSinceLastTaskTimer()
    {
        if(_isTryingToGetTask || _hasActiveTask)
            return;

        _timeSinceLastTask += Time.deltaTime;

        if(_timeSinceLastTask >= _minTimeBetweenTasks)
        {
            _timeSinceLastTask = 0;
            _isTryingToGetTask = true;
            _taskGettingAttemptCoroutine = StartCoroutine(TaskGettingAttempt());
        }
    }

    /// <summary>
    /// Tries to get a new task every _taskGettingAttemptFrequency seconds. 
    /// If it fails it increases the chances of success by _chanceIncreaseAfterFailedAttempt and retries it until it succeeds.
    /// /summary>
    /// <returns></returns>
    IEnumerator TaskGettingAttempt()
    {
        while(_isTryingToGetTask && !_hasActiveTask)
        {
            Debug.Log("Attempting to get a new task");
            float randomChance = Random.Range(0f,100f);
            if(randomChance <= _chanceToGetTask)
            {
                GetRandomTask();
                break;
            }

            Debug.Log("Failed to get a new task");
            _chanceToGetTask += _chanceIncreaseAfterFailedAttempt;
            yield return new WaitForSeconds(_taskGettingAttemptFrequency);
        }
    }

    public void OnTaskSuccess()
    {
        _hasActiveTask = false;
        _isTryingToGetTask = false;
        _chanceToGetTask = _baseChanceToGetTask;
        _currentTask.OnTaskSuccess();

        Debug.Log("Task is completed. Restarting task attempt timer.");
    }

    public void OnTaskFailed()
    {
        _hasActiveTask = false;
        _isTryingToGetTask = false;
        _chanceToGetTask = _baseChanceToGetTask;
        _currentTask.OnTaskFail();

        Debug.Log("Task is failed. Restarting task attempt timer.");
    }
}