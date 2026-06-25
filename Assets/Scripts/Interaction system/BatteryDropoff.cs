using UnityEngine;

public class BatteryDropoff : InteractableObjectInstance
{
    GameContext _ctx;

    public void Initialize(GameContext ctx)
    {
        _ctx = ctx;
    }

    public override void Interact()
    {
        if(_ctx.Battery._isFilled) {
            Debug.Log("Battery Dropped Off");
            _ctx.PoolManager.GetSfx(AudioType.BatterySwap);
            _ctx.Battery.SwapBattery();
            _ctx.MoneyController.GainMoney(_ctx.Battery.moneyPerDropoff);
            _ctx.WorldHealthMeter.GainHealth(_ctx.Battery.healthPerDropoff,true);
            _ctx.Quota.DropOff();
            _ctx.BatteryPickup.SetBatteryPickupIsVisible(true);
        }
    }
}
