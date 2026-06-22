using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    #region Temporary
    public GameObject PrinterUi;
    public void SetPrinterUiState(bool state) => PrinterUi.SetActive(state);
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
    private TaskManagerTimer _timer;
    private float _chanceToGetTask;
    private float _timeSinceLastTask;

    private bool _hasActiveTask;
    private bool _isTryingToGetTask;

    private Coroutine _taskGettingAttemptCoroutine;
    private bool _isSystemActive;

    PlayerTask _currentTask; public PlayerTask CurrentTask => _currentTask;
    List<PlayerTask> _playerTasks = new();
    List<PlayerTask> _completedTasks = new(); 

    GameContext _ctx;
    #endregion

    #region Unity lifecycle methods
    void Update()
    {
        _currentTask?.Tick();
        _timer?.Tick();

        if(!_isSystemActive) return;
        TimeSinceLastTaskTimer();
    }

    public void Initialize(GameContext ctx)
    {       
        _ctx = ctx;
        _playerTasks.AddRange(FindObjectsByType<PlayerTask>().ToList());
        _chanceToGetTask = _baseChanceToGetTask;

        _timer = new();
        _timer.Initialize(this,_timerData);

        foreach (PlayerTask task in _playerTasks)
        {
            task.Initialize(ctx);
        }

        SetSystemState(false);
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

    public void SetSystemState(bool state) => _isSystemActive = state;

    public void GetRandomTask()
    {
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
        _timer.StartTimer(_currentTask.AvailableTime);

        if(_isDebugOn) Debug.Log($"New Task selected: {randomTask.TaskName}");
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
        if(_currentTask.IsSuccess)
            _currentTask.OnTaskSuccess();
        else
            _currentTask.OnTaskFail();

        _currentTask.DisableTriggerObj();
        SetSystemState(true);
        SetPrinterUiState(false);
        _currentTask = null;
        _timer.SetIsTimerOn(false);
        _timer.SetPanelState(false);
    }

    public void OnTimeOut()
    {
        _currentTask.OnTaskEnd(false);
    }
}