using System.Collections;
using UnityEngine;

public class GameplayState : GameState
{
    public override IEnumerator OnEnter()
    {
        _ctx.WorldHealthMeter.SetSystemIsEnabled(true);
        _ctx.WorldHealthMeter.SetTimerState(true);
        _ctx.DayTimeController.SetIsTimerOn(true);
        _ctx.DayTimeController.SetPanelState(true);

        _ctx.PlayerControl.enabled = true;
        _ctx.UiManager.InGameUiHandler.SetPanelState(true);

        _ctx.CoworkerManager.SetCoworkerMoverState(false);
        _ctx.CoworkerManager.ContinueMovingCoworkersMovement();

        yield return new WaitForSeconds(0.5f);

        _ctx.CoworkerManager.SetCoworkerMoverState(true);
        _ctx.TaskManager.SetSystemState(true);
        _ctx.TaskManager.RestartTaskSystem();

        yield return null;
    }

    public override IEnumerator OnExit()
    {
        _ctx.PlayerControl.enabled = false;
        _ctx.UiManager.InGameUiHandler.SetPanelState(false);
        _ctx.TaskManager.SetSystemState(false);
        _ctx.CoworkerManager.SetCoworkerMoverState(false);
        _ctx.CoworkerManager.StopCoworkerMovement();

        yield return null;
    }

    public override void Tick()
    {
        
    }
}