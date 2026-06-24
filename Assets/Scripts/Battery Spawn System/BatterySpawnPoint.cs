using UnityEngine;

public class BatterySpawnPoint : MonoBehaviour
{
    [SerializeField] private bool isAvailable = true;
    [SerializeField] private Transform spawnTransform;

    private BatteryPickup _currentBattery;
    public bool IsFree => isAvailable && _currentBattery == null;

    private void Start()
    {
        if(spawnTransform == null)
        {
            spawnTransform = transform;
        }
    }
    public Vector3 GetPosition()
    {
        return spawnTransform != null ? spawnTransform.position : transform.position;
    }
    public Quaternion GetRotation()
    {
        return spawnTransform != null ? spawnTransform.rotation : transform.rotation;
    }

    public void Occupy(BatteryPickup battery)
    {
        _currentBattery = battery;
    }
    public void Clear()
    {
        _currentBattery = null;
    }
    public void SetAvailable(bool value)
    {
        isAvailable = value;
    }
}