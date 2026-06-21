using System.Collections;

public class DayChangeState : GameState
{
    public override IEnumerator OnEnter()
    {
        // start transition and get to dark
        _ctx.WorldHealthMeter.SetTimerState(false);
        _ctx.GameStateController.ChangeState(StateType.Gameplay);
        yield return null;
    }

    public override IEnumerator OnExit()
    {
        _ctx.DayTimeController.IncrementDay();
        _ctx.DayTimeController.ResetTime();
        _ctx.WorldHealthMeter.ResetHealth();
        // end transition
        yield return null;
    }

    public override void Tick()
    {

    }
}