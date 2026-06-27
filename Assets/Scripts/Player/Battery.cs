using System.Collections;
using UnityEngine;

public class Battery : MonoBehaviour
{
    public GameObject batteryVisual;
    [SerializeField] private GameObject _batteryFill;
    [SerializeField] private Material _batteryOrange;
    [SerializeField] private Material _batteryGreen;
    [SerializeField] private DebugBatteryUI batteryUI;
    [SerializeField] private GameObject pickupVisual;
    [SerializeField] private GameObject _droppedBatteryVisual;
    [SerializeField] private GameObject _droppedBatteryTargetLocation;
    [SerializeField] private float _droppedBatteryAnimationDuration;
    private Vector3 _droppedBatteryOGLocation;
    private MeshRenderer _batteryRenderer;
    public TaskTriggerLight lightScript;
    public int moneyPerDropoff;
    public int healthPerDropoff;
    public int rotationsPerFill;
    public int rotationsSoFar = 0;
    public bool _isFilled;
    public bool IsDebugOn;
    public bool hasBattery = true;
    private GameContext _ctx;

    public void Initialize(GameContext ctx)
    {
        _ctx = ctx;
        lightScript.Initialize();
        _batteryRenderer = _batteryFill.GetComponent<MeshRenderer>();
        _droppedBatteryOGLocation = _droppedBatteryVisual.transform.position;
    }

    public void ChargeBattery()
    {
        if (_isFilled || !hasBattery) return;
        rotationsSoFar++;
        IncreaseVisualFill();
        if (rotationsSoFar >= rotationsPerFill)
        {
            _isFilled = true;
            _ctx.BatteryDropoff.canInteract = true;
            _batteryRenderer.material = _batteryGreen;
            _ctx.BatteryDropoff.StartLightIndication();
            _ctx.PoolManager.GetSfx(AudioType.BatteryIsChargedAlert);
        }
    }

    private void IncreaseVisualFill()
    {
        if (_isFilled) return;
        if(IsDebugOn) Debug.Log("Battery is " + (float)rotationsSoFar / (float)rotationsPerFill * 100 + " percent full");
        _batteryFill.transform.localScale = new Vector3(1, (float)rotationsSoFar / (float)rotationsPerFill, 1);
        batteryUI.ChangeBatteryText(rotationsSoFar);
        //visual element changes here (soFar / perFill) amount
    }

    public void DecreaseVisualFill()
    {
        if(IsDebugOn) Debug.Log("New Battery Visual");
        _batteryRenderer.material = _batteryOrange;
        _batteryFill.transform.localScale = new Vector3(1, (float)rotationsSoFar / (float)rotationsPerFill, 1);
        batteryUI.ChangeBatteryText(rotationsSoFar);
        //visual element changes here (soFar / perFill) amount
    }

    public void SwapBattery()
    {
        if(!_isFilled) return;
        rotationsSoFar = 0;
        DecreaseVisualFill();
        hasBattery = false;
        batteryVisual.SetActive(false);
        if (IsDebugOn) Debug.Log("Battery swapped");
        _isFilled = false;
        _ctx.BatteryDropoff.canInteract = false;
        lightScript.StartIndicator();
        pickupVisual.SetActive(true);
        StartCoroutine(DroppedBatteryAnimation(_droppedBatteryAnimationDuration));
    }

    IEnumerator DroppedBatteryAnimation(float duration) {
        _droppedBatteryVisual.SetActive(true);
        //_droppedBatteryVisual.transform.position = _droppedBatteryOGLocation.position;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float normalizedTime = t / duration;
            _droppedBatteryVisual.transform.position = Vector3.Lerp(_droppedBatteryOGLocation, _droppedBatteryTargetLocation.transform.position, normalizedTime);
            yield return null;
        }
        _droppedBatteryVisual.SetActive(false);
        _droppedBatteryVisual.transform.localPosition = _droppedBatteryOGLocation;
    }

    public void ResetBatteryFill()
    {
        rotationsSoFar = 0;
        _isFilled = false;
        _ctx.BatteryDropoff.canInteract = false;
        DecreaseVisualFill();
    }

}
