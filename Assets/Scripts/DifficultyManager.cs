using System;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public List<DayDifficultySetting> DaySettings = new();
    private GameContext _gameContext;

    public void Initialize(GameContext gameContext)
    {
        _gameContext = gameContext;
    }
        
    public void SetDifficulty(int day)
    {
        ApplySettings(DaySettings.Find(setting => setting.Day == day));
    }

    void ApplySettings(DayDifficultySetting setting)
    {
        _gameContext.CoworkerManager.SetCoworkerMovementSpeed(setting.CoworkerSpeed);
        _gameContext.CoworkerManager.SetMaximumConcurrentMovingCoworkers(setting.MaxConcurrentMovingCoworker);
        _gameContext.CoworkerManager.SetMoverGapTime(setting.CoworkerMoverGapTime);
        _gameContext.TaskManager.SetTaskGettingGapTime(setting.TaskGapTime);
        _gameContext.WorldHealthMeter.SetHealthLossPerSecond(setting.HealthLossPerSecond);
        _gameContext.Quota.quotaAmount = setting.ChargedBatteryQuotaCount;

        Debug.Log($"Day {setting.Day} settings applied.");
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
    public int ChargedBatteryQuotaCount;
}