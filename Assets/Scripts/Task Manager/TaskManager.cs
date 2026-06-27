using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    #region Temporary

    #endregion

    #region Serialized fields
    [SerializeField] private float _minTimeBetweenTasks = 5;
    [SerializeField] private float _taskGettingAttemptFrequency;
    [SerializeField] private float _baseChanceToGetTask = 10;
    [SerializeField] private float _chanceIncreaseAfterFailedAttempt = 10;
    [SerializeField] private bool _isDebugOn = false;
    [Space(10)]
    [Header("Timer Data")]
    [SerializeField] private TaskManagerTimerData _timerData;
    #endregion
    #region Private fields
    private TaskManagerTimer _remainingTimeToTriggerCurrentTaskTimer;
    private float _chanceToGetTask;
    private float _timeSinceLastTask;

    private bool _hasActiveTask;
    private bool _isTryingToGetTask;

    private Coroutine _taskGettingAttemptCoroutine;
    private bool _isSystemActive; public bool IsSystemActive => _isSystemActive;

    PlayerTask _currentTask; public PlayerTask CurrentTask => _currentTask;
    List<PlayerTask> _playerTasks = new();
    List<PlayerTask> _completedTasks = new(); 

    #endregion

    #region Unity lifecycle methods
    void Update()
    {
        _remainingTimeToTriggerCurrentTaskTimer?.Tick();

        if(!_isSystemActive) return;
        TimeSinceLastTaskTimer();
    }

    public void Initialize(GameContext ctx)
    {       
        _playerTasks.AddRange(FindObjectsByType<PlayerTask>().ToList());
        _chanceToGetTask = _baseChanceToGetTask;

        _remainingTimeToTriggerCurrentTaskTimer = new();
        _remainingTimeToTriggerCurrentTaskTimer.Initialize(this,_timerData);

        foreach (PlayerTask task in _playerTasks)
            task.Initialize(ctx);

        // InitializeTaskTriggerObjectInstances(ctx);

        SetSystemState(false);
    }

    void InitializeTaskTriggerObjectInstances(GameContext ctx)
    {
        TaskTriggerObjectInstance[] ttoi = FindObjectsByType<TaskTriggerObjectInstance>();

        if(ttoi.Length > 0)
        {
            foreach (TaskTriggerObjectInstance instance in ttoi) instance.Initialize(ctx);
        }
    }
    #endregion
    
    public void RestartTaskSystem()
    {
        _chanceToGetTask = _baseChanceToGetTask;
        _timeSinceLastTask = 0;
        _currentTask = null;
        _hasActiveTask = false;
        _isTryingToGetTask = false;
        SetSystemState(true);

        if(_isDebugOn) Debug.Log("Task system restarted and reset.");
    }


    /// <summary>
    /// Stops the task system from getting new tasks.
    /// </summary>

    public void SetSystemState(bool state) => _isSystemActive = state;
    public void SetisTimerOn(bool state) => _remainingTimeToTriggerCurrentTaskTimer.SetIsTimerOn(state);
    public bool IsTimerOn() => _remainingTimeToTriggerCurrentTaskTimer.IsTimerOn;
    public void SetTaskGettingGapTime(float frequency) => _taskGettingAttemptFrequency = frequency;

    public void GetRandomTask()
    {
        if(!_isSystemActive) return;

        if(_taskGettingAttemptCoroutine != null)
            StopCoroutine(_taskGettingAttemptCoroutine);

        _hasActiveTask = true;
        _chanceToGetTask = _baseChanceToGetTask;

        if(_playerTasks.Count == 0)
        {
            if(_completedTasks.Count == 0)
            {
                if(_isDebugOn) Debug.Log("Attempted to get a task but no tasks are available. Tasks system turns off.");
                SetSystemState(false);
                return;
            }

            _playerTasks.AddRange(_completedTasks);
            _completedTasks.Clear();
        }

        PlayerTask randomTask = _playerTasks[Random.Range(0, _playerTasks.Count)];

        _currentTask = randomTask;
        _playerTasks.Remove(randomTask);
        _completedTasks.Add(randomTask);
        _currentTask.OnTaskAnnouncement();
        _remainingTimeToTriggerCurrentTaskTimer.StartTimer(_currentTask.AvailableTime, _currentTask.TaskDescription);

        if(_isDebugOn) Debug.Log($"New Task selected: {randomTask.TaskName}");
    }

    /// <summary>
    /// Accumulates time since last task completion. When the minimum amount of time is passed between tasks it calls the random task getter and stops the time accumulation. 
    /// </summary>
    void TimeSinceLastTaskTimer()
    {
        if(_isTryingToGetTask || _hasActiveTask || !_isSystemActive)
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
            if(_isDebugOn) Debug.Log("Attempting to get a new task");
            float randomChance = Random.Range(0f,100f);
            if(randomChance <= _chanceToGetTask)
            {
                GetRandomTask();
                break;
            }

            if(_isDebugOn) Debug.Log("Failed to get a new task");
            _chanceToGetTask += _chanceIncreaseAfterFailedAttempt;
            yield return new WaitForSeconds(_taskGettingAttemptFrequency);
        }
    }

    public void BeginTask()
    {
        SetSystemState(false);
        _currentTask.OnTaskStart();
    }

    public void OnTaskEnd()
    {

        if(_currentTask == null) return;
        if(_currentTask.IsSuccess)
            _currentTask.OnTaskSuccess();
        else
            _currentTask.OnTaskFail();

        _currentTask.DisableTriggerObj();
        _currentTask.SetTaskPanelState(false);
        SetSystemState(true);
        _currentTask = null;
        _remainingTimeToTriggerCurrentTaskTimer.SetIsTimerOn(false);
        _remainingTimeToTriggerCurrentTaskTimer.SetPanelState(false);
        RestartTaskSystem();
    }

    public void InterruptTask()
    {
        if(_currentTask != null)
        {
            if(_currentTask.IsSuccess)
                _currentTask.OnTaskSuccess(false);
            else
                _currentTask.OnTaskFail(false);

            _currentTask.DisableTriggerObj();
            _currentTask.SetTaskPanelState(false);

            _currentTask = null;
        }

        _remainingTimeToTriggerCurrentTaskTimer.SetIsTimerOn(false);
        _remainingTimeToTriggerCurrentTaskTimer.SetPanelState(false);
    }

    public void SetTaskTimerPanelState(bool state)
    {
        _remainingTimeToTriggerCurrentTaskTimer.SetPanelState(state);
    }

    public void OnTimeOut()
    {
        if(_currentTask != null) 
            _currentTask.OnTaskEnd(false);
    }
}