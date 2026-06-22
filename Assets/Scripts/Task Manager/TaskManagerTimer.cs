using DG.Tweening;
using TMPro;
using UnityEngine;

public class TaskManagerTimer : IUiHandler
{
    private TaskManager _taskManager;
    private GameObject _panel;
    private TextMeshProUGUI _timerText;
    private float _timePassed;
    private float _availableTime;
    private bool _isTimerOn; public bool IsTimerOn => _isTimerOn;
    private string _currentTaskDescription;

    public void Initialize(TaskManager taskManager, TaskManagerTimerData data)
    {
        _taskManager = taskManager;
        _panel = data.Panel;
        _timerText = data.TimerText;

        SetPanelState(false);
    }

    public void Tick()
    {
        if(!_isTimerOn) return;

        _timePassed += Time.deltaTime;

        if(_timePassed >= 0.1f)
        {
            _availableTime -= _timePassed;
            _timePassed = 0;
            UpdateTimerVisual();

            if(_availableTime <= 0)
            {
                _isTimerOn = false;
                SetPanelState(false);
                _taskManager.OnTimeOut();
            }
        }
    }   

    public void StartTimer(float availableTime, string taskDescription)
    {
        _availableTime = availableTime;
        _currentTaskDescription = taskDescription;
        _timePassed = 0;
        _isTimerOn = true;

        _timerText.transform.DOScale(1.02f, 0.3f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);

        UpdateTimerVisual();
        SetPanelState(true);
    }

    void UpdateTimerVisual() => _timerText.text = _currentTaskDescription + "\nRemaining task time: " + _availableTime.ToString("0.0");

    /// <summary>
    /// Sets the time available for the task.
    /// </summary>
    public void SetAvailableTime(float time) => _availableTime = time;
    public void SetPanelState(bool state) => _panel.SetActive(state);
    public void SetIsTimerOn(bool state)
    {
        _isTimerOn = state;
        _timerText.transform.DOKill();
        _timerText.transform.localScale = Vector3.one;
    }
    public bool IsPanelActive() => _panel.activeSelf;
}

[System.Serializable]
public class TaskManagerTimerData
{
    public TextMeshProUGUI TimerText;
    public GameObject Panel;
}