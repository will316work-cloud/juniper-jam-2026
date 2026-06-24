using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatterySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BatteryPool batteryPool;
    [SerializeField] private DayTimeController dayTime;
    [SerializeField] private List<BatterySpawnPoint> spawnPoints = new List<BatterySpawnPoint>();

    [Header("Spawn Settings")]
    public int batteriesPerDay = 8;

    [Tooltip("No batteries will spawn during the last X seconds of the day")]
    [SerializeField] private float endOfDay = 60f;

    private Coroutine _spawnRoutine;

    public void StartDaySpawning()
    {
        if(_spawnRoutine != null)
            StopCoroutine(_spawnRoutine);

        _spawnRoutine = StartCoroutine(SpawnRoutine());
    }

    public void StopDaySpawning()
    {
        if(_spawnRoutine != null)
        {
            StopCoroutine(_spawnRoutine);
            _spawnRoutine = null;
        }
    }

    private IEnumerator SpawnRoutine()
    {
        if(batteriesPerDay <= 0)
            yield break;

        float safeDayLength = Mathf.Max(1f, dayTime.DayLenghtInSeconds - endOfDay);
        float interval = safeDayLength / batteriesPerDay;

        for(int i = 0; i < batteriesPerDay; i++)
        {
            yield return new WaitForSeconds(interval);

            if(!dayTime.IsTimerOn)
                yield break;

            SpawnOneBattery();
        }

        _spawnRoutine = null;
    }

    private void SpawnOneBattery()
    {
        List<BatterySpawnPoint> availablePoints = GetAvailablePoints();

        if(availablePoints.Count == 0)
        {
            Debug.Log("No battery spawn points available");
            return;
        }

        BatterySpawnPoint selectedPoint = availablePoints[Random.Range(0, availablePoints.Count)];
        BatteryPickup battery = batteryPool.GetBattery();
        battery.SpawnAt(selectedPoint);
    }

    private List<BatterySpawnPoint> GetAvailablePoints()
    {
        List<BatterySpawnPoint> availablePoints = new List<BatterySpawnPoint>();

        foreach(BatterySpawnPoint point in spawnPoints)
        {
            if(point != null && point.IsFree)
                availablePoints.Add(point);
        }
        return availablePoints;
    }
}