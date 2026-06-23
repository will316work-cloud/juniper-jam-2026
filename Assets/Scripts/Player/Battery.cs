using UnityEngine;

public class Battery : MonoBehaviour
{
    public GameObject batteryVisual;
    [SerializeField] private DebugBatteryUI batteryUI;
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
        }
    }

    private void IncreaseVisualFill()
    {
        if (_isFilled) return;
        if(IsDebugOn) Debug.Log("Battery is " + (float)rotationsSoFar / (float)rotationsPerFill * 100 + " percent full");
        batteryUI.ChangeBatteryText(rotationsSoFar);
        //visual element changes here (soFar / perFill) amount
    }

    private void DecreaseVisualFill()
    {
        if(IsDebugOn) Debug.Log("New Battery Visual");
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
    }

    public void ResetBatteryFill()
    {
        rotationsSoFar = 0;
        _isFilled = false;
        _ctx.BatteryDropoff.canInteract = false;
        DecreaseVisualFill();
    }

}
