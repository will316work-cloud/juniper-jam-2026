using System.Collections;
using UnityEngine;

public class GameplayState : GameState
{
    public override IEnumerator OnEnter()
    {
        if(_ctx.WorldHealthMeter.IsSystemActive == false)
        {
            _ctx.WorldHealthMeter.SetSystemIsEnabled(true);
            _ctx.WorldHealthMeter.SetTimerState(true);
        }

        if(_ctx.DayTimeController.IsTimerOn == false)
            _ctx.DayTimeController.SetIsTimerOn(true);

        if(_ctx.DayTimeController.IsPanelActive == false)
            _ctx.DayTimeController.SetPanelState(true);

        _ctx.PlayerControl.enabled = true;
        _ctx.UiManager.InGameUiHandler.SetPanelState(true);

        _ctx.CoworkerManager.ContinueMovingCoworkersMovement();

        yield return new WaitForSeconds(0.5f);

        if(_ctx.CoworkerManager.IsCoworkerMoverActive == false)
            _ctx.CoworkerManager.SetCoworkerMoverState(true);

        _ctx.TaskManager.SetSystemState(true);


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