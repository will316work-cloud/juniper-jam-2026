using System.Collections;
using UnityEngine;

public class GameplayState : GameState
{
    public override IEnumerator OnEnter()
    {
        Time.timeScale = 1f;
        _ctx.CameraController.SwitchToCamera(CameraType.Gameplay);
        CursorHandler.SetCursorVisible(false);
        _ctx.UiManager.IngameMenuHandler.SetCanOpenInGameMenueState(true);
        _ctx.GameStateController.IsPlayerDead = false;
        _ctx.CameraController.SetIsDepthOfFieldEnabled(false);

        _ctx.WorldHealthMeter.SetSystemIsEnabled(true);
        _ctx.WorldHealthMeter.SetTimerState(true);

        _ctx.DayTimeController.SetIsTimerOn(true);
        _ctx.DayTimeController.SetPanelState(true);

        _ctx.PlayerControl.RemoveMovementBlockReason(MovementBlockReason.StateChange);
        _ctx.PlayerControl.RemoveMovementBlockReason(MovementBlockReason.GameOver);
        _ctx.PlayerControl.RemoveMovementBlockReason(MovementBlockReason.Menu);
        
        _ctx.UiManager.InGameUiHandler.SetPanelState(true);

        _ctx.CoworkerManager.SetCoworkerMoverState(false);
        _ctx.CoworkerManager.ContinueMovingCoworkersMovement();

        yield return new WaitForSeconds(0.5f);

        _ctx.CoworkerManager.SetCoworkerMoverState(true);

        _ctx.TaskManager.SetSystemState(true);
        _ctx.TaskManager.RestartTaskSystem();

        _ctx.MoneyController.SetPanelState(true);

        _ctx.UiManager.InGameUiHandler.SetPanelState(true);
        _ctx.UiManager.MainMenuHandler.SetPanelState(false);
        yield return null;
    }

    public override IEnumerator OnExit()
    {
        _ctx.PlayerControl.AddMovementBlockReason(MovementBlockReason.StateChange);
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