using System.Collections;

public class GameOverState : GameState
{
    public override IEnumerator OnEnter()
    {
        _ctx.UiManager.GameOverUiHandler.SetPanelState(true);
        _ctx.PlayerControl.enabled = false;
        _ctx.DayTimeController.SetIsTimerOn(false);
        yield return null;
    }

    public override IEnumerator OnExit()
    {
        _ctx.UiManager.GameOverUiHandler.SetPanelState(false);
        _ctx.DayTimeController.ResetTime();
        _ctx.DayTimeController.ResetDay();
        yield return null;
    }

    public override void Tick()
    {
    }
}