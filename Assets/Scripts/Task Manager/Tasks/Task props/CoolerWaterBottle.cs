using UnityEngine;
using UnityEngine.InputSystem;

public class CoolerWaterBottle : TaskProp
{
    [Header("Animation")]
    public Animator BottleAnimator;

    [Tooltip("Animator state name")]
    public string AnimationState = "BottlePull";

    [Tooltip("Mouse pixels needed to complete one animation cycle")]
    public float PullDistancePerCycle = 600f;

    [Tooltip("Finish after this many completed cycles")]
    public int RequiredCycles = 5;

    private bool _isHolding;
    private bool _taskFinished;
    private Vector2 _previousMouse;
    private float _progress;
    private int _completedCycles;

    public override void Initialize(GameContext ctx)
    {
        base.Initialize(ctx);

        if (BottleAnimator != null)
            BottleAnimator.speed = 0f;
    }

    public override void OnTaskStart()
    {
        ResetTaskState();
    }

    public override void PointerDownHandler()
    {
        if (_taskFinished)
            return;

        _isHolding = true;
        if (Mouse.current != null)
            _previousMouse = Mouse.current.position.ReadValue();
    }

    public override void PointerUpHandler()
    {
        _isHolding = false;
    }

    private void Update()
    {
        if (!_isHolding || _taskFinished || Mouse.current == null || BottleAnimator == null)
            return;

        Vector2 currentMouse = Mouse.current.position.ReadValue();
        float deltaLeft = _previousMouse.x - currentMouse.x;

        if (deltaLeft > 0f)
        {
            _progress += deltaLeft / PullDistancePerCycle;
            while (_progress >= 1f)
            {
                _progress -= 1f;
                _completedCycles++;
                if (_completedCycles >= RequiredCycles)
                {
                    _ctx.TaskManager.CurrentTask.OnTaskEnd(true);
                    ResetTaskState();
                    return;
                }
            }
            BottleAnimator.Play(AnimationState, 0, _progress);
            BottleAnimator.speed = 0f;
        }
        _previousMouse = currentMouse;
    }

    private void ResetTaskState()
    {
        _isHolding = false;
        _taskFinished = false;
        _progress = 0f;
        _completedCycles = 0;
        if (BottleAnimator != null)
        {
            BottleAnimator.Play(AnimationState, 0, 0f);
            BottleAnimator.speed = 0f;
        }
    }
}