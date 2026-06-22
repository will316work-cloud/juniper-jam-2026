using System.Collections;

public class DayChangeState : GameState
{
    public override IEnumerator OnEnter()
    {
        _ctx.WorldHealthMeter.SetTimerState(false);
        _ctx.DayTimeController.SetIsTimerOn(false);
        yield return _ctx.TransitionController.TransitionFadeIn();
        _ctx.GameStateController.ChangeState(StateType.Gameplay);
        yield return null;
    }

    public override IEnumerator OnExit()
    {
        _ctx.DayTimeController.IncrementDay();
        _ctx.DayTimeController.ResetTime();
        _ctx.WorldHealthMeter.ResetHealth();
        _ctx.CoworkerManager.TeleportCoworkersToOriginalPlace();
        yield return _ctx.TransitionController.TransitionFadeOut();
        yield return null;
    }

    public override void Tick()
    {

    }
}