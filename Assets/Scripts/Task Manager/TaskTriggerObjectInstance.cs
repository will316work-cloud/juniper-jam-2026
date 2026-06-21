public class TaskTriggerObjectInstance : InteractableObjectInstance
{
    GameContext _ctx;

    public void Initialize(GameContext ctx)
    {
        _ctx = ctx;
    }

    public override void Interact()
    {
        canInteract = false;
        _ctx.GameStateController.ChangeState(StateType.PlayerTask);
    }
}