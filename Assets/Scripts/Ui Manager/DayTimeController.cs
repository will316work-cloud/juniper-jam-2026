using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayTimeController : MonoBehaviour
{
    public GameObject Panel;
    public int DayLenghtInSeconds;
    public TextMeshProUGUI DayText;
    public TextMeshProUGUI TimeText;

    [SerializeField] private BatterySpawner batterySpawner;

    private int _shiftStartingHour = 9;
    private int _shiftEndHour = 17;
    private float _timePassed;
    private float _ingameMinuteLengthInSeconds;
    private int _currentDay;
    private int _currentHour;
    private int _currentMinute;
    private bool _isTimerOn; public bool IsTimerOn => _isTimerOn;
    private bool _isPanelActive => Panel.activeSelf; public bool IsPanelActive => _isPanelActive;
    GameContext _ctx;

    public bool QuotaProtection;

    public void Initialize(GameContext ctx)
    {
        _ctx = ctx;
        _currentDay = 1;
        _ingameMinuteLengthInSeconds = (float)DayLenghtInSeconds / ((_shiftEndHour - _shiftStartingHour) * 60);

        if(batterySpawner == null)
            batterySpawner = Object.FindAnyObjectByType<BatterySpawner>();

        ResetTime();
        ResetDay();
    }

    public void ResetTime()
    {
        _timePassed = 0;
        _currentHour = _shiftStartingHour;
        _currentMinute = 0;

        UpdateTimeVisual();
    }

    public void ResetDay()
    {
        _currentDay = 1;
        UpdateDayVisual();
    }

    void UpdateTimeVisual()
    {
        if(_currentMinute < 10)
            TimeText.text = $"{_currentHour}:0{_currentMinute}";
        else
            TimeText.text = $"{_currentHour}:{_currentMinute}";
    }

    void UpdateDayVisual()
    {
        DayText.text = $"Day {_currentDay}";
    }

    void Timer()
    {
        if(!_isTimerOn) return;

        _timePassed += Time.deltaTime;

        if(_timePassed > _ingameMinuteLengthInSeconds)
        {
            _currentMinute++;

            if(_currentMinute >= 60)
            {
                _currentMinute = 0;
                _currentHour++;

                if(_currentHour >= _shiftEndHour)
                {
                    SetIsTimerOn(false);
                    GoNextDay();
                    return;
                }
            }

            UpdateTimeVisual();
            _timePassed = 0;
        }
    }

    public void IncrementDay()
    {
        _currentDay++;
        UpdateDayVisual();
        _ctx.Quota.ResetDroppedCount();
        _ctx.DifficultyManager.SetDifficulty(_currentDay);
    }

    public void SetIsTimerOn(bool state)
    {
        _isTimerOn = state;
        Debug.Log($"Day Time Timer is {_isTimerOn}");

        if(batterySpawner == null)
            return;

        if(state)
            batterySpawner.StartDaySpawning();
        else
            batterySpawner.StopDaySpawning();
    }

    public void SetPanelState(bool state) => Panel.SetActive(state);

    void Update()
    {
        Timer();
    }

    public void GoNextDay()
    {
        if(_ctx.TaskManager.CurrentTask != null) _ctx.TaskManager.CurrentTask.IsSuccess = true;
        if(_ctx.Quota.batteriesDroppedCount < _ctx.Quota.quotaAmount && !QuotaProtection)
        {
            _ctx.GameStateController.LooseReason = LooseReason.Quota;
            _ctx.GameStateController.ChangeState(StateType.GameOver);
            return;
        }
        _ctx.GameStateController.ChangeState(StateType.DayChange);
    }
}