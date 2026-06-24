using System.Collections.Generic;
using UnityEngine;

public class BatteryPool : MonoBehaviour
{
    [SerializeField] private BatteryPickup batteryPrefab;
    [SerializeField] private int initialPoolSize = 10;
    private readonly Queue<BatteryPickup> _pool = new Queue<BatteryPickup>();

    private void Awake()
    {
        for(int i = 0; i < initialPoolSize; i++)
        {
            BatteryPickup battery = CreateNewBattery();
            battery.gameObject.SetActive(false);
            _pool.Enqueue(battery);
        }
    }

    private BatteryPickup CreateNewBattery()
    {
        BatteryPickup battery = Instantiate(batteryPrefab, transform);
        battery.Initialize(this);
        return battery;
    }
    public BatteryPickup GetBattery()
    {
        BatteryPickup battery = _pool.Count > 0 ? _pool.Dequeue() : CreateNewBattery();
        battery.gameObject.SetActive(true);
        return battery;
    }

    public void ReturnBattery(BatteryPickup battery)
    {
        battery.gameObject.SetActive(false);
        battery.transform.SetParent(transform);
        _pool.Enqueue(battery);
    }
}