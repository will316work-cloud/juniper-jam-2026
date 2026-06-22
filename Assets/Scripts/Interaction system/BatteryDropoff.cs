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
            _ctx.Battery.SwapBattery();
            _ctx.MoneyController.GainMoney(_ctx.Battery.moneyPerDropoff);
            _ctx.WorldHealthMeter.GainHealth(_ctx.Battery.healthPerDropoff);
            _ctx.Quota.DropOff();
        }
    }
}
