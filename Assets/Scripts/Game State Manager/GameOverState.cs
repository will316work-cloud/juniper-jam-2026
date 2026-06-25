using System.Collections;

public class GameOverState : GameState
{
    public override IEnumerator OnEnter()
    {
        _ctx.PoolManager.FadeInGameOverMusic();
        _ctx.UiManager.IngameMenuHandler.SetCanOpenInGameMenueState(false);
        _ctx.GameStateController.IsPlayerDead = true;
        _ctx.CameraController.SetIsDepthOfFieldEnabled(true);
        _ctx.TaskManager.InterruptTask();
        _ctx.PoolManager.GetSfx(AudioType.GameOver);
        _ctx.UiManager.GameOverUiHandler.SetPanelState(true);
        _ctx.PlayerControl.AddMovementBlockReason(MovementBlockReason.GameOver);
        _ctx.CoworkerManager.StopCoworkerMovement();
        _ctx.TaskManager.SetSystemState(false);
        _ctx.TaskManager.OnTimeOut();
        _ctx.DayTimeController.SetIsTimerOn(false);
        _ctx.WorldHealthMeter.SetSystemIsEnabled(false);
        CursorHandler.SetCursorVisible(true);

        yield return null;
    }

    public override IEnumerator OnExit()
    {
        _ctx.PoolManager.FadeOutGameOverMusic();
        
        _ctx.CameraController.SetIsDepthOfFieldEnabled(false);
        _ctx.Quota.ResetDroppedCount();
        _ctx.UiManager.GameOverUiHandler.SetPanelState(false);
        _ctx.DifficultyManager.SetDifficulty(1);
        _ctx.PlayerControl.TeleportPlayerToStartingPosition();
        _ctx.Battery.ResetBatteryFill();
        _ctx.WorldHealthMeter.ResetHealth();
        _ctx.MoneyController.ResetMoney();
        _ctx.CoworkerManager.TeleportCoworkersToOriginalPlace();
        _ctx.DayTimeController.ResetTime();
        _ctx.DayTimeController.ResetDay();
        yield return null;
    }

    public override void Tick()
    {
    }
}