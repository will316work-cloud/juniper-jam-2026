using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DifficultyManager : MonoBehaviour
{
    public List<DayDifficultySetting> DaySettings = new();
    public bool IsDebugOn;
    private GameContext _gameContext;
    private DayDifficultySetting _currentSetting;

    public void Initialize(GameContext gameContext)
    {
        _gameContext = gameContext;
    }
        
    public void SetDifficulty(int day)
    {
        if(IsDebugOn) Debug.Log($"Setting difficulty for day {day}.");
        if (day > DaySettings.Count) day = DaySettings.Count;
        ApplySettings(GetSettingsForDay(day));
    }

    void ApplySettings(DayDifficultySetting setting)
    {
        _gameContext.CoworkerManager.SetCoworkerMovementSpeed(setting.CoworkerSpeed);
        _gameContext.CoworkerManager.SetMaximumConcurrentMovingCoworkers(setting.MaxConcurrentMovingCoworker);
        _gameContext.CoworkerManager.SetMoverGapTime(setting.CoworkerMoverGapTime);
        _gameContext.TitleCoworkerManager.SetCoworkerMovementSpeed(6f);
        _gameContext.TitleCoworkerManager.SetMaximumConcurrentMovingCoworkers(3);
        _gameContext.TitleCoworkerManager.SetMoverGapTime(0.25f);
        _gameContext.TaskManager.SetTaskGettingGapTime(setting.TaskGapTime);
        _gameContext.WorldHealthMeter.SetHealthLossPerSecond(setting.HealthLossPerSecond);
        _gameContext.Quota.SetQuota(setting.BatteryQuotaCountPerDay);

        if(IsDebugOn) Debug.Log($"Day {setting.Day} settings applied.");
    }

    DayDifficultySetting GetSettingsForDay(int day)
    {
        foreach(DayDifficultySetting setting in DaySettings)
        {
            if(setting.Day == day)
            {
                _currentSetting = setting;
                return setting;
            }
        }

        return _currentSetting;
    }
}

[Serializable]
public class DayDifficultySetting
{
    public int Day;
    public int MaxConcurrentMovingCoworker;
    public float CoworkerSpeed;
    public float CoworkerMoverGapTime;
    public float TaskGapTime;
    public float HealthLossPerSecond;
    public int BatteryQuotaCountPerDay;
}