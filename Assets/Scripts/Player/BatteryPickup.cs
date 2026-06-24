using UnityEngine;

public class BatteryPickup : MonoBehaviour
{
    [SerializeField] private Battery battery;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            battery.hasBattery = true;
            battery.batteryVisual.SetActive(true);
            battery.lightScript.StopIndicator();
        }
    }
}
