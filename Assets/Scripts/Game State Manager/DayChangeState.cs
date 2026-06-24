using System.Collections;

public class DayChangeState : GameState
{
    public override IEnumerator OnEnter()
    {
        _ctx.WorldHealthMeter.SetTimerState(false);
        _ctx.DayTimeController.SetIsTimerOn(false);
        
        _ctx.TaskManager.SetSystemState(false);
        _ctx.TaskManager.SetisTimerOn(false);
        _ctx.TaskManager.SetTaskTimerPanelState(false);
        _ctx.TaskManager.InterruptTask();

        _ctx.CoworkerManager.StopCoworkerMovement();
        
        _ctx.PoolManager.GetSfx(AudioType.DayChangeClockSound);
        yield return _ctx.TransitionController.TransitionFadeIn();

        _ctx.PlayerControl.TeleportPlayerToStartingPosition();
        _ctx.GameStateController.ChangeState(StateType.Gameplay);
        yield return null;
    }

    public override IEnumerator OnExit()
    {
        // Day - Time
        _ctx.DayTimeController.IncrementDay();
        _ctx.DayTimeController.ResetTime();

        // World Health
        _ctx.WorldHealthMeter.ResetHealth();

        // Coworkers
        _ctx.CoworkerManager.TeleportCoworkersToOriginalPlace();

        // Quota
        _ctx.Quota.ResetDroppedCount();

        // Battery
        _ctx.Battery.ResetBatteryFill();

        yield return _ctx.TransitionController.TransitionFadeOut();

        // TaskManager
        _ctx.TaskManager.RestartTaskSystem();
        yield return null;
    }

    public override void Tick()
    {

    }
}