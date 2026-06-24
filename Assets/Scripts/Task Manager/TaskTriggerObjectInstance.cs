using UnityEngine;

public class TaskTriggerObjectInstance : InteractableObjectInstance
{
    GameContext _ctx;
    TaskTriggerLight _light;

    public void StartLightIndication() => _light.StartIndicator();
    public void StopLightIndication() => _light.StopIndicator();

    public void Initialize(GameContext ctx)
    {
        _light = GetComponentInChildren<TaskTriggerLight>();
        _light.Initialize();
        _ctx = ctx;
    }

    public override void Interact()
    {
        canInteract = false;
        _ctx.GameStateController.ChangeState(StateType.PlayerTask);
    }
}