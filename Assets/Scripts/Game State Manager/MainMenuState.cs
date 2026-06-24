using System.Collections;

public class MainMenuState : GameState
{
    public override IEnumerator OnEnter()
    {
        _ctx.TaskManager.SetSystemState(false);
        _ctx.TaskManager.InterruptTask();
        _ctx.TaskManager.SetisTimerOn(false);
        _ctx.TaskManager.SetTaskTimerPanelState(false);

        _ctx.CoworkerManager.ResetSystem();
        _ctx.CoworkerManager.SetCoworkerMoverState(false);

        _ctx.UiManager.IngameMenuHandler.SetCanOpenInGameMenueState(false);
        _ctx.UiManager.IngameMenuHandler.SetPanelState(false);
        _ctx.UiManager.InGameUiHandler.SetPanelState(false);
        _ctx.UiManager.GameOverUiHandler.SetPanelState(false);

        _ctx.WorldHealthMeter.SetSystemIsEnabled(false);
        _ctx.WorldHealthMeter.SetTimerState(false);
        _ctx.WorldHealthMeter.SetPanelState(false);

        _ctx.MoneyController.ResetMoney();
        _ctx.MoneyController.SetPanelState(false);

        _ctx.Battery.ResetBatteryFill();
        
        _ctx.Quota.ResetDroppedCount();

        _ctx.DayTimeController.SetIsTimerOn(false);
        _ctx.DayTimeController.SetPanelState(false);
        _ctx.DayTimeController.ResetTime();
        _ctx.DayTimeController.ResetDay();

        _ctx.UiManager.MainMenuHandler.SetPanelState(true);
        _ctx.UiManager.CreditsHandler.SetPanelState(false);

        yield return null;
    }

    public override IEnumerator OnExit()
    {
        _ctx.UiManager.MainMenuHandler.SetPanelState(false);
        _ctx.PlayerControl.RemoveMovementBlockReason(MovementBlockReason.Menu);
        yield return null;
    }

    public override void Tick()
    {
    }
}