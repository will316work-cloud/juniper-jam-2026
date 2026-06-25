using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BatteryPickup : MonoBehaviour
{

    [SerializeField] private Battery battery;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            battery.hasBattery = true;
            battery.batteryVisual.SetActive(true); 
        }
    }

    // private BatteryPool _pool;
    // private BatterySpawnPoint _spawnPoint;
    // private Rigidbody _rb;
    // private bool _collected;

    // private void Awake()
    // {
    //     _rb = GetComponent<Rigidbody>();
    // }

    // public void Initialize(BatteryPool pool)
    // {
    //     _pool = pool;
    // }

    // public void SpawnAt(BatterySpawnPoint spawnPoint)
    // {
    //     _spawnPoint = spawnPoint;
    //     _spawnPoint.Occupy(this);

    //     _collected = false;

    //     transform.SetPositionAndRotation(
    //         spawnPoint.GetPosition(),
    //         spawnPoint.GetRotation()
    //     );

    //     ResetPhysics();
    //     gameObject.SetActive(true);
    // }

    // private void OnTriggerEnter(Collider other)
    // {
    //     if(_collected) return;
    //     if(!other.CompareTag("Player")) return;

    //     Battery playerBattery = other.GetComponent<Battery>();
    //     if(playerBattery != null)
    //     {
    //         playerBattery.hasBattery = true;
    //         playerBattery.batteryVisual.SetActive(true);
    //     }
        
    //     if(playerBattery.hasBattery) return;
        
    //     Collect();
    // }

    // private void Collect()
    // {
    //     _collected = true;

    //     if(_spawnPoint != null)
    //     {
    //         _spawnPoint.Clear();
    //         _spawnPoint = null;
    //     }

    //     if(_pool != null)
    //         _pool.ReturnBattery(this);
    //     else
    //         gameObject.SetActive(false);
    // }

    // private void ResetPhysics()
    // {
    //     if(_rb != null)
    //     {
    //         _rb.linearVelocity = Vector3.zero;
    //         _rb.angularVelocity = Vector3.zero;
    //     }
    // }
}