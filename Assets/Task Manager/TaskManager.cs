using System.Collections;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        
        else
            Destroy(gameObject);
    }

    public static TaskManager Instance;


    [SerializeField] private float _minTimeBetweenTasks = 5;
    [SerializeField] private float _taskGettingAttemptFrequency;
    [SerializeField] private float _baseChanceToGetTask = 10;
    [SerializeField] private float _chanceIncreaseAfterFailedAttempt = 10;

    private float _chanceToGetTask;
    private float _timeSinceLastTask;

    private bool _hasActiveTask;
    private bool _isTryingToGetTask;

    private Coroutine _taskGettingAttemptCoroutine;

    void Start()
    {
        _chanceToGetTask = _baseChanceToGetTask;
    }

    void Update()
    {
        TimeSinceLastTaskTimer();
    }

    public void GetRandomTask()
    {
        if(_taskGettingAttemptCoroutine != null)
            StopCoroutine(_taskGettingAttemptCoroutine);

        _hasActiveTask = true;
        _chanceToGetTask = _baseChanceToGetTask;

        Debug.Log("Getting random task");
    }

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

    public void OnTaskComplete()
    {
        _hasActiveTask = false;
        _isTryingToGetTask = false;
        _chanceToGetTask = _baseChanceToGetTask;
        Debug.Log("Task is completed. Restarting task attempt timer.");
    }
}