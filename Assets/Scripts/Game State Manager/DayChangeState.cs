using System.Collections;
using UnityEngine;

public class DayChangeState : GameState
{
    public override IEnumerator OnEnter()
    {
        _ctx.PoolManager.FadeInDayChangeMusic();

        _ctx.WorldHealthMeter.SetTimerState(false);
        _ctx.DayTimeController.SetIsTimerOn(false);
        
        _ctx.TaskManager.SetSystemState(false);
        _ctx.TaskManager.SetisTimerOn(false);
        _ctx.TaskManager.SetTaskTimerPanelState(false);
        _ctx.TaskManager.InterruptTask();

        _ctx.CoworkerManager.StopCoworkerMovement();
        
        // _ctx.PoolManager.GetSfx(AudioType.DayChangeClockSound);
        yield return _ctx.TransitionController.TransitionFadeIn();

        _ctx.PlayerControl.TeleportPlayerToStartingPosition();
        _ctx.GameStateController.ChangeState(StateType.Gameplay);
        yield return null;
    }

    public override IEnumerator OnExit()
    {
        // Day - Time
        if(_ctx.DayTimeController.CurrentDay == 5)
            _ctx.SettingsData.SetLevelFiveCleared();
            
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

        yield return new WaitForSeconds(2.5f);
        _ctx.PoolManager.FadeInGameplayMusic();
        yield return _ctx.TransitionController.TransitionFadeOut();

        // TaskManager
        _ctx.TaskManager.RestartTaskSystem();
        UiEffectHandler.BounceText(Type.Out, _ctx.DayTimeController.DayText, 1f, 8f);
        yield return null;
    }

    public override void Tick()
    {

    }
}